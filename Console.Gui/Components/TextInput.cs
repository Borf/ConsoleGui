using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.Components;
public class TextInput : Component
{
    private string Text;
    private bool border;

    public TextInput(ref string text, Vec2 pos, bool border)
    {
        Text = text;
        Pos = pos;
        this.border = border;

        if (this.border)
        {
            Size = new Vec2 { X = text.Length, Y = 3 };
        }
        else
            Size = new Vec2 { X = text.Length, Y = 1 };
    }

    public override void Render(Context context, Vec2 parentPos)
    {
        if (border)
        {
            context.DrawCommands.Add(new DrawBorderCommand(Pos + parentPos, Size, Id, DrawBorderCommand.BorderType.Round, context.Style.WindowBackground, context.Style.InputBorder, context.Style.InputCenter, null ));
            if (!string.IsNullOrEmpty(Text))
                context.DrawCommands.Add(new DrawTextCommand(Id, Text, Pos + parentPos + new Vec2 { X = 2, Y = 1 }, new ElementProperties().SetBg(context.Style.InputCenter).SetFg(context.Style.InputText)));
        }
        else
        {
            context.DrawCommands.Add(new DrawTextCommand(Id, $"▌ {Text} ▐", Pos + parentPos, new ElementProperties().SetBg(context.Style.InputCenter).SetFg(context.Style.InputText).SetUnderLine().SetOverLine()));
        }

    }
}
