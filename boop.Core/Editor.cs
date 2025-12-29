using boop.Rendering;

namespace boop.Core;

public class Editor {
    private int _frameCount = 0;

    public Editor() {
        // todo: init editor state
    }

    public void Update(double deltaTime) {
        // todo: update logic
    }

    public void Render(IRenderer renderer) {
        _frameCount++;
        
        // background
        renderer.Clear(new Color(30, 30, 40));
        
        // title
        renderer.DrawText("boop!", 100, 100, 48, Color.White);
        
        // subtitle
        renderer.DrawText("a text editor that wants to be better", 100, 150, 16, new Color(150, 150, 150));
        
        // some test rectangles
        renderer.DrawRoundRect(100, 200, 400, 100, 8, new Color(50, 50, 70), filled: true);
        renderer.DrawText("This is where text will go...", 120, 250, 20, new Color(200, 200, 200));
    }

    public void HandleInput() {
        // todo: handle keyboard/mouse
    }
}