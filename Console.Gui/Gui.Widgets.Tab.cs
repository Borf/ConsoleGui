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
    public static void BeginTabPanel(string tabPanelId)
    {
        var sp = Context.CascadedStackFrame;
        Context.PushId(tabPanelId);
        Context.LastStackFrame.FrameType = "tabPane";

        Context.LastStackFrame.ScreenPos = sp.ScreenPos + new Vec2 { X = 0, Y = 0 };
        Context.LastStackFrame.Size = sp.Size - new Vec2 { X = 2, Y = 2 };
        Context.LastStackFrame.Cursor = new Vec2 { X = 1, Y = 0 }; // Reset cursor for drawing the tab headers to top left of the tab panel

        var tabPane = Context.CascadedStackFrame;
        Context.AddDrawCommand(new DrawTextCommand("╟" + new string('─', tabPane.Size.X) + "╢", tabPane.ScreenPos + new Vec2 { X = -1, Y = 1 }, new ElementProperties().SetBg(Style.WindowBackground).SetFg(Style.WindowForeground)));
    }

    public static void EndTabPanel()
    {
        Context.PopId();
    }

    public static bool BeginTab(string title)
    {
        var tabPane = Context.CascadedStackFrame;
        var state = Context.GetComponentState<TabPanelState>(tabPane.Id);

        //draw the tab header in the context of the panel
        bool selected = state.SelectedTabId == title;
        Context.AddDrawCommand(new DrawTextCommand(title, tabPane.ScreenPos + new Vec2 { X = tabPane.Cursor.X, Y = 0 }, new ElementProperties().SetBg(selected ? Style.TabSelected : Style.WindowBackground).SetFg(Style.WindowForeground)));
        Context.AddDrawCommand(new DrawTextCommand("│", tabPane.ScreenPos + new Vec2 { X = tabPane.Cursor.X + title.Length + 1, Y = 0 }, new ElementProperties().SetFg(Style.WindowForeground)));
        Context.AddDrawCommand(new DrawTextCommand( new string('─', title.Length+2) + "┴", tabPane.ScreenPos + tabPane.Cursor + new Vec2 { X = -1, Y = 1 }, new ElementProperties().SetBg(Style.WindowBackground).SetFg(Style.WindowForeground)));
        Context.LastStackFrame.Cursor += new Vec2 { X = title.Length + 3, Y = 0 };


        if (Context.HoveredComponent == Context.CurrentId && 
            Context.MouseStates[0] == MouseState.JustPressed && 
            Context.MousePos.Y == Context.LastStackFrame.ScreenPos.Y &&
            Context.MousePos.X > tabPane.ScreenPos.X + tabPane.Cursor.X &&
            Context.MousePos.X < tabPane.ScreenPos.X + Context.LastStackFrame.Cursor.X)
        {
            state.SelectedTabId = title;
        }

        Context.PushId(title); // TODO; unique name!
        Context.LastStackFrame.ScreenPos = tabPane.ScreenPos + new Vec2 { X = 1, Y = 2 };
        Context.LastStackFrame.Size = new Vec2 { X = tabPane.Size.X-2, Y = tabPane.Size.Y - 2 };
        Context.LastStackFrame.Cursor = Vec2.Zero;

        
        if (string.IsNullOrEmpty(state.SelectedTabId))
            state.SelectedTabId = title;
        if (title == state.SelectedTabId)
        {
            return true;
        }
        else
        {
            EndTab();
            return false;
        }
    }

    public static void EndTab()
    {
        Context.PopId();
    }


}
public class TabPanelState : ComponentState
{
    public string SelectedTabId { get; set; } = string.Empty;
}