// Copyright (c) itsdaliia <me@daliia.ch>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence.

using System.Text;
using boop.Core.Graphics;
using boop.Core.Input;
using boop.Core.Terminal;

namespace boop.Core;

public class Editor {
    private readonly List<StringBuilder> _lines = [new()];
    private readonly TerminalBuffer _terminalBuffer = new();
    private readonly ITerminalSession _terminalSession;

    private bool _terminalVisible;
    private string _terminalInput = "";

    private bool _ctrlDown;
    private string? _currentFilePath;

    private int _cursorLine;
    private int _cursorColumn;

    public Editor(IInputHandler input, ITextInput textInput, ITerminalSession terminalSession) {
        input.OnKeyDown += HandleInput;
        input.OnKeyUp += key => {
            if (key is Key.ControlLeft or Key.ControlRight) {
                _ctrlDown = false;
            }
        };

        textInput.OnCharTyped += HandleChar;
        terminalSession.CommandInterceptor += cmd => {
            if (cmd.StartsWith(":open", StringComparison.OrdinalIgnoreCase)) {
                string path = cmd.Substring(":open ".Length);
                if (OpenFile(path)) {
                    _terminalBuffer.Print($"[boop] opened {path}");
                }

                return true;
            }

            if (cmd.StartsWith(":close", StringComparison.OrdinalIgnoreCase)) {
                _terminalBuffer.Print($"[boop] closed {_currentFilePath}");
                _currentFilePath = null;

                _lines.Clear();
                _lines.Add(new StringBuilder());

                _cursorLine = 0;
                _cursorColumn = 0;
                return true;
            }

            return false;
        };

        terminalSession.OnOutput += line => {
            _terminalBuffer.Print(line);
        };

        _terminalSession = terminalSession;
    }

    public void Update(double deltaTime) {
        // todo: update logic
    }

    public void Render(IRenderer renderer) {
        renderer.Clear(new Color(30, 30, 40));

        const float x = 100;
        const float gutterWidth = 40;
        const float lineHeight = 24;
        const float fontSize = 20f;
        float y = 100;

        for (int i = 0; i < _lines.Count; i++) {
            renderer.DrawText((i + 1).ToString(), x, y, fontSize, Color.White);
            renderer.DrawText(_lines[i].ToString(), x + gutterWidth, y, fontSize, Color.White);
            y += lineHeight;
        }

        string beforeCursor = "";
        if (_cursorLine >= 0 && _cursorLine < _lines.Count && _cursorColumn > 0) {
            int len = Math.Min(_cursorColumn, _lines[_cursorLine].Length);
            beforeCursor = _lines[_cursorLine].ToString(0, len);
        }

        float textWidthBeforeCursor = renderer.MeasureTextWidth(beforeCursor, fontSize);
        float cursorX = x + gutterWidth + textWidthBeforeCursor;

        renderer.DrawRect(
            cursorX,
            80 + _cursorLine * lineHeight,
            2,
            lineHeight,
            Color.White
        );

        if (_terminalVisible) {
            renderer.DrawRect(0, renderer.Height - 300, renderer.Width, 300, new Color(0,0,0,180));

            float yt = renderer.Height - 280;
            foreach (string line in _terminalBuffer.Lines) {
                renderer.DrawText(line, 10, yt, 14, Color.White);
                yt += 16;
            }

            renderer.DrawText("> " + _terminalInput, 10, yt, 14, Color.White);
        }
    }

    private bool OpenFile(string path) {
        _lines.Clear();
        _cursorLine = 0;
        _cursorColumn = 0;

        try {
            string[] lines = File.ReadAllLines(path);
            _lines.AddRange(lines.Select(line => new StringBuilder(line)));
            _currentFilePath = path;
            return true;
        } catch (Exception e) {
            _terminalBuffer.Print("[boop] failed to open file: " + e.Message);
            return false;
        }
    }

    private void HandleInput(Key key) {
        Console.WriteLine("Input: " + key);

        switch (key) {
            case Key.Left:
                if (_cursorColumn > 0) {
                    _cursorColumn--;
                }
                break;
            case Key.Right:
                if (_cursorColumn < _lines[_cursorLine].Length) {
                    _cursorColumn++;
                }
                break;
            case Key.Up:
                if (_cursorLine > 0) {
                    _cursorLine--;
                    _cursorColumn = Math.Min(_cursorColumn, _lines[_cursorLine].Length);
                }
                break;
            case Key.Down:
                if (_cursorLine < _lines.Count - 1) {
                    _cursorLine++;
                    _cursorColumn = Math.Min(_cursorColumn, _lines[_cursorLine].Length);
                }
                break;
            case Key.F5:
                _terminalVisible = !_terminalVisible;
                return;
            case Key.ControlLeft:
            case Key.ControlRight:
                _ctrlDown = true;
                return;
        }

        if (_ctrlDown && key == Key.S) {
            SaveFile();
        }
    }

    private void SaveFile() {
        if (_currentFilePath == null) {
            _terminalBuffer.Print("[boop] no file path");
            return;
        }

        File.WriteAllLines(
            _currentFilePath,
            _lines.Select(l => l.ToString())
        );

        _terminalBuffer.Print($"[boop] saved {_currentFilePath}");
    }

    private void HandleChar(char c) {
        if (_terminalVisible) {
            HandleTerminal(c);
            return;
        }

        if (_ctrlDown) {
            return;
        }

        switch (c) {
            case '\b' when _cursorColumn > 0:
                _lines[_cursorLine].Remove(_cursorColumn - 1, 1);
                _cursorColumn--;
                break;
            case '\b': {
                if (_cursorLine > 0) {
                    int prevLength = _lines[_cursorLine - 1].Length;
                    _lines[_cursorLine - 1].Append(_lines[_cursorLine]);
                    _lines.RemoveAt(_cursorLine);
                    _cursorLine--;
                    _cursorColumn = prevLength;
                }

                break;
            }
            case '\n': {
                var newLine = new StringBuilder();
                StringBuilder current = _lines[_cursorLine];

                if (_cursorColumn < current.Length) {
                    newLine.Append(current.ToString(_cursorColumn, current.Length - _cursorColumn));
                    current.Remove(_cursorColumn, current.Length - _cursorColumn);
                }

                _lines.Insert(_cursorLine + 1, newLine);
                _cursorLine++;
                _cursorColumn = 0;

                break;
            }
            default:
                _lines[_cursorLine].Insert(_cursorColumn, c);
                _cursorColumn++;
                break;
        }
    }

    private void HandleTerminal(char c) {
        switch (c) {
            case '\b':
                if (_terminalInput.Length > 0) {
                    _terminalInput = _terminalInput[..^1];
                }

                break;

            case '\n':
                _terminalBuffer.Print("> " + _terminalInput);
                _terminalSession.Send(_terminalInput);
                _terminalInput = "";
                break;

            default:
                _terminalInput += c;
                break;
        }
    }
}
