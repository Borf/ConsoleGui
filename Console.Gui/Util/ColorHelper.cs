using System.Drawing;

namespace ConGui.Util;


public static class ColorHelper
{
    public static Color Darker(this Color color, float fac = 0.8f) => Color.FromArgb(color.A, (byte)(color.R * fac), (byte)(color.G * fac), (byte)(color.B * fac));
    public static Color Lighter(this Color color, float fac = 1.2f) => Color.FromArgb(color.A, (byte)Math.Min(255, color.R * 1.2f), (byte)Math.Min(255, color.G * fac), (byte)Math.Min(255, color.B * fac));
}
