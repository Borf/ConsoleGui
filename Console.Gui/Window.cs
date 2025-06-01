using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Windows.Markup;

namespace ConGui;

public class Window
{
    public required string Id { get; set; }
    public List<DrawCommand> DrawCommands { get; set; } = new List<DrawCommand>();

    public LinkedList<StackFrame> Stack { get; set; } = new();
    public StackFrame LastStackFrame => Stack.Last();
    public StackFrame CascadedStackFrame => new StackFrame()
    {
        Id = CurrentId,
        FrameType = Stack.LastOrDefault()?.FrameType, //should this be the last one or the cascaded one?
        Cursor = Stack.LastOrDefault(sf => sf.Cursor != null)?.Cursor,
        Size = Stack.LastOrDefault(sf => sf.Size != null)?.Size,
        ScreenPos = Stack.LastOrDefault(sf => sf.ScreenPos != null)?.ScreenPos,
        HasBorder = Stack.LastOrDefault(sf => sf.HasBorder != null)?.HasBorder,
        BackgroundColor = Stack.LastOrDefault(sf => sf.BackgroundColor != null)?.BackgroundColor,
        ForegroundColor = Stack.LastOrDefault(sf => sf.ForegroundColor != null)?.ForegroundColor,
        TextColor = Stack.LastOrDefault(sf => sf.TextColor != null)?.TextColor,
    };
    public string CurrentId => string.Join("/", Stack.Select(s => s.Id));

    public void PushId(StackFrame templateFrame, string id)
    {
        var sf = new StackFrame() { Id = id };
        if (templateFrame.HasBorder.HasValue)
            sf.HasBorder = templateFrame.HasBorder;
        if(templateFrame.Size != null)
            sf.Size = templateFrame.Size;
        if(templateFrame.ScreenPos != null)
            sf.ScreenPos = templateFrame.ScreenPos;
        if(templateFrame.Cursor != null)
            sf.Cursor = templateFrame.Cursor;
        if(templateFrame.ForegroundColor != null)
            sf.ForegroundColor = templateFrame.ForegroundColor;
        if (templateFrame.BackgroundColor != null)
            sf.BackgroundColor = templateFrame.BackgroundColor;
        if (templateFrame.TextColor != null)
            sf.TextColor = templateFrame.TextColor;
        if (templateFrame.FrameType != null)
            sf.FrameType = templateFrame.FrameType;
        Stack.AddLast(sf);
    }
    public void PopId()
    {
        Stack.RemoveLast();
    }
}
