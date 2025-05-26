using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.DrawCommands;
public class DrawBorderCommand(Vec2 pos, Vec2 size, string? id, DrawBorderCommand.BorderType type, Color bgColor, Color borderColor, Color insideColor, Color? shadowColor) : DrawCommand
{
    private string? id = id;
    private Vec2 pos = pos;
    private Vec2 size = size;
    private BorderType type = type;
    private readonly Color bgColor = bgColor;
    private readonly Color borderColor = borderColor;
    private readonly Color insideColor = insideColor;
    private readonly Color? shadowColor = shadowColor;

    public void Draw(FrameBuffer buffer)
    {
        buffer.Cursor = pos;
        if (type == BorderType.Double)
        {
            buffer.Write(id, $"╔" + new string('═', size.X - 2) + "╗", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
            for (int i = 0; i < size.Y - 2; i++)
            {
                buffer.Cursor = pos + new Vec2 { X = 0, Y = i + 1 };
                buffer.Write(id, "║" + new string(' ', size.X - 2) + "║", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
            }
            buffer.Cursor = pos + new Vec2 { X = 0, Y = size.Y - 1 };
            buffer.Write(id, "╚" + new string('═', size.X - 2) + "╝", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
        }

        if (type == BorderType.Shadow)
        {
            buffer.Write(id, "█" + new string('▀', size.X-3)+ "█", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
            buffer.Write(id, "▄", new ElementProperties().SetFg(shadowColor.Value).SetBg(bgColor));
            for (int i = 0; i < size.Y - 3; i++)
            {
                buffer.Cursor = pos + new Vec2 { X = 0, Y = i + 1 };
                buffer.Write(id, "█" + new string(' ', size.X - 3), new ElementProperties().SetFg(borderColor).SetBg(insideColor));
                buffer.Write(id, "█", new ElementProperties().SetFg(borderColor).SetBg(shadowColor.Value));
                buffer.Write(id, "█", new ElementProperties().SetFg(shadowColor.Value).SetBg(shadowColor.Value));
            }
            buffer.Cursor = pos + new Vec2 { X = 0, Y = size.Y - 2 };
            buffer.Write(id, "█" + new string('▄', size.X - 3) + "█", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
            buffer.Write(id, "█", new ElementProperties().SetFg(shadowColor.Value).SetBg(shadowColor.Value));
            buffer.Cursor = pos + new Vec2 { X = 0, Y = size.Y - 1 };
            buffer.Write(id, " " + new string('▀', size.X - 1), new ElementProperties().SetFg(shadowColor.Value).SetBg(bgColor));
        }

        if (type == BorderType.Sunk)
        {
            buffer.Cursor = pos + new Vec2 { X = 1, Y = 0 };
            buffer.Write(id, new string('▄', size.X-1), new ElementProperties().SetFg(borderColor).SetBg(bgColor));
            for (int i = 0; i < size.Y - 2; i++)
            {
                buffer.Cursor = pos + new Vec2 { X = 1, Y = i + 1 };
                buffer.Write(id, "█" + new string(' ', size.X - 3), new ElementProperties().SetFg(borderColor).SetBg(insideColor));
                buffer.Write(id, "█", new ElementProperties().SetFg(borderColor).SetBg(shadowColor.Value));
            }
            buffer.Cursor = pos + new Vec2 { X = 1, Y = size.Y - 1 };
            buffer.Write(id, new string('▀', size.X - 1), new ElementProperties().SetFg(borderColor).SetBg(bgColor));
        }
        if (type == BorderType.Slim)
        {
            buffer.Cursor = pos + new Vec2 { X = 1, Y = 0 };
            buffer.Write(id, new string('▄', size.X), new ElementProperties().SetFg(borderColor).SetBg(bgColor));
            for (int i = 0; i < size.Y - 1; i++)
            {
                buffer.Cursor = pos + new Vec2 { X = 1, Y = i + 1 };
                buffer.Write(id, "█" + new string(' ', size.X - 2) + "█", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
            }
            buffer.Cursor = pos + new Vec2 { X = 1, Y = size.Y - 1 };
            buffer.Write(id, new string('▀', size.X), new ElementProperties().SetFg(borderColor).SetBg(bgColor));
        }
        if (type == BorderType.Wide)
        {
            buffer.Cursor = pos + new Vec2 { X = 0, Y = 0 };
            buffer.Write(id, "█" + new string('▀', size.X-2) + "█", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
            for (int i = 0; i < size.Y - 1; i++)
            {
                buffer.Cursor = pos + new Vec2 { X = 0, Y = i + 1 };
                buffer.Write(id, "█" + new string(' ', size.X - 2) + "█", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
            }
            buffer.Cursor = pos + new Vec2 { X = 0, Y = size.Y - 1 };
            buffer.Write(id, "█" + new string('▄', size.X-2) + "█", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
        }    
        if (type == BorderType.Round)
        {
            buffer.Cursor = pos + new Vec2 { X = 0, Y = 0 };
            buffer.Write(id, "╭" + new string('─', size.X-2) + "╮", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
            for (int i = 0; i < size.Y - 1; i++)
            {
                buffer.Cursor = pos + new Vec2 { X = 0, Y = i + 1 };
                buffer.Write(id, "│" + new string(' ', size.X - 2) + "│", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
            }
            buffer.Cursor = pos + new Vec2 { X = 0, Y = size.Y - 1 };
            buffer.Write(id, "╰" + new string('─', size.X-2) + "╯", new ElementProperties().SetFg(borderColor).SetBg(insideColor));
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
