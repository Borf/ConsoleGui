using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.Components;
public class Button : Component
{
    private string Text;
    private bool border;

    public Button(string text, Vec2 pos, bool border)
    {
        Text = text;
        Pos = pos;
        this.border = border;

        if (this.border)
        {
            Size = new Vec2 { X = text.Length + 5, Y = 3 };
        }
        else
            Size = new Vec2 { X = text.Length + 2, Y = 1 };
    }

    public static Stopwatch sw = new();
    static Button()
    {
        sw.Start();
    }

    public override void Render(Context context, Vec2 parentPos)
    {
        if (border)
        {
            context.DrawCommands.Add(new DrawBorderCommand(Pos + parentPos, Size, Id, DrawBorderCommand.BorderType.Round, context.Style.WindowBackground, context.Style.ButtonBorder, context.Style.ButtonCenter, context.Style.ButtonShadow));
            context.DrawCommands.Add(new DrawTextCommand(Id, Text, Pos + parentPos + new Vec2 { X = 2, Y = 1 }, new ElementProperties().SetBg(context.Style.ButtonCenter).SetFg(context.Style.ButtonText)));
        }
        else
        {
            context.DrawCommands.Add(new DrawTextCommand(Id, $"▏{Text}▕", Pos + parentPos, new ElementProperties().SetBg(context.Style.ButtonCenter).SetFg(context.Style.ButtonText).SetUnderLine().SetOverLine()));
        }

    }
}
