// Copyright (c) itsdaliia <me@daliia.ch>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence.

using boop.Core.Input;
using Silk.NET.Input;
using Silk.NET.Windowing;
using Key = boop.Core.Input.Key;

namespace boop.Platform;

public class InputManager : IInputHandler, ITextInput {
    private readonly HashSet<Key> _keysHeld = [];
    private readonly Dictionary<Key, double> _keyTimers = new();
    private const float _repeatDelay = 0.5f;
    private const float _repeatRate = 0.05f;

    public event Action<Key>? OnKeyDown;
    public event Action<Key>? OnKeyUp;
    public event Action<char>? OnCharTyped;

    public void Init(IWindow window) {
        IInputContext input = window.CreateInput();
        foreach (IKeyboard k in input.Keyboards) {
            k.KeyDown += (_, key, _) => KeyDown(ConvertKey(key));
            k.KeyUp += (_, key, _) => KeyUp(ConvertKey(key));

            k.KeyChar += (_, chr) => OnCharTyped?.Invoke(chr);
        }
    }

    public void Update(double deltaTime) {
        foreach (Key key in _keysHeld.ToArray()) {
            _keyTimers[key] += deltaTime;

            if (!(_keyTimers[key] >= _repeatDelay)) {
                continue;
            }

            ProcessKey(key);
            _keyTimers[key] -= _repeatRate;
        }
    }

    private void KeyDown(Key key) {
        if (!_keysHeld.Add(key)) {
            return;
        }

        _keyTimers[key] = 0f;
        ProcessKey(key);
    }

    private void KeyUp(Key key) {
        _keysHeld.Remove(key);
        _keyTimers.Remove(key);
    }

    private void ProcessKey(Key key) {
        OnKeyDown?.Invoke(key);

        char? c = key switch {
            Key.Backspace => '\b',
            Key.Enter => '\n',
            Key.Tab => '\t',
            _ => null
        };

        if (c.HasValue) {
            OnCharTyped?.Invoke(c.Value);
        }
    }

    private static Key ConvertKey(Silk.NET.Input.Key key) {
        return Enum.TryParse(key.ToString(), out Key converted) ? converted : Key.Unknown;
    }
}
