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
    public static bool CheckBox(string label, ref bool value)
    {
        if (!Context.NextFrameProperties.SameLine)
            Context.SetLastCursor(new Vec2 { X = 0, Y = (Context.CascadedStackFrame.Cursor?.Y ?? 0) + Context.CascadedStackFrame.LastHeight }, 0);

        SetNextBackgroundColorDefault(Style.CheckboxBackground);
        SetNextTextColorDefault(Style.CheckboxText);
        Context.PushId(label);

        var activationState = GetComponentActivationState();
        if(activationState == ComponentActivationState.Pressed)
            value = !value;

        var sf = Context.CascadedStackFrame;
        int width = Context.LastStackFrame.Size?.X ?? label.Length + 1;
        Context.AddDrawCommand(new DrawTextCommand(value ? "▏×▕" : "▏ ▕", sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(sf.BackgroundColor.Value).SetFg(sf.TextColor.Value).SetOverLine().SetUnderLine()));
        Context.PopId();

        Context.SetLastCursor(new Vec2 { X = sf.Cursor.X + width, Y = sf.Cursor.Y }, 1);

        return activationState == ComponentActivationState.Pressed;
    }

    public static bool CheckBox(string label, bool value)
    {
        if (!Context.NextFrameProperties.SameLine)
            Context.SetLastCursor(new Vec2 { X = 0, Y = (Context.CascadedStackFrame.Cursor?.Y ?? 0) + Context.CascadedStackFrame.LastHeight }, 0);

        SetNextBackgroundColorDefault(Style.CheckboxBackground);
        SetNextTextColorDefault(Style.CheckboxText);
        Context.PushId(label);
        label = label.StripHash();
        var activationState = GetComponentActivationState();

        var sf = Context.CascadedStackFrame;
        int width = Context.LastStackFrame.Size?.X ?? label.Length + 3;
        Context.AddDrawCommand(new DrawTextCommand(value ? "▏×▕" : "▏ ▕", sf.ScreenPos + sf.Cursor, new ElementProperties().SetBg(sf.BackgroundColor.Value).SetFg(sf.TextColor.Value).SetOverLine().SetUnderLine()));
        Context.PopId();

        Context.SetLastCursor(new Vec2 { X = sf.Cursor.X + width, Y = sf.Cursor.Y }, 1);

        return activationState == ComponentActivationState.Pressed;
    }
}
