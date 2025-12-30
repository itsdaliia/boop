namespace boop.Core.Graphics;

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

    public static readonly Color Black = new(0, 0, 0);
    public static readonly Color White = new(255, 255, 255);
}
