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
            Context.LastStackFrame.ScreenPos = new Vec2 { X = 0, Y = 0 };
            Context.LastStackFrame.Size = new Vec2 { X = Console.WindowWidth, Y = Console.WindowHeight };
            Context.LastStackFrame.HasBorder = BorderDir.None;
        }
        else
        {
            Context.LastStackFrame.HasBorder = BorderDir.Double;
            //TODO
        }

        if (flags.HasFlag(WindowFlags.HasMenu))
        {
        }
        Context.PushId(title);
        Context.CurrentWindow = new Window() { Id = Context.CurrentId };
        Context.Windows.Add(Context.CurrentWindow);

        var sf = Context.CascadedStackFrame;

        Context.LastStackFrame.ScreenPos = sf.ScreenPos + new Vec2 { X = flags.HasFlag(WindowFlags.TopWindow) ? 0 : 1, Y = (flags.HasFlag(WindowFlags.TopWindow) ? 0 : 1) + (flags.HasFlag(WindowFlags.HasMenu) ? 1 : 0)};
        Context.LastStackFrame.Size = sf.Size + new Vec2 { X = flags.HasFlag(WindowFlags.TopWindow)  ? 0 : - 2, Y = flags.HasFlag(WindowFlags.TopWindow)  ? 0 : - 2 };
        Context.LastStackFrame.Cursor = Vec2.Zero;

        if (!flags.HasFlag(WindowFlags.TopWindow))
        {
            Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos!, sf.Size!, DrawBorderCommand.BorderType.Double, Context.Style.WindowBackground, Context.Style.WindowForeground, Context.Style.WindowBackground, null));
            if (!string.IsNullOrEmpty(title))
                Context.CurrentWindow.DrawCommands.Add(new DrawTextCommand($" {title} ", sf.ScreenPos! + new Vec2 { X = (sf.Size!.X - title.Length) / 2, Y = -1 }, new ElementProperties().SetBg(Context.Style.WindowBackground).SetFg(Context.Style.WindowForeground)));
        }
        else
        {
            Context.AddDrawCommand(new DrawFillCommand(sf.ScreenPos!, sf.Size!, Context.Style.WindowBackground));
            if (flags.HasFlag(WindowFlags.HasMenu))
                Context.AddDrawCommand(new DrawFillCommand(sf.ScreenPos!, new Vec2 { X = sf.Size.X, Y = 1}, Context.Style.MenuBackground));
        }
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
