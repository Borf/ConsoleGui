using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.Components;
public class TabPanel : Panel
{
    public override void Render(Context context, Vec2 parentPos)
    {
        string[] tabs = ["tab1", "tab2", "tab3"];

        Vec2 p = Pos + parentPos + new Vec2 { X = 1, Y = 0 };
        foreach(var tab in tabs)
        {
            context.DrawCommands.Add(new DrawTextCommand(Id, tab + " │", p, new ElementProperties().SetBg(context.Style.WindowBackground).SetFg(context.Style.WindowForeground)));
            context.DrawCommands.Add(new DrawTextCommand(Id, new string('─', tab.Length+2) + "┴", p + new Vec2 { X = -1, Y = 1 }, new ElementProperties().SetBg(context.Style.WindowBackground).SetFg(context.Style.WindowForeground)));
            p += new Vec2 { X = tab.Length + 3, Y = 0 }; // Move position for next tab
        }
        context.DrawCommands.Add(new DrawTextCommand(Id, new string('─', Size.X - p.X), p + new Vec2 { X = -1, Y = 1 }, new ElementProperties().SetBg(context.Style.WindowBackground).SetFg(context.Style.WindowForeground)));



    }
}
