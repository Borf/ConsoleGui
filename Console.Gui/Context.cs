using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;
public class Context
{
    public List<Window> Windows { get; set; } = new();
    public List<string> WindowOrder { get; set; } = new();
    //public LinkedList<Panel> PanelStack { get; set; } = new();
    public LinkedList<StackFrame> Stack { get; set; } = new();
    public StackFrame LastStackFrame => Stack.Last();
    public StackFrame CascadedStackFrame => new StackFrame()
    {
        Id = CurrentId,
        FrameType = Stack.LastOrDefault()?.FrameType, //should this be the last one or the cascaded one?
        Cursor = Stack.LastOrDefault(sf => sf.Cursor != null)?.Cursor,
        Size = Stack.LastOrDefault(sf => sf.Size != null)?.Size,
        ScreenPos = Stack.LastOrDefault(sf => sf.ScreenPos != null)?.ScreenPos,
    };
    public Window? CurrentWindow { get; set; } = null;
    public string CurrentId => string.Join("/", Stack.Select(s => s.Id));
    public void AddDrawCommand(DrawCommand drawCommand)
    {
        drawCommand.Id = CurrentId;
        Debug.Assert(CurrentWindow != null, "CurrentWindow should not be null when adding a draw command.");
        CurrentWindow.DrawCommands.Add(drawCommand);
    }



    public void PushId(string id)
    {
        var sf = new StackFrame() { Id = id };
        Stack.AddLast(sf);
    }
    public void PopId()
    {
        Stack.RemoveLast();
    }
    
    
    public Dictionary<string, ComponentState> ComponentStates = new();

    public T GetComponentState<T>(string id) where T : ComponentState, new()
    {
        if (!ComponentStates.ContainsKey(id))
            ComponentStates[id] = new T();
        return (T)ComponentStates[id];
    }

    
    
    public Vec2 MousePos = Vec2.Zero;
    public MouseState[] MouseStates { get; set; } = [MouseState.Up, MouseState.Up, MouseState.Up];

    public string HoveredComponent { get; set; } = string.Empty;

    public Style Style { get; set; } = new Style();
}

public class ComponentState
{

}

public enum MouseState
{
    Up,
    Down,
    Pressed,
    Released,
}