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
    public StackFrame NextFrameProperties { get; private set; } = new() { Id = string.Empty };

    public List<Window> Windows { get; set; } = new();
    public List<string> WindowOrder { get; set; } = new();
    public StackFrame LastStackFrame => CurrentWindow?.Stack?.LastOrDefault() ?? throw new Exception("Current window not set");
    public StackFrame CascadedStackFrame => CurrentWindow?.CascadedStackFrame ?? throw new Exception("Current window not set");
    public string CurrentId => CurrentWindow?.CurrentId ?? "";


    public void SetLastCursor(Vec2 cursor, int height)
    {
        if (CurrentWindow == null)
            throw new Exception("Current window not set");
        CurrentWindow.Stack.Last(c => c.Cursor != null).Cursor = cursor;
        CurrentWindow.Stack.Last(c => c.Cursor != null).LastHeight = height;
    }

    public string FocussedComponent = string.Empty;
    public Vec2? MouseFocus = null;

    public LinkedList<Window> WindowCreationStack { get; set; } = new();
    public Window? CurrentWindow => WindowCreationStack.LastOrDefault();
    public void AddDrawCommand(DrawCommand drawCommand)
    {
        var sf = CascadedStackFrame;

        drawCommand.Id = sf.Id;
        drawCommand.BackgroundColor = sf.BackgroundColor ?? throw new Exception("Need to have colors to draw");
        drawCommand.ForegroundColor = sf.ForegroundColor ?? throw new Exception("Need to have colors to draw");
        drawCommand.TextColor = sf.TextColor ?? throw new Exception("Need to have colors to draw");
        Debug.Assert(CurrentWindow != null, "CurrentWindow should not be null when adding a draw command.");
        CurrentWindow.DrawCommands.Add(drawCommand);
    }
    public void PushId(string id)
    {
        CurrentWindow?.PushId(NextFrameProperties, id);
        NextFrameProperties = new() { Id = string.Empty };
    }
    public void PopId() => CurrentWindow?.PopId();
    
    
    public Dictionary<string, ComponentState> ComponentStates = new();

    public T GetComponentState<T>(string id) where T : ComponentState, new()
    {
        if (!ComponentStates.ContainsKey(id))
            ComponentStates[id] = new T();
        return (T)ComponentStates[id];
    }


    public string KeyInput = string.Empty;

    public List<(KeyModifier Modifier, Key Key)> KeyButtonInput = new();
    public Vec2 MousePos = Vec2.Zero;
    public MouseState[] MouseStates { get; set; } = [MouseState.Up, MouseState.Up, MouseState.Up];

    public string HoveredComponent { get; set; } = string.Empty;
    public int MouseScroll { get; set; } = 0;
}

public class ComponentState
{

}

public enum MouseState
{
    Up,
    Down,
    JustPressed,
    JustReleased,
}

public enum Key
{
    Up,
    Down,
    Left,
    Right,
    Esc,
    Tab,
    Backspace,
    Delete,
    Home,
    End,
    A
}

[Flags]
public enum KeyModifier
{
    None = 0,
    Ctrl = 1<<0,
    Shift = 1<<1,
    Alt = 1<<2,
}