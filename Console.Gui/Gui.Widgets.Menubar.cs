using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        var menuState = Context.GetComponentState<MenuState>(Context.CurrentId);
        var menuId = Context.CurrentId;

        Context.PushId(value);

        if (Context.MouseStates[0].HasFlag(MouseState.Pressed))
            if (!Context.HoveredComponent.StartsWith(menuId) && !Context.HoveredComponent.Contains("#menuPopup")) //hardcoded meh
                menuState.OpenedId = string.Empty;

        if (Context.MouseStates[0].HasFlag(MouseState.Pressed))
            if (Context.HoveredComponent == Context.CurrentId)
                menuState.OpenedId = Context.CurrentId;

        bool hovered = Context.HoveredComponent == Context.CurrentId;
        if(hovered && menuState.OpenedId.StartsWith(menuId))
            menuState.OpenedId = Context.CurrentId;

        var color = (menuState.OpenedId == Context.CurrentId) ? Context.Style.MenuOpened : (hovered ? Context.Style.MenuBackground.Darker().Darker() : Context.Style.MenuBackground);
        Context.AddDrawCommand(new DrawTextCommand(value, sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(color).SetFg(Context.Style.WindowForeground)));
        var menuItemId = Context.CurrentId;
        Context.PopId();

        Context.LastStackFrame.Cursor = Context.CascadedStackFrame.Cursor + new Vec2 { X = value.Length + 2, Y = 0 }; // +1 for the space after the menu item

        if (menuState.OpenedId == menuItemId)
        {
            SetNextBackgroundColor(Context.Style.MenuBackground);
            Context.NextFrameProperties.ScreenPos = sf.ScreenPos + sf.Cursor + new Vec2 { X = -1, Y = 0 };
            Context.NextFrameProperties.Size = new Vec2 { X = 30, Y = 2 };
            Context.NextFrameProperties.AddMargin(new Vec2 { X = 2, Y = 1 });
            Begin("#menuPopup#" + value, WindowFlags.HideBorder);
            var subMenuState = Context.GetComponentState<MenuState>(Context.CurrentId);
            subMenuState.ItemCount = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool MenuItem(string label)
    {
        var menuState = Context.GetComponentState<MenuState>(Context.CurrentId);
        menuState.ItemCount++;

        Context.PushId(label);
        var sf = Context.CascadedStackFrame;

        int state = 0;

        if(Context.HoveredComponent == sf.Id)
        {
            state = 1;
            if (Context.MouseStates[0] == MouseState.Pressed)
                state = 2;
        }

        var color = Context.LastStackFrame.BackgroundColor;
        if(state == 0)
            color ??= Context.Style.MenuBackground;
        if(state == 1)
            color ??= Context.Style.MenuBackground.Darker();
        if (state == 2)
            color ??= Context.Style.MenuBackground.Darker().Darker();


        Context.AddDrawCommand(new DrawTextCommand(label, sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(color.Value).SetFg(Context.Style.WindowForeground)));
        Context.PopId();
        Context.LastStackFrame.Cursor = sf.Cursor + new Vec2 { X = 0, Y = 1 };

        return state == 2;
    }

    //Because the menu is a dynamically resized window, draw the window after the contents so the size can still change while it is being drawn. Dirty hack, should fix this and make it more generic
    public static void EndMenu()
    {
        var menuState = Context.GetComponentState<MenuState>(Context.CurrentId);
        Context.LastStackFrame.Size = new Vec2 { X = 30, Y = menuState.ItemCount+2 };
        var sf = Context.CascadedStackFrame;
        Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos! + new Vec2 { X = -2, Y = -1 }, sf.Size!, DrawBorderCommand.BorderType.Double, true));
        Gui.End();
    }


}


class MenuState : ComponentState
{
    public int ItemCount { get; set; } = 0;
    public string OpenedId { get; set; } = string.Empty;
}
