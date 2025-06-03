using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;

public static partial class Gui
{
    public static bool Button(string text, bool big)
    {

        Context.PushId(text);

        var state = GetComponentActivationState();
        var sf = Context.CascadedStackFrame;

        var color = Context.Style.ButtonCenter;
        if (state == ComponentActivationState.Hovered)
            color = Context.Style.ButtonCenter.Darker();
        if (state == ComponentActivationState.Pressed)
            color = Context.Style.ButtonCenter.Lighter();

        Context.LastStackFrame.BackgroundColor ??= color;
        Context.LastStackFrame.TextColor ??= Context.Style.ButtonText;
        Context.LastStackFrame.ForegroundColor ??= Context.Style.ButtonText;

        if (Context.LastStackFrame.Size == null)
            if (big)
                Context.LastStackFrame.Size = new Vec2 { X = text.Length + 4, Y = 3 };



        if (big)
        {
            Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos + sf.Cursor, Context.LastStackFrame.Size, DrawBorderCommand.BorderType.Round));
            Context.AddDrawCommand(new DrawTextCommand(text, sf.ScreenPos + sf.Cursor + new Vec2 { X = (Context.LastStackFrame.Size.X - text.Length)/2, Y = 1 }, new ElementProperties().SetBg(color).SetFg(Context.Style.ButtonText)));
        }
        else
        {
            Context.AddDrawCommand(new DrawTextCommand($"▏{text}▕", sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(color).SetFg(Context.Style.ButtonText).SetUnderLine().SetOverLine()));
        }

        Context.PopId();
        Context.LastStackFrame.Cursor += new Vec2 { X = 0, Y = big ? 3 : 1 };
        return state == ComponentActivationState.Released;
    }


}
