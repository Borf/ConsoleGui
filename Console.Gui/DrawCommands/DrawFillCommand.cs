using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.DrawCommands;
public class DrawFillCommand(Vec2 pos, Vec2 size,Color bgColor) : DrawCommand
{
    private Vec2 pos = pos;
    private Vec2 size = size;
    private readonly Color bgColor = bgColor;

    public override void Draw(FrameBuffer buffer)
    {
        buffer.Cursor = pos;
        for (int i = 0; i < size.Y; i++)
        {
            buffer.Cursor = pos + new Vec2 { X = 0, Y = i };
            buffer.Write(Id, new string(' ', size.X), new ElementProperties().SetBg(bgColor));
        }
    }

    public enum BorderType
    {
        Single,
        Double,
        Shadow,
        Sunk,
        Slim,
        Wide,
        Round,
    }
}
