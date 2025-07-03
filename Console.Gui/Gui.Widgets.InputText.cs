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
    public static bool InputText(string label, bool big, ref string value)
    {
        if (!Context.NextFrameProperties.SameLine)
            Context.SetLastCursor(new Vec2 { X = 0, Y = (Context.CascadedStackFrame.Cursor?.Y ?? 0) + Context.CascadedStackFrame.LastHeight }, 0);

        bool returnValue = false;
        Context.PushId(label);
        bool focussed = HandleFocus();
        var sf = Context.CascadedStackFrame;
        var activationState = GetComponentActivationState();
        var color = Style.InputCenter;
        var state = Context.GetComponentState<TextInputState>(sf.Id);

        if (state.Cursor > value.Length)
            state.Cursor = value.Length;
        else if(state.CursorSelection > value.Length)
            state.CursorSelection = value.Length;

        int labelLength = 30;
        label = label.StripHash();
        if (label.Length == 0)
            labelLength = 0;
        if (labelLength > sf.Size.X - 10)
            labelLength = sf.Size.X - 10;

        var textViewX = sf.ScreenPos.X + labelLength + (big ? 2 : 1);
        int textViewSize = sf.Size.X - labelLength - (big ? 4 : 2);


        if(state.CursorSelection - state.ScrollOffset > textViewSize)
        {
            state.ScrollOffset = state.CursorSelection - textViewSize;
        }
        if (state.CursorSelection - state.ScrollOffset < 0)
        {
            state.ScrollOffset = state.CursorSelection;
        }

        if (focussed)
        {
            if (Context.MouseStates[0] == MouseState.Down)
            {
                state.Cursor = Math.Clamp(Context.MousePos.X - textViewX, 0, value.Length);
                state.CursorSelection = Math.Clamp(Context.MousePos.X - textViewX, 0, value.Length);
            }

            if (!string.IsNullOrEmpty(Context.KeyInput))
            {
                if(state.CursorSelection != state.Cursor)
                {
                    value = value[0..Math.Min(state.CursorSelection, state.Cursor)] + value[Math.Max(state.Cursor, state.CursorSelection)..];
                    state.Cursor = Math.Min(Math.Min(value.Length, state.Cursor), state.CursorSelection);
                    state.CursorSelection = state.Cursor;
                }
                value = value[0..state.Cursor] + Context.KeyInput + value[state.Cursor..];
                state.Cursor += Context.KeyInput.Length;
                state.CursorSelection = state.Cursor;
            }

            foreach(var key in Context.KeyButtonInput)
            {
                switch (key.Key)
                {
                    case Key.Enter:
                        {
                            returnValue = true;
                        }
                        break;
                    case Key.A:
                        if(key.Modifier.HasFlag(KeyModifier.Ctrl))
                        {
                            state.CursorSelection = 0;
                            state.Cursor = value.Length;
                        }
                        break;
                    case Key.Left:
                        state.Cursor = Math.Max(0, state.Cursor - 1);
                        if(!key.Modifier.HasFlag(KeyModifier.Shift))
                            state.CursorSelection = state.Cursor;
                        break;
                    case Key.Right:
                        state.Cursor = Math.Min(value.Length, state.Cursor + 1);
                        if (!key.Modifier.HasFlag(KeyModifier.Shift))
                            state.CursorSelection = state.Cursor;
                        break;
                    case Key.Home:
                        state.Cursor = 0;
                        if (!key.Modifier.HasFlag(KeyModifier.Shift))
                            state.CursorSelection = state.Cursor;
                        break;
                    case Key.End:
                        if (state.CursorSelection != state.Cursor)
                        {
                            value = value[0..Math.Min(state.CursorSelection, state.Cursor)] + value[Math.Max(state.Cursor, state.CursorSelection)..];
                            state.Cursor = state.CursorSelection;
                        }
                        else
                        {
                            state.Cursor = value.Length;
                            if (!key.Modifier.HasFlag(KeyModifier.Shift))
                                state.CursorSelection = state.Cursor;
                        }
                        break;
                    case Key.Backspace:
                        if (state.CursorSelection != state.Cursor)
                        {
                            value = value[0..Math.Min(state.CursorSelection, state.Cursor)] + value[Math.Max(state.Cursor, state.CursorSelection)..];
                            state.Cursor = Math.Min(Math.Min(value.Length, state.Cursor), state.CursorSelection);
                            state.CursorSelection = state.Cursor;
                        }
                        else if (state.Cursor > 0)
                        {
                            state.Cursor--;
                            value = value[0..state.Cursor] + value[(state.Cursor + 1)..];
                            state.CursorSelection = state.Cursor;
                        }
                        break;
                    case Key.Delete:
                        break;
                }
            }

        }




        if (focussed)
            color = Style.InputSelected;
        else if (activationState == ComponentActivationState.Hovered)
            color = Style.InputHovered;
        else //if (state == ComponentActivationState.Idle)
            color = Style.InputCenter;

        if(labelLength > 0)
            Context.AddDrawCommand(new DrawTextCommand(label, sf.ScreenPos + sf.Cursor + new Vec2 { X = 0, Y = big?1:0 }, new ElementProperties().SetBg(Style.WindowBackground).SetFg(Style.InputText)));


        if(focussed)
        {
            Context.MouseFocus = sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength + state.Cursor + (big ? 2 : 1) - state.ScrollOffset, Y = big ? 1 : 0 };
        }


        if (big)
        {
            Context.LastStackFrame.BackgroundColor = Style.InputCenter;
            Context.LastStackFrame.ForegroundColor = focussed ? Style.InputBorderSelected : Style.InputBorder;

            Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength, Y = 0}, new Vec2 { X = textViewSize+4, Y = 3 }, DrawBorderCommand.BorderType.Round));

            if(state.Cursor == state.CursorSelection) // no selection
                Context.AddDrawCommand(new DrawTextCommand(value, sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength+2, Y = 1 }, new ElementProperties().SetBg(color).SetFg(Style.InputText)));
            else
            {
                var pre = value[0..Math.Min(state.CursorSelection, state.Cursor)];
                var selection = value[Math.Min(state.CursorSelection, state.Cursor)..Math.Max(state.CursorSelection, state.Cursor)];
                var post = value[Math.Max(state.CursorSelection, state.Cursor)..];

                Context.AddDrawCommand(new DrawTextCommand(pre, sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength+2, Y = 1 }, new ElementProperties().SetBg(color).SetFg(Style.InputText)));
                Context.AddDrawCommand(new DrawTextCommand(selection, sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength+2 + pre.Length, Y = 1 }, new ElementProperties().SetBg(Style.InputText).SetFg(color)));
                Context.AddDrawCommand(new DrawTextCommand(post, sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength+2 + pre.Length + selection.Length, Y = 1 }, new ElementProperties().SetBg(color).SetFg(Style.InputText)));


            }
        }
        else
        {
            var textColor = focussed ? Style.InputBorderSelected : Style.InputBorder;
            Context.AddDrawCommand(new DrawTextCommand($"▏{new string(' ', textViewSize)}▕", sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength, Y = 0 }, new ElementProperties().SetBg(color).SetFg(textColor).SetUnderLine().SetOverLine()));
            if (state.Cursor == state.CursorSelection) // no selection
            {
                var text = value[state.ScrollOffset..];
                if (text.Length > textViewSize)
                    text = text[0..textViewSize];

                Context.AddDrawCommand(new DrawTextCommand($"{text}", sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength + 1, Y = 0 }, new ElementProperties().SetBg(color).SetFg(textColor).SetUnderLine().SetOverLine()));
            }
            else
            {
                var pre = value[state.ScrollOffset..Math.Min(state.CursorSelection, state.Cursor)];
                var selection = value[Math.Min(state.CursorSelection, state.Cursor)..Math.Max(state.CursorSelection, state.Cursor)];
                var post = value[Math.Max(state.CursorSelection, state.Cursor)..];

                if (pre.Length + selection.Length + post.Length > textViewSize)
                    post = post[0..(textViewSize - pre.Length - selection.Length)];


                Context.AddDrawCommand(new DrawTextCommand(pre, sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength + 1, Y = 0 }, new ElementProperties().SetBg(color).SetFg(textColor).SetUnderLine().SetOverLine()));
                Context.AddDrawCommand(new DrawTextCommand(selection, sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength + 1 + pre.Length, Y = 0 }, new ElementProperties().SetBg(textColor).SetFg(color).SetUnderLine().SetOverLine()));
                Context.AddDrawCommand(new DrawTextCommand(post, sf.ScreenPos + sf.Cursor + new Vec2 { X = labelLength + 1 + pre.Length + selection.Length, Y = 0 }, new ElementProperties().SetBg(color).SetFg(textColor).SetUnderLine().SetOverLine()));
            }
        }

        Context.PopId();
        Context.SetLastCursor(new Vec2 { X = sf.Cursor.X + sf.Size.X, Y = sf.Cursor.Y }, big ? 3 : 1);
        return returnValue;
    }



    public static bool InputButton(string title, ref string value, string buttonText)
    {
        bool returnValue = false;
        Gui.SetNextWidth(Gui.CurrentSize.X - buttonText.Length-5-4);
        returnValue |= Gui.InputText(title, true, ref value);
        Gui.SameLine(1);
        Gui.SetNextWidth(buttonText.Length+4);
        returnValue |= Gui.Button("Create", true);
        return returnValue;
    }





}


public class TextInputState : ComponentState
{
    public int Cursor { get; set; } = 0;
    public int CursorSelection { get; set; } = 0;
    public int ScrollOffset { get; set; } = 0;
}
