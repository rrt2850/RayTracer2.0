using System.Drawing;

public readonly record struct Color(double R, double G, double B)
{
    public static Color operator +(Color x, Color y)
    {
        return new Color (x.R + y.R, x.G + y.G, x.B + y.B);
    }

    public static Color operator *(Color x, double scalar)
    {
        return new Color (x.R * scalar, x.G * scalar, x.B * scalar);
    }

    public System.Drawing.Color convert()
    {   
        return System.Drawing.Color.FromArgb(
            255,
            toByte(R),
            toByte(G),
            toByte(B)
        );
    }

    private byte toByte(double value)
    {
        value = Math.Clamp(value, 0.0, 1.0);
        return (byte) Math.Round(value * 255);
    }
}