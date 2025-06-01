using System.Drawing;

namespace ConGui.Util;


public static class ColorHelper
{
    public static Color Darker(this Color color) => Color.FromArgb(color.A, (byte)(color.R * 0.8f), (byte)(color.G * 0.8f), (byte)(color.B * 0.8f));
    public static Color Lighter(this Color color) => Color.FromArgb(color.A, (byte)Math.Min(255, color.R * 1.2f), (byte)Math.Min(255, color.G * 1.2f), (byte)Math.Min(255, color.B * 1.2f));
}
