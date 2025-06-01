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
    public StackFrame LastStackFrame => CurrentWindow.Stack.Last();
    public StackFrame CascadedStackFrame => CurrentWindow.CascadedStackFrame;
    public string CurrentId => CurrentWindow?.CurrentId ?? "";


    public LinkedList<Window> WindowCreationStack { get; set; } = new();
    public Window? CurrentWindow => WindowCreationStack.LastOrDefault();
    public void AddDrawCommand(DrawCommand drawCommand)
    {
        drawCommand.Id = CurrentId;
        Debug.Assert(CurrentWindow != null, "CurrentWindow should not be null when adding a draw command.");
        CurrentWindow.DrawCommands.Add(drawCommand);
    }
    public void PushId(string id) => CurrentWindow.PushId(id);
    public void PopId() => CurrentWindow.PopId();
    
    
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

