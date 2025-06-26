using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;

public static partial class Gui
{
    public static void Split(string title, bool horizontal, int size)
    {
        var sp = Context.CascadedStackFrame;
        Context.PushId(title);
        Context.LastStackFrame.FrameType = "splitpane";

        Debug.Assert(sp.ScreenPos != null, "ScreenPos should not be null in Split.");
        Debug.Assert(sp.Size != null, "Size should not be null in Split.");

        Context.LastStackFrame.ScreenPos = sp.ScreenPos + new Vec2 { X = 0, Y = 0 };
        Context.LastStackFrame.Size = sp.Size - new Vec2 { X = 0, Y = 0 };
        var state = Context.GetComponentState<SplitPaneState>(Context.CurrentId);
        state.SplitSizes.Clear();

        NextSplit(size);

    }

    public static void NextSplit(int size = -1)
    {
        if(Context.CascadedStackFrame.FrameType != "splitpane")
        {
            Context.PopId();
        }
        var tabPane = Context.CascadedStackFrame;
        var state = Context.GetComponentState<SplitPaneState>(tabPane.Id);

        Debug.Assert(tabPane.Size != null, "Size should not be null in NextSplit.");

        if (size == -1)
            size = tabPane.Size.X - state.SplitSizes.Sum(); // Default size is the remaining space

        Context.PushId("Split#" + state.SplitSizes.Count);
        Context.LastStackFrame.ScreenPos = tabPane.ScreenPos! + new Vec2 { X = 1 + state.SplitSizes.Sum() + state.SplitSizes.Count(), Y = 0 };
        Context.LastStackFrame.Size = new Vec2 { X = size, Y = tabPane.Size.Y - 2 };
        Context.LastStackFrame.Cursor = Vec2.Zero;
        Context.LastStackFrame.HasBorder = tabPane.HasBorder!.Value | BorderDir.RightDouble; //TODO
        state.SplitSizes.Add(size);

        if (Context.LastStackFrame.ScreenPos.X + size < tabPane.ScreenPos!.X + tabPane.Size.X)
        {
            for (int ii = 0; ii < tabPane.Size.Y-1; ii++)
                Context.AddDrawCommand(new DrawTextCommand("║", Context.LastStackFrame.ScreenPos + new Vec2 { X = size, Y = ii }, new ElementProperties().SetFg(Style.WindowForeground)));

            if (tabPane.HasBorder.Value.HasFlag(BorderDir.Up))
                Context.AddDrawCommand(new DrawTextCommand("╦", Context.LastStackFrame.ScreenPos + new Vec2 { X = size, Y = -1 }, new ElementProperties().SetFg(Style.WindowForeground)));

            if (tabPane.HasBorder.Value.HasFlag(BorderDir.Down))
                Context.AddDrawCommand(new DrawTextCommand("╩", Context.LastStackFrame.ScreenPos + new Vec2 { X = size, Y = tabPane.Size.Y }, new ElementProperties().SetFg(Style.WindowForeground)));
        }
    }

    public static void EndSplit()
    {
        Context.PopId();
        Context.PopId();
    }
}


class SplitPaneState : ComponentState
{
    public bool Horizontal { get; set; } = true;
    public List<int> SplitSizes { get; set; } = new();
}
