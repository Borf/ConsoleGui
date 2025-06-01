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
    public static void BeginMenuBar()
    {
        var window = Context.CascadedStackFrame;
        Context.PushId("MenuBar");
        Context.LastStackFrame.ScreenPos = window.ScreenPos + new Vec2 { X = -1, Y = -1 };
        Context.LastStackFrame.Cursor = new Vec2 { X = 1, Y = 0 }; 
    }
    public static void EndMenuBar()
    {
        Context.PopId();
    }
    public static bool BeginMenu(string value)
    {
        var sf = Context.CascadedStackFrame;

        Context.AddDrawCommand(new DrawTextCommand(value, sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(Context.Style.WindowBackground).SetFg(Context.Style.WindowForeground)));

        Context.LastStackFrame.Cursor += new Vec2 { X = value.Length + 2, Y = 0 }; // +1 for the space after the menu item
        return false;
    }

    public static bool MenuItem(string label)
    {
        return false;
    }

    public static void EndMenu()
    {
    }

 
}
