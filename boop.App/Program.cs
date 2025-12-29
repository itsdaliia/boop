using boop.Core;
using boop.Platform;
using boop.Rendering;

namespace boop.App;

public static class Program {
    public static void Main(string[] args) {
        var window = new BoopWindow(1280, 720, "boop!");
        var renderer = new SkiaRenderer();
        var editor = new Editor();

        window.OnLoad(() => {
            window.MakeCurrent();
            renderer.Init(window.Width, window.Height);
        });

        window.OnUpdate(delta => {
            editor.Update(delta);
        });

        window.OnRender(delta => {
            renderer.BeginFrame();
            editor.Render(renderer);
            renderer.EndFrame();
        });

        window.OnClose(() => {
            renderer.Dispose();
        });

        window.Run();
    }
}