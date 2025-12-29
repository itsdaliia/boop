using boop.Core.Input;
using Silk.NET.Input;
using Silk.NET.Windowing;
using Key = boop.Core.Input.Key;

namespace boop.Platform;

public class InputManager : IInputHandler, ITextInput {
    public event Action<Key>? OnKeyDown;
    public event Action<char>? OnCharTyped;

    public void Init(IWindow window) {
        var input = window.CreateInput();
        foreach (var k in input.Keyboards) {
            k.KeyDown += (_, key, _) => {
                OnKeyDown?.Invoke(ConvertKey(key));
            };
            
            k.KeyChar += (_, chr) => {
                OnCharTyped?.Invoke(chr);
            };
        }
    }
    
    private Key ConvertKey(Silk.NET.Input.Key key) {
        if (Enum.TryParse<Key>(key.ToString(), out var converted)) {
            return converted;
        }
        
        return Key.Unknown;
    }
}