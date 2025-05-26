using ConGui.Components;
using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;
public class Context
{
    public List<Window> Windows { get; set; } = new List<Window>();
    public Window? CurrentWindow { get; set; }

    public Window? HoveredWindow { get; set; }

    public Style Style { get; set; } = new Style();
    public List<DrawCommand> DrawCommands { get; set; } = new List<DrawCommand>();
}
