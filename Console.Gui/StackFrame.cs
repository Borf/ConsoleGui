using ConGui.Util;
using System.Diagnostics;
using System.Drawing;

namespace ConGui;
public class StackFrame
{
    public required string Id { get; set; } = string.Empty;
    public string? FrameType { get; set; } = null;
    public Vec2? Cursor { get; set; } = null;
    public int LastHeight { get; set; } = 0;
    public bool SameLine { get; set; } = false;
    public Vec2? ScreenPos { get; set; } = null;
    public Vec2? Size { get; set; } = null;
    public BorderDir? HasBorder { get; set; } = null;
    public Color? BackgroundColor { get; set; } = null;
    public Color? ForegroundColor { get; set; } = null;
    public Color? TextColor { get; set; } = null;

    public void AddMargin(Vec2 marginLeft, Vec2? marginRight = null)
    {
        if (marginRight == null)
            marginRight = marginLeft;
         Debug.Assert(ScreenPos != null && Size != null);
        ScreenPos += marginLeft;
        Size -= (marginLeft + marginRight);
    }
}


[Flags]
public enum BorderDir
{
    None = 0,
    Left = 1 << 0,
    Right = 1 << 1,
    Up = 1 << 2,
    Down = 1 << 3,

    LeftDouble = 1 << 4,
    RightDouble = 1 << 5,
    UpDouble = 1 << 6,
    DownDouble = 1 << 7,

    Double = LeftDouble | RightDouble | UpDouble | DownDouble,
    Single = Left | Right | Up | Down,

    LeftAny = Left | LeftDouble,
    RightAny = Right | RightDouble,
    UpAny = Up | UpDouble,
    DownAny = Down | DownDouble,

    Any = ~0,

}