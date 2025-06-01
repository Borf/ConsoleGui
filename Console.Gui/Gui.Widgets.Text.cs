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
        Context.PushId(text);
        var sf = Context.CascadedStackFrame;

        Context.AddDrawCommand(new DrawTextCommand(text, sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(Context.Style.WindowBackground).SetFg(Context.Style.WindowForeground)));
        Context.PopId();
        Context.LastStackFrame.Cursor = sf.Cursor + new Vec2 { X = 0, Y = 1 };
    }


}
