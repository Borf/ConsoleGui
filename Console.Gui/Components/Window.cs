using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Windows.Markup;

namespace ConGui.Components;

public class Window : Panel
{
    public required WindowFlags Flags { get; set; }
    public MenuBar? MenuBar { get; internal set; }


}
