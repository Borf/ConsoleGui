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
        Context.Windows.Add(Context?.CurrentWindow ?? throw new Exception("Current window not set"));


        if (Context.NextFrameProperties.ScreenPos == null)
        {
            if (flags.HasFlag(WindowFlags.TopWindow))
            {
                Context.NextFrameProperties.ScreenPos = new Vec2 { X = 0, Y = 0 };
                Context.NextFrameProperties.HasBorder = BorderDir.None;
            }
            else
            {
                Context.NextFrameProperties.HasBorder = BorderDir.Double;
            }
        }

        if (Context.NextFrameProperties.Size == null)
        {
            if (flags.HasFlag(WindowFlags.TopWindow))
                Context.NextFrameProperties.Size = new Vec2 { X = Console.WindowWidth, Y = Console.WindowHeight };
            else
                Context.NextFrameProperties.Size = new Vec2 { X = 20, Y = 20 };
        }

        SetNextCursorDefault(new Vec2 { X = 0, Y = 0 });
        SetNextBackgroundColorDefault(Context.Style.WindowBackground);
        SetNextForegroundColorDefault(Context.Style.WindowForeground);
        SetNextTextColorDefault(Context.Style.WindowForeground);
        Context.PushId(title);

        var sf = Context.CascadedStackFrame;

        if(!flags.HasFlag(WindowFlags.TopWindow))
            Context.LastStackFrame.AddMargin(new Vec2 { X = 1, Y = 1 }, new Vec2 { X = 1, Y = 1 });
        if(flags.HasFlag(WindowFlags.HasMenu))
            Context.LastStackFrame.AddMargin(new Vec2 { X = 0, Y = 1 }, new Vec2 { X = 0, Y = 0 });

        if (!flags.HasFlag(WindowFlags.TopWindow))
        {
            if(!flags.HasFlag(WindowFlags.HideBorder))
                Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos!, sf.Size!, DrawBorderCommand.BorderType.Double));
            if (!string.IsNullOrEmpty(title.StripHash()))
                Context.CurrentWindow.DrawCommands.Add(new DrawTextCommand($" {title.StripHash()} ", sf.ScreenPos! + new Vec2 { X = (sf.Size!.X - title.Length) / 2, Y = -1 }, new ElementProperties().SetBg(Context.Style.WindowBackground).SetFg(Context.Style.WindowForeground)));
        }
        else
        {
            Context.AddDrawCommand(new DrawFillCommand(sf.ScreenPos!, sf.Size!, Context.Style.WindowBackground));
            if (flags.HasFlag(WindowFlags.HasMenu))
                Context.AddDrawCommand(new DrawFillCommand(sf.ScreenPos!, new Vec2 { X = sf.Size!.X, Y = 1}, Context.Style.MenuBackground));
        }
    }
    public static void End()
    {
        if (Context.CurrentWindow == null)
            throw new Exception("Can't end when there's no window open");
        //TODO: assert stack is 1 and only the window
        Context.PopId();
        Context.WindowCreationStack.RemoveLast();
    }

}
