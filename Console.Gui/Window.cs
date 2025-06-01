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
        HasBorder = Stack.LastOrDefault(sf => sf.HasBorder != null)?.HasBorder
    };
    public string CurrentId => string.Join("/", Stack.Select(s => s.Id));

    public void PushId(string id)
    {
        var sf = new StackFrame() { Id = id };
        Stack.AddLast(sf);
    }
    public void PopId()
    {
        Stack.RemoveLast();
    }
}
