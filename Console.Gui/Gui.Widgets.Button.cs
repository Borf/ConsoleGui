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

        var sf = Context.CascadedStackFrame;

        int state = 0;
        if (Context.HoveredComponent == Context.CurrentId)
        {
            state = 1;
            if (Context.MouseStates[0] == MouseState.Down || Context.MouseStates[1] == MouseState.Pressed)
                state = 2;
            if (Context.MouseStates[0] == MouseState.Released)
                state = 3;
        }

        var color = Context.Style.ButtonCenter;
        if (state == 1)
            color = Context.Style.ButtonCenter.Darker();
        if (state == 2)
            color = Context.Style.ButtonCenter.Lighter();

        if (big)
        {
            Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos + sf.Cursor, new Vec2 { X = text.Length+3, Y = 3}, DrawBorderCommand.BorderType.Round, Context.Style.WindowBackground, Context.Style.ButtonBorder, color, Context.Style.ButtonShadow));
            Context.AddDrawCommand(new DrawTextCommand(text, sf.ScreenPos + sf.Cursor + new Vec2 { X = 2, Y = 1 }, new ElementProperties().SetBg(color).SetFg(Context.Style.ButtonText)));
        }
        else
        {
            Context.AddDrawCommand(new DrawTextCommand($"▏{text}▕", sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(color).SetFg(Context.Style.ButtonText).SetUnderLine().SetOverLine()));
        }

        Context.PopId();
        Context.LastStackFrame.Cursor += new Vec2 { X = 0, Y = big ? 3 : 1 };
        return state == 3;
    }


}
