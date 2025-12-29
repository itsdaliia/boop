namespace boop.Rendering;

public readonly struct Color {
    public readonly byte R;
    public readonly byte G;
    public readonly byte B;
    public readonly byte A;

    public Color(byte r, byte g, byte b, byte a = 255) {
        R = r;
        G = g;
        B = b;
        A = a;
    }
    
    public static Color FromRgb(byte r, byte g, byte b) => new(r, g, b);
    public static Color FromRgba(byte r, byte g, byte b, byte a) => new(r, g, b, a);
    
    public readonly static Color Black = new(0, 0, 0);
    public readonly static Color White = new(255, 255, 255);
    public readonly static Color Red = new(255, 0, 0);
    public readonly static Color Green = new(0, 255, 0);
    public readonly static Color Blue = new(0, 0, 255);
}