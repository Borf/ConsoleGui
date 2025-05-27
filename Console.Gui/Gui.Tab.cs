using ConGui.Components;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;

public static partial class Gui
{
    public static void BeginTabPanel(string tabPaneId)
    {
        var sp = new TabPanel { Id = tabPaneId };
        //Context.LastPanel.Margin = new Vec2 { X = 2, Y = 2 };
        Context.LastPanel.Add(sp);
        Context.PanelStack.AddLast(sp);
    }

    public static void EndTabPanel()
    {
        Context.PanelStack.RemoveLast();
    }

    public static bool BeginTab(string title)
    {
        return false;
    }

    public static void EndTab()
    {
        
    }
}
