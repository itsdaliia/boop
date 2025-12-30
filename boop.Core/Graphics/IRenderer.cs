// Copyright (c) itsdaliia <me@daliia.ch>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence.

namespace boop.Core.Graphics;

public interface IRenderer {
    public float Width { get; }
    public float Height { get; }

    void BeginFrame();
    void EndFrame();
    void Clear(Color color);
    void DrawText(string text, float x, float y, float size, Color color);
    void DrawRect(float x, float y, float width, float height, Color color, bool filled = true, int strokeWidth = 2);
    void DrawRoundRect(float x, float y, float width, float height, float radius, Color color, bool filled = true, int strokeWidth = 2);
    float MeasureTextWidth(string text, float size);
}
