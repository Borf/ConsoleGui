using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.DrawCommands;
public class DrawBorderCommand(Vec2 pos, Vec2 size, DrawBorderCommand.BorderType type, bool noOverrideSelf = false) : DrawCommand
{
    private Vec2 pos = pos;
    private Vec2 size = size;
    private BorderType type = type;
    private readonly bool noOverrideSelf = noOverrideSelf;

    public override void Draw(FrameBuffer buffer)
    {
        buffer.NoOverrideSelf = noOverrideSelf;
        buffer.Cursor = pos;

        var standardElement = new ElementProperties().SetFg(ForegroundColor).SetBg(BackgroundColor);

        if (type == BorderType.Double)
        {
            buffer.Write(Id, $"╔" + new string('═', size.X - 2) + "╗", standardElement);
            for (int i = 0; i < size.Y - 2; i++)
            {
                buffer.Cursor = pos + new Vec2 { X = 0, Y = i + 1 };
                buffer.Write(Id, "║" + new string(' ', size.X - 2) + "║", standardElement);
            }
            buffer.Cursor = pos + new Vec2 { X = 0, Y = size.Y - 1 };
            buffer.Write(Id, "╚" + new string('═', size.X - 2) + "╝", standardElement);
        }
     
        if (type == BorderType.Slim)
        {
            buffer.Cursor = pos + new Vec2 { X = 1, Y = 0 };
            buffer.Write(Id, new string('▄', size.X), standardElement);
            for (int i = 0; i < size.Y - 1; i++)
            {
                buffer.Cursor = pos + new Vec2 { X = 1, Y = i + 1 };
                buffer.Write(Id, "█" + new string(' ', size.X - 2) + "█", standardElement);
            }
            buffer.Cursor = pos + new Vec2 { X = 1, Y = size.Y - 1 };
            buffer.Write(Id, new string('▀', size.X), standardElement);
        }
        if (type == BorderType.Wide)
        {
            buffer.Cursor = pos + new Vec2 { X = 0, Y = 0 };
            buffer.Write(Id, "█" + new string('▀', size.X-2) + "█", standardElement);
            for (int i = 0; i < size.Y - 1; i++)
            {
                buffer.Cursor = pos + new Vec2 { X = 0, Y = i + 1 };
                buffer.Write(Id, "█" + new string(' ', size.X - 2) + "█", standardElement);
            }
            buffer.Cursor = pos + new Vec2 { X = 0, Y = size.Y - 1 };
            buffer.Write(Id, "█" + new string('▄', size.X-2) + "█", standardElement);
        }    
        if (type == BorderType.Round)
        {
            buffer.Cursor = pos + new Vec2 { X = 0, Y = 0 };
            buffer.Write(Id, "╭" + new string('─', size.X-2) + "╮", standardElement);
            for (int i = 0; i < size.Y - 1; i++)
            {
                buffer.Cursor = pos + new Vec2 { X = 0, Y = i + 1 };
                buffer.Write(Id, "│" + new string(' ', size.X - 2) + "│", standardElement);
            }
            buffer.Cursor = pos + new Vec2 { X = 0, Y = size.Y - 1 };
            buffer.Write(Id, "╰" + new string('─', size.X-2) + "╯", standardElement);
        }
        if (type == BorderType.Single)
        {
            buffer.Cursor = pos + new Vec2 { X = 0, Y = 0 };
            buffer.Write(Id, "┌" + new string('─', size.X - 2) + "┐", standardElement);
            for (int i = 0; i < size.Y - 1; i++)
            {
                buffer.Cursor = pos + new Vec2 { X = 0, Y = i + 1 };
                buffer.Write(Id, "│" + new string(' ', size.X - 2) + "│", standardElement);
            }
            buffer.Cursor = pos + new Vec2 { X = 0, Y = size.Y - 1 };
            buffer.Write(Id, "└" + new string('─', size.X - 2) + "┘", standardElement);
        }
        buffer.NoOverrideSelf = false;

    }

    public enum BorderType
    {
        Single,
        Double,
        Slim,
        Wide,
        Round,
    }
}
