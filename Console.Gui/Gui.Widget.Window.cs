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

    public static void Begin(string title, WindowFlags flags = 0)
    {
        if (Context.CurrentWindow != null)
            throw new Exception("Can't begin when there's a window open already");

        if (flags.HasFlag(WindowFlags.TopWindow))
        {
            Context.LastStackFrame.ScreenPos = Vec2.Zero;
            Context.LastStackFrame.Size = new Vec2 { X = Console.WindowWidth, Y = Console.WindowHeight };
        }
        else
        {
            //TODO
        }
        Context.PushId(title);
        Context.CurrentWindow = new Window() { Id = Context.CurrentId };
        Context.Windows.Add(Context.CurrentWindow);

        Context.LastStackFrame.Cursor = Vec2.Zero;

        var sf = Context.CascadedStackFrame;
        //TODO: assert sf.ScreenPos and sf.Size are set (only set for main window)
        Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos!, sf.Size!, DrawBorderCommand.BorderType.Double, Context.Style.WindowBackground, Context.Style.WindowForeground, Context.Style.WindowBackground, null));
        if (!string.IsNullOrEmpty(title))
            Context.CurrentWindow.DrawCommands.Add(new DrawTextCommand($" {title} ", sf.ScreenPos! + new Vec2 { X = (sf.Size!.X - title.Length) / 2, Y = 0 }, new ElementProperties().SetBg(Context.Style.WindowBackground).SetFg(Context.Style.WindowForeground)));
    }
    public static void End()
    {
        if (Context.CurrentWindow == null)
            throw new Exception("Can't end when there's no window open");
        //TODO: assert stack is 1 and only the window
        Context.CurrentWindow = null;
        Context.PopId();
    }

}
