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
    public static void Text(string text)
    {
        if (!Context.NextFrameProperties.SameLine)
            Context.SetLastCursor(new Vec2 { X = 0, Y = (Context.CascadedStackFrame.Cursor?.Y ?? 0) + Context.CascadedStackFrame.LastHeight }, 0);

        SetNextTextColorDefault(Style.WindowForeground);
        Context.PushId(text);
        var sf = Context.CascadedStackFrame;
        int width = Context.LastStackFrame.Size?.X ?? text.Length + 1;
        Context.AddDrawCommand(new DrawTextCommand(text, sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(Style.WindowBackground).SetFg(sf.TextColor.Value)));
        Context.PopId();

        Context.SetLastCursor(new Vec2 { X = sf.Cursor.X + width, Y = sf.Cursor.Y }, 1);
    }


}
