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
    public static void BeginList(string name)
    {
        var parentSf = Context.CascadedStackFrame;
        Context.NextFrameProperties.Size = parentSf.Size + new Vec2 { X = 1, Y = 2 };
        Context.NextFrameProperties.ScreenPos = parentSf.ScreenPos + new Vec2 { X = -1, Y = 0 };
        SetNextBackgroundColorDefault(Context.Style.ListBackground);
        SetNextForegroundColorDefault(Context.Style.ListOutline);
        Context.PushId(name);
        var state = Context.GetComponentState<ListState>(Context.CurrentId);
        state.CurrentIndex = 0;
        int scrollHeight = state.MaxIndex;
        state.MaxIndex = 0;
        var sf = Context.CascadedStackFrame;

        if(Context.HoveredComponent.StartsWith(sf.Id))
        {
            state.ScrollOffset = Math.Clamp(state.ScrollOffset + Context.MouseScroll * (Context.LastStackFrame.Size.Y/3), 0, Math.Max(0, scrollHeight - Context.LastStackFrame.Size.Y+2));
        }

        Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos, Context.LastStackFrame.Size, DrawBorderCommand.BorderType.Single));
        Context.LastStackFrame.ScreenPos = parentSf.ScreenPos + new Vec2 { X = 1, Y = 1 };

    }

    public static bool ListEntry(string text, ListChangedEvent whenEvent)
    {
        bool ret = false;
        var listSf = Context.CascadedStackFrame;
        var listState = Context.GetComponentState<ListState>(Context.CurrentId);
        listState.MaxIndex++;
        if (listState.CurrentIndex < listState.ScrollOffset || listState.CurrentIndex-listState.ScrollOffset >= listSf.Size.Y-2)
        {
            listState.CurrentIndex++;
            return false;
        }

        Context.PushId(text);
        var sf = Context.CascadedStackFrame;
        if (Context.HoveredComponent == sf.Id && Context.MouseStates[0] == MouseState.Pressed)
        {
            listState.SelectedIndex = listState.CurrentIndex;
            if(whenEvent == ListChangedEvent.OnSelect)
                ret = true;
        }



        var props = new ElementProperties().SetBg(Context.Style.ListBackground).SetFg(Context.Style.ListOutline);
        if (listState.SelectedIndex == listState.CurrentIndex)
        {
            if (whenEvent == ListChangedEvent.Continuous)
                ret = true;
            props.SetBg(Context.Style.ListSelectionBackground).SetFg(Context.Style.ListSelectionText);
        }



        Context.AddDrawCommand(new DrawTextCommand(text, sf.ScreenPos + sf.Cursor, props));



        Context.PopId();
        Context.LastStackFrame.Cursor = sf.Cursor + new Vec2 { X = 0, Y = 1 };

        listState.CurrentIndex++;
        return ret;
    }


    public static void EndList()
    {
        var sf = Context.CascadedStackFrame;
        var listState = Context.GetComponentState<ListState>(Context.CurrentId);

        int pageHeight = sf.Size.Y - 2;
        int scrollHeight = listState.MaxIndex;

        int scrollbarHeight = pageHeight - 2; // minus top and bottom arrows
        scrollbarHeight = Math.Max(1, scrollbarHeight);

        // Avoid division by zero
        int scrollRange = Math.Max(1, scrollHeight - pageHeight);
        int barHeight = Math.Max(1, scrollbarHeight * pageHeight / scrollHeight);
        int barY = 1 + (listState.ScrollOffset * (scrollbarHeight - barHeight)) / scrollRange;

        Context.AddDrawCommand(new DrawTextCommand("↑", sf.ScreenPos + new Vec2 { X = sf.Size.X - 3, Y = 0 }, new ElementProperties().SetBg(Context.Style.ListScrollbar).SetFg(Context.Style.ListOutline)));
        for(int i = 1; i < sf.Size.Y-3; i++)
            Context.AddDrawCommand(new DrawTextCommand("░", sf.ScreenPos + new Vec2 { X = sf.Size.X - 3, Y = i }, new ElementProperties().SetBg(Context.Style.ListScrollbar).SetFg(Context.Style.ListOutline)));

        Context.AddDrawCommand(new DrawTextCommand("↓", sf.ScreenPos + new Vec2 { X = sf.Size.X - 3, Y = sf.Size.Y-3 }, new ElementProperties().SetBg(Context.Style.ListScrollbar).SetFg(Context.Style.ListOutline)));

        for(int i = barY; i < barY + barHeight; i++)
            Context.AddDrawCommand(new DrawTextCommand("▓", sf.ScreenPos + new Vec2 { X = sf.Size.X - 3, Y = i }, new ElementProperties().SetBg(Context.Style.ListScrollbar).SetFg(Context.Style.ListScrollbarTracker)));

        Context.PopId();
    }


}

public class ListState : ComponentState
{
    public int SelectedIndex { get; set; } = 0;
    public int ScrollOffset { get; set; } = 0;
    public int CurrentIndex { get; set; } = 0;
    public int MaxIndex { get; set; } = 0;
}


public enum ListChangedEvent
{
    OnSelect,
    Continuous
}