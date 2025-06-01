using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Windows.Markup;

namespace ConGui;

public class Window
{
    public required string Id { get; set; }
    public List<DrawCommand> DrawCommands { get; set; } = new List<DrawCommand>();
}
