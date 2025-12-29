namespace boop.Core.Graphics;

public interface IRenderer {
    void BeginFrame();
    void EndFrame();
    void Clear(Color color);
    void DrawText(string text, float x, float y, float size, Color color);
    void DrawRect(float x, float y, float width, float height, Color color, bool filled = true, int strokeWidth = 2);
    void DrawRoundRect(float x, float y, float width, float height, float radius, Color color, bool filled = true, int strokeWidth = 2);
}