// Copyright (c) itsdaliia <me@daliia.ch>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence.

using System.Text;
using boop.Core.Graphics;
using boop.Core.Input;

namespace boop.Core;

public class Editor {
    private readonly List<StringBuilder> _lines = [new()];
    
    private int _cursorLine;
    private int _cursorColumn;
    
    public Editor(IInputHandler input, ITextInput textInput) {
        input.OnKeyDown += HandleInput;
        textInput.OnCharTyped += HandleChar;
    }

    public void Update(double deltaTime) {
        // todo: update logic
    }

    public void Render(IRenderer renderer) {
        // background
        renderer.Clear(new Color(30, 30, 40));
        
        const float x = 100;
        const float lineHeight = 24; 
        float y = 100;

        for (var i = 0; i < _lines.Count; i++) {
            renderer.DrawText($"{i + 1} {_lines[i]}", x, y, 20, Color.White);
            y += lineHeight;
        }
    }

    private static void HandleInput(Key key) {
        Console.WriteLine("Input: " + key);
    }
    
    private void HandleChar(char c) {
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
                var current = _lines[_cursorLine];
            
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
}