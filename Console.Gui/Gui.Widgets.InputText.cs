using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConGui;

public static partial class Gui
{
    public static bool InputText(string label, bool big, ref string value)
    {
        Context.PushId(label);

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

        var color = Context.Style.InputCenter;
        if (state == 1)
            color = Context.Style.InputCenter.Darker();
        if (state == 2)
            color = Context.Style.InputCenter.Lighter();


        Context.AddDrawCommand(new DrawTextCommand(label, sf.ScreenPos + sf.Cursor + new Vec2 { X = 0, Y = big?1:0 }, new ElementProperties().SetBg(Context.Style.WindowBackground).SetFg(Context.Style.InputText)));

        if (big)
        {
            //use color
            Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos + sf.Cursor + new Vec2 { X = 30, Y = 0}, new Vec2 { X = 30, Y = 3 }, DrawBorderCommand.BorderType.Round));
            Context.AddDrawCommand(new DrawTextCommand(value, sf.ScreenPos + sf.Cursor + new Vec2 { X = 32, Y = 1 }, new ElementProperties().SetBg(color).SetFg(Context.Style.InputText)));
        }
        else
        {
            //use color
            Context.AddDrawCommand(new DrawTextCommand($"▏{new string(' ', 30-2)}▕", sf.ScreenPos + sf.Cursor + new Vec2 { X = 30, Y = 0 }, new ElementProperties().SetBg(color).SetFg(Context.Style.InputText).SetUnderLine().SetOverLine()));
            Context.AddDrawCommand(new DrawTextCommand($"{value}", sf.ScreenPos + sf.Cursor + new Vec2 { X = 31, Y = 0 }, new ElementProperties().SetBg(color).SetFg(Context.Style.InputText).SetUnderLine().SetOverLine()));
        }

        Context.PopId();
        Context.LastStackFrame.Cursor += new Vec2 { X = 0, Y = big ? 3 : 1 };
        return state == 3;
    }


}
