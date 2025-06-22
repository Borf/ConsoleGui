using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;

public static partial class Gui
{
    public static bool BeginComboBox(string label, string value)
    {
        if (!Context.NextFrameProperties.SameLine)
            Context.SetLastCursor(new Vec2 { X = 0, Y = (Context.CascadedStackFrame.Cursor?.Y ?? 0) + Context.CascadedStackFrame.LastHeight }, 0);

        SetNextTextColorDefault(Style.ComboBoxText);
        PushId(label);
        bool focussed = HandleFocus();
        var sf = Context.CascadedStackFrame;
        var activationState = GetComponentActivationState();
        var state = Context.GetComponentState<ComboboxState>(sf.Id);


        if (Context.MouseStates[0].HasFlag(MouseState.JustPressed))
            if (activationState == ComponentActivationState.Idle && !Context.HoveredComponent.Contains("#comboBoxPopup")) //hardcoded meh
                state.Opened = false;

        if (activationState == ComponentActivationState.Pressed)
        {
            state.Opened = !state.Opened;
        }


        int labelLength = 30;
        label = label.StripHash();
        if (label.Length == 0)
            labelLength = 0;

        if (labelLength > 0)
            Context.AddDrawCommand(new DrawTextCommand(label, sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(Style.WindowBackground).SetFg(Style.InputText)));

        int width = Context.LastStackFrame.Size?.X ?? value.Length + 4;
        Context.AddDrawCommand(new DrawTextCommand(value.PadLength(width-3) + "   ", sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength, Y = 0 }, new ElementProperties().SetBg(Style.ComboBoxbackground).SetFg(sf.TextColor.Value)));
        Context.AddDrawCommand(new DrawTextCommand(" ↓ ", sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength + width-3, Y = 0 }, new ElementProperties().SetBg(Style.ComboBoxButtonBackground).SetFg(sf.TextColor.Value)));

        if (state.Opened)
        {
            SetNextBackgroundColor(Style.ComboBoxbackground);
            SetNextForegroundColor(Style.ComboBoxText);
            Context.NextFrameProperties.ScreenPos = sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength-1, Y = 0 };
            Context.NextFrameProperties.Size = new Vec2 { X = 30, Y = 2 };
            Context.NextFrameProperties.AddMargin(new Vec2 { X = 2, Y = 1 });
            Begin("#comboBoxPopup#" + sf.Id, WindowFlags.HideBorder);
            var subMenuState = Context.GetComponentState<ComboboxState>(Context.CurrentId);
            subMenuState.ItemCount = 0;
            subMenuState.Opened = true;
            return true;
        }
        else
        {
            Context.PopId();
            Context.SetLastCursor(new Vec2 { X = sf.Cursor.X + width, Y = sf.Cursor.Y }, 1);
        }
        return state.Opened;
    }


    public static void EndComboBox()
    {
        var menuState = Context.GetComponentState<ComboboxState>(Context.CurrentId);
        Context.LastStackFrame.Size = new Vec2 { X = 30, Y = menuState.ItemCount + 2 };
        var sf = Context.CascadedStackFrame;
        Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos! + new Vec2 { X = -2, Y = -1 }, sf.Size!, DrawBorderCommand.BorderType.Double, true));
        Gui.End();

        if (!menuState.Opened)
        {
            var state = Context.GetComponentState<ComboboxState>(Context.CurrentId);
            state.Opened = false;
        }
        Context.PopId();

    }

    public static bool ComboBoxEntry(string label, bool selected)
    {
        var menuState = Context.GetComponentState<ComboboxState>(Context.CurrentId);
        menuState.ItemCount++;

        Context.PushId(label);
        var sf = Context.CascadedStackFrame;

        var state = GetComponentActivationState();

        var color = Context.LastStackFrame.BackgroundColor;
        if (state == ComponentActivationState.Hovered)
            color ??= Style.ComboBoxbackground;
        else if (state == ComponentActivationState.Down || state == ComponentActivationState.Pressed)
            color ??= Style.ComboBoxbackground.Darker().Darker();
        else
            color ??= Style.ComboBoxbackground;


        Context.AddDrawCommand(new DrawTextCommand(label, sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(color.Value).SetFg(Style.ComboBoxText)));
        Context.PopId();
        Context.LastStackFrame.Cursor = sf.Cursor + new Vec2 { X = 0, Y = 1 };
        if (state == ComponentActivationState.Released)
            menuState.Opened = false;
        return state == ComponentActivationState.Released;
    }



    public class ComboboxState : ComponentState
    {
        public bool Opened { get; set; } = false;
        public int ItemCount { get; set; } = 0;
    }

}
