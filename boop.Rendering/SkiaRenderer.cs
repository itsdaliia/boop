using SkiaSharp;

namespace boop.Rendering;

public class SkiaRenderer : IRenderer, IDisposable {
    private GRContext? _grContext;
    private GRBackendRenderTarget? _renderTarget;
    private SKSurface? _surface;
    private int _width;
    private int _height;
    
    public SKCanvas? Canvas => _surface?.Canvas;

    public void Init(int width, int height) {
        _width = width;
        _height = height;

        _grContext = GRContext.CreateGl();

        if (_grContext == null) {
            throw new InvalidOperationException("Failed to create GRContext");
        }

        CreateSurface();
    }

    public void Resize(int width, int height) {
        if (_width == width && _height == height) return;
        
        _width = width;
        _height = height;
        
        _surface?.Dispose();
        _renderTarget?.Dispose();

        CreateSurface();
    }

    public void BeginFrame() {
        if (Canvas == null) return;
        Canvas.Save();
    }

    public void EndFrame() {
        if (Canvas == null) return;
        
        Canvas.Restore();
        Canvas.Flush();
        _grContext?.Flush();
    }

    public void Clear(Color color) {
        Canvas?.Clear(new SKColor(color.R, color.G, color.B, color.A));
    }

    public void DrawText(string text, float x, float y, float size, Color color) {
        if (Canvas == null) return;
        
        using var font = new SKFont(SKTypeface.FromFamilyName("Arial"), size);
        using var paint = new SKPaint {
            Color = new SKColor(color.R, color.G, color.B, color.A),
            IsAntialias = true
        };

        Canvas.DrawText(text, x, y, font, paint);
    }

    public void DrawRect(float x, float y, float width, float height, Color color, bool filled = true, int strokeWidth = 2) {
        if (Canvas == null) return;
        
        using var paint = new SKPaint {
            Color = new SKColor(color.R, color.G, color.B, color.A),
            Style = filled ? SKPaintStyle.Fill : SKPaintStyle.Stroke,
            StrokeWidth = strokeWidth,
            IsAntialias = true
        };
        
        Canvas.DrawRect(x, y, width, height, paint);
    }

    public void DrawRoundRect(float x, float y, float width, float height, float radius, Color color, bool filled = true, int strokeWidth = 2) {
        if (Canvas == null) return;
        
        using var paint = new SKPaint {
            Color = new SKColor(color.R, color.G, color.B, color.A),
            Style = filled ? SKPaintStyle.Fill : SKPaintStyle.Stroke,
            StrokeWidth = strokeWidth,
            IsAntialias = true
        };
        
        Canvas.DrawRoundRect(x, y, width, height, radius, radius, paint);
    }

    private void CreateSurface() {
        if (_grContext == null) return;

        _renderTarget = new GRBackendRenderTarget(
            _width,
            _height,
            sampleCount: 0,
            stencilBits: 8,
            new GRGlFramebufferInfo(
                fboId: 0,
                format: SKColorType.Rgba8888.ToGlSizedFormat()
            )
        );

        _surface = SKSurface.Create(
            _grContext,
            _renderTarget,
            GRSurfaceOrigin.BottomLeft,
            SKColorType.Rgba8888
        );

        if (_surface == null) {
            throw new InvalidOperationException("Failed to create SKSurface");
        }
    }

    public void Dispose() {
        _surface?.Dispose();
        _renderTarget?.Dispose();
        _grContext?.Dispose();
    }
}