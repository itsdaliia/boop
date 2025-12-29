using System.Text;
using boop.Core.Graphics;
using boop.Core.Input;

namespace boop.Core;

public class Editor {
    private readonly StringBuilder _buffer = new();
    
    public Editor(IInputHandler input, ITextInput textInput) {
        input.OnKeyDown += HandleInput;
        textInput.OnCharTyped += HandleText;
    }

    public void Update(double deltaTime) {
        // todo: update logic
    }

    public void Render(IRenderer renderer) {
        // background
        renderer.Clear(new Color(30, 30, 40));
        
        // title
        renderer.DrawText("boop!", 100, 100, 48, Color.White);
        
        // subtitle
        renderer.DrawText("a text editor that wants to be better", 100, 150, 16, new Color(150, 150, 150));
        
        // some test rectangles
        renderer.DrawRoundRect(100, 200, 400, 100, 8, new Color(50, 50, 70), filled: true);
        renderer.DrawText(_buffer.Length == 0 ? "This is where text will go..." : _buffer.ToString(), 120, 250, 20, new Color(200, 200, 200));
    }

    private void HandleInput(Key key) {
        Console.WriteLine("Input: " + key);
        if (key == Key.Backspace) {
            if (_buffer.Length > 0)
                _buffer.Length--;
        }
    }
    
    private void HandleText(char chr) {
        Console.WriteLine("Char: " + chr);
        _buffer.Append(chr);
    }
}