// Copyright (c) itsdaliia <me@daliia.ch>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence.

using boop.Core;
using boop.Platform;
using boop.Rendering;

namespace boop.App;

public static class Program {
    public static void Main(string[] args) {
        var window = new BoopWindow(1280, 720, "boop!");
        var inputManager = new InputManager();
        var renderer = new SkiaRenderer();
        Editor editor = null!;

        window.OnLoad(() => {
            window.MakeCurrent();
            
            renderer.Init(window.Width, window.Height);
            inputManager.Init(window.GetNativeWindow());
            
            editor = new Editor(inputManager, inputManager);
        });

        window.OnUpdate(delta => {
            editor.Update(delta);
            inputManager.Update(delta);
        });

        window.OnRender(delta => {
            renderer.BeginFrame();
            editor.Render(renderer);
            renderer.EndFrame();
        });

        window.OnResize(size => {
            renderer.Resize(size.X, size.Y);
        });
        
        window.OnClose(() => {
            renderer.Dispose();
        });

        window.Run();
    }
}