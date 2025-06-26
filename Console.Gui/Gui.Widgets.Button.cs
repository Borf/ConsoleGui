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
        if (!Context.NextFrameProperties.SameLine)
            Context.SetLastCursor(new Vec2 { X = 0, Y = (Context.CascadedStackFrame.Cursor?.Y ?? 0) + Context.CascadedStackFrame.LastHeight }, 0);


        var state = GetComponentActivationState(text);


        if (state == ComponentActivationState.Hovered)
            SetNextBackgroundColorDefault(Style.ButtonCenter.Darker());
        else if (state == ComponentActivationState.Pressed || state == ComponentActivationState.Down)
            SetNextBackgroundColorDefault(Style.ButtonCenter.Lighter().Lighter());
        else
            SetNextBackgroundColorDefault(Style.ButtonCenter);

        SetNextForegroundColorDefault(Style.ButtonText);
        SetNextTextColorDefault(Style.ButtonText);

        Context.PushId(text);

        var sf = Context.CascadedStackFrame;


        if (Context.LastStackFrame.Size == null)
            Context.LastStackFrame.Size = new Vec2 { X = text.Length + (big ? 4 : 2), Y = big ? 3 : 1 };
        if(Context.LastStackFrame.Size.Y == 0)
            Context.LastStackFrame.Size = new Vec2 { X = Context.LastStackFrame.Size.X, Y = big ? 3 : 1 };
        int size = Context.LastStackFrame.Size.X;

        if (big)
        {
            Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos + sf.Cursor, Context.LastStackFrame.Size, DrawBorderCommand.BorderType.Round));
            Context.AddDrawCommand(new DrawTextCommand(text, sf.ScreenPos + sf.Cursor + new Vec2 { X = (Context.LastStackFrame.Size.X - text.Length)/2, Y = 1 }, new ElementProperties().SetBg(sf.BackgroundColor.Value).SetFg(sf.TextColor.Value)));
        }
        else
        {
            Context.AddDrawCommand(new DrawTextCommand($"▏{text}▕", sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(sf.BackgroundColor.Value).SetFg(Style.ButtonText).SetUnderLine().SetOverLine()));
        }

        Context.PopId();
        Context.SetLastCursor(new Vec2 { X = sf.Cursor.X + size, Y = sf.Cursor.Y }, big ? 3 : 1);
        return state == ComponentActivationState.Released;
    }


}
