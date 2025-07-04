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
    public static void Text(string text, bool underline = false, string hover = "")
    {
        if (!Context.NextFrameProperties.SameLine)
            Context.SetLastCursor(new Vec2 { X = 0, Y = (Context.CascadedStackFrame.Cursor?.Y ?? 0) + Context.CascadedStackFrame.LastHeight }, 0);

        SetNextTextColorDefault(Style.WindowForeground);
        SetNextBackgroundColorDefault(Style.WindowBackground);
        Context.PushId(text.Length > 10 ? text[0..10] : text);
        var sf = Context.CascadedStackFrame;

        var activationState = GetComponentActivationState();

        int width = Context.LastStackFrame.Size?.X ?? text.Length;
        Context.AddDrawCommand(new DrawTextCommand(text, sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(sf.BackgroundColor.Value).SetFg(sf.TextColor.Value).SetUnderLine(underline)));

        if(hover != text.TrimEnd() && activationState == ComponentActivationState.Hovered)
            Context.AddDrawCommand(new DrawTextCommand(hover, sf.ScreenPos + sf.Cursor + new Vec2 { X = 0, Y = -1}, new ElementProperties().SetBg(sf.TextColor.Value).SetFg(sf.BackgroundColor.Value)));

        Context.PopId();


        Context.SetLastCursor(new Vec2 { X = sf.Cursor.X + width, Y = sf.Cursor.Y }, 1);
    }


}
