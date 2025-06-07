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
        Context.PushId(label);
        bool focussed = HandleFocus();



        var sf = Context.CascadedStackFrame;
        var activationState = GetComponentActivationState();
        var color = Context.Style.InputCenter;
        var state = Context.GetComponentState<TextInputState>(sf.Id);

        var textViewX = sf.ScreenPos.X + 30 + (big ? 2 : 1);
        int textViewSize = sf.Size.X - 30 - (big ? 4 : 2);

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
                        }
                        break;
                    case Key.Delete:
                        break;
                }
            }

        }




        if (focussed)
            color = Context.Style.InputSelected;
        else if (activationState == ComponentActivationState.Hovered)
            color = Context.Style.InputHovered;
        else //if (state == ComponentActivationState.Idle)
            color = Context.Style.InputCenter;
        Context.AddDrawCommand(new DrawTextCommand(label, sf.ScreenPos + sf.Cursor + new Vec2 { X = 0, Y = big?1:0 }, new ElementProperties().SetBg(Context.Style.WindowBackground).SetFg(Context.Style.InputText)));


        if(focussed)
        {
            Context.MouseFocus = sf.ScreenPos + sf.Cursor + new Vec2 { X = 30 + state.Cursor + (big ? 2 : 1), Y = big ? 1 : 0 };
        }


        if (big)
        {
            Context.LastStackFrame.BackgroundColor = Context.Style.InputCenter;
            Context.LastStackFrame.ForegroundColor = focussed ? Context.Style.InputBorderSelected : Context.Style.InputBorder;

            Context.AddDrawCommand(new DrawBorderCommand(sf.ScreenPos + sf.Cursor + new Vec2 { X = 30, Y = 0}, new Vec2 { X = textViewSize+4, Y = 3 }, DrawBorderCommand.BorderType.Round));

            if(state.Cursor == state.CursorSelection) // no selection
                Context.AddDrawCommand(new DrawTextCommand(value, sf.ScreenPos + sf.Cursor + new Vec2 { X = 32, Y = 1 }, new ElementProperties().SetBg(color).SetFg(Context.Style.InputText)));
            else
            {
                var pre = value[0..Math.Min(state.CursorSelection, state.Cursor)];
                var selection = value[Math.Min(state.CursorSelection, state.Cursor)..Math.Max(state.CursorSelection, state.Cursor)];
                var post = value[Math.Max(state.CursorSelection, state.Cursor)..];

                Context.AddDrawCommand(new DrawTextCommand(pre, sf.ScreenPos + sf.Cursor + new Vec2 { X = 32, Y = 1 }, new ElementProperties().SetBg(color).SetFg(Context.Style.InputText)));
                Context.AddDrawCommand(new DrawTextCommand(selection, sf.ScreenPos + sf.Cursor + new Vec2 { X = 32 + pre.Length, Y = 1 }, new ElementProperties().SetBg(Context.Style.InputText).SetFg(color)));
                Context.AddDrawCommand(new DrawTextCommand(post, sf.ScreenPos + sf.Cursor + new Vec2 { X = 32 + pre.Length + selection.Length, Y = 1 }, new ElementProperties().SetBg(color).SetFg(Context.Style.InputText)));


            }
        }
        else
        {
            var textColor = focussed ? Context.Style.InputBorderSelected : Context.Style.InputBorder;
            Context.AddDrawCommand(new DrawTextCommand($"▏{new string(' ', textViewSize)}▕", sf.ScreenPos + sf.Cursor + new Vec2 { X = 30, Y = 0 }, new ElementProperties().SetBg(color).SetFg(textColor).SetUnderLine().SetOverLine()));
            if (state.Cursor == state.CursorSelection) // no selection
                Context.AddDrawCommand(new DrawTextCommand($"{value}", sf.ScreenPos + sf.Cursor + new Vec2 { X = 31, Y = 0 }, new ElementProperties().SetBg(color).SetFg(textColor).SetUnderLine().SetOverLine()));
            else
            {
                var pre = value[0..Math.Min(state.CursorSelection, state.Cursor)];
                var selection = value[Math.Min(state.CursorSelection, state.Cursor)..Math.Max(state.CursorSelection, state.Cursor)];
                var post = value[Math.Max(state.CursorSelection, state.Cursor)..];

                Context.AddDrawCommand(new DrawTextCommand(pre, sf.ScreenPos + sf.Cursor + new Vec2 { X = 31, Y = 0 }, new ElementProperties().SetBg(color).SetFg(textColor).SetUnderLine().SetOverLine()));
                Context.AddDrawCommand(new DrawTextCommand(selection, sf.ScreenPos + sf.Cursor + new Vec2 { X = 31+pre.Length, Y = 0 }, new ElementProperties().SetBg(textColor).SetFg(color).SetUnderLine().SetOverLine()));
                Context.AddDrawCommand(new DrawTextCommand(post, sf.ScreenPos + sf.Cursor + new Vec2 { X = 31+pre.Length+selection.Length, Y = 0 }, new ElementProperties().SetBg(color).SetFg(textColor).SetUnderLine().SetOverLine()));
            }
        }

        Context.PopId();
        Context.LastStackFrame.Cursor += new Vec2 { X = 0, Y = big ? 3 : 1 };
        return false;
    }


}


public class TextInputState : ComponentState
{
    public int Cursor { get; set; } = 0;
    public int CursorSelection { get; set; } = 0;
    public int ScrollOffset { get; set; } = 0;
}
