using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;


[Flags]
public enum WindowFlags
{
    None = 0,
    TopWindow = 1 << 0,
    HasMenu = 1 << 1,
    HideBorder = 1 << 2,
}

public static partial class Gui
{

    public static void Begin(string title, WindowFlags flags = 0, Vec2? pos = null, Vec2? size = null, int marginLeft = 0)
    {
        Context.WindowCreationStack.AddLast(new Window() { Id = Context.CurrentId });
        Context.Windows.Add(Context.CurrentWindow);
        Context.PushId("Root" + title);

        if (flags.HasFlag(WindowFlags.TopWindow))
        {
            Context.LastStackFrame.ScreenPos = new Vec2 { X = 0, Y = 0 };
            Context.LastStackFrame.Size = new Vec2 { X = Console.WindowWidth, Y = Console.WindowHeight };
            Context.LastStackFrame.HasBorder = BorderDir.None;
        }
        else
        {
            Context.LastStackFrame.HasBorder = BorderDir.Double;
            if(pos != null) //TODO: not happy with passing these as parameters...what if we get more construction parameters?
                Context.LastStackFrame.ScreenPos = pos;
            if(size != null)
                Context.LastStackFrame.Size = size;
            //TODO
        }

        if (flags.HasFlag(WindowFlags.HasMenu))
        {
        }
        Context.PushId(title);

        var sf = Context.CascadedStackFrame;

        Context.LastStackFrame.ScreenPos = sf.ScreenPos + new Vec2 { X = (flags.HasFlag(WindowFlags.TopWindow) ? 0 : 1) + marginLeft, Y = (flags.HasFlag(WindowFlags.TopWindow) ? 0 : 1) + (flags.HasFlag(WindowFlags.HasMenu) ? 1 : 0)};
        Context.LastStackFrame.Size = sf.Size + new Vec2 { X = (flags.HasFlag(WindowFlags.TopWindow)  ? 0 : - 2) - 2 * marginLeft, Y = flags.HasFlag(WindowFlags.TopWindow)  ? 0 : - 2 };
        Context.LastStackFrame.Cursor = Vec2.Zero;

        if (!flags.HasFlag(WindowFlags.TopWindow))
        {
            if(!flags.HasFlag(WindowFlags.HideBorder))
                Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos!, sf.Size!, DrawBorderCommand.BorderType.Double, Context.Style.WindowBackground, Context.Style.WindowForeground, Context.Style.WindowBackground, null));
            if (!string.IsNullOrEmpty(title.StripHash()))
                Context.CurrentWindow.DrawCommands.Add(new DrawTextCommand($" {title.StripHash()} ", sf.ScreenPos! + new Vec2 { X = (sf.Size!.X - title.Length) / 2, Y = -1 }, new ElementProperties().SetBg(Context.Style.WindowBackground).SetFg(Context.Style.WindowForeground)));
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
        Context.PopId();
        Context.PopId();//root
        Context.WindowCreationStack.RemoveLast();
    }

}
