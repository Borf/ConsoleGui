using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.Components;
public class Label : Component
{
    public string Text { get; set; }
    public Label(string text, Vec2 pos)
    {
        Text = text;
        Pos = pos;
    }

    public override void Render(Context context, Vec2 parentPos)
    {
        context.DrawCommands.Add(new DrawTextCommand(Id, Text, parentPos + Pos, new ElementProperties().SetBg(context.Style.WindowBackground).SetFg(context.Style.WindowForeground)));
    }
}
