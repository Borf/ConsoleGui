using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.DrawCommands;
public class DrawTextCommand : DrawCommand
{
    private string text;
    private Vec2 pos;
    private ElementProperties properties;

    public DrawTextCommand(string text, Vec2 pos, ElementProperties properties)
    {
        this.text = text;
        this.pos = pos;
        this.properties = properties;
    }

    public override void Draw(FrameBuffer buffer)
    {
        buffer.Cursor = pos;
        buffer.Write(Id, text, properties);
    }
}
