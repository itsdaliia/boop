using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace boop.Platform;

public class BoopWindow {
    private readonly IWindow _window;
    private Action<double>? _onUpdate;
    private Action<double>? _onRender;
    private Action? _onLoad;
    private Action? _onClose;

    public int Width => _window.Size.X;
    public int Height => _window.Size.Y;
    public string Title {
        get => _window.Title;
        set => _window.Title = value;
    }

    public BoopWindow(int width = 1280, int height = 720, string title = "boop!") {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(width, height);
        options.Title = title;
        options.VSync = true;
        
        _window = Window.Create(options);
        
        _window.Load += () => _onLoad?.Invoke();
        _window.Update += delta => _onUpdate?.Invoke(delta);
        _window.Render += delta => _onRender?.Invoke(delta);
        _window.Closing += () => _onClose?.Invoke();
    }
    
    public void OnLoad(Action callback) => _onLoad = callback;
    public void OnUpdate(Action<double> callback) => _onUpdate = callback;
    public void OnRender(Action<double> callback) => _onRender = callback;
    public void OnClose(Action callback) => _onClose = callback;
    
    public void Run() => _window.Run();
    public void Close() => _window.Close();
    
    public void MakeCurrent() => _window.MakeCurrent();
    public void SwapBuffers() => _window.SwapBuffers();
}