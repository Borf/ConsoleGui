using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;

public static partial class Gui
{
    public static void GeneratePopups()
    {
        if (!string.IsNullOrEmpty(Context.ConfirmMessage))
        {
            SetNextWidth(Console.WindowWidth - 20);
            SetNextHeight(9);
            SetNextPosition(new Vec2 { X = 10, Y = 10 });
            Begin("Confirm", WindowFlags.Modal, 1);
            NewLine();
            Text(Context.ConfirmMessage);
            NewLine();

            SetNextWidth((Console.WindowWidth - 24) / 2);
            SetNextBackgroundColor(Style.Danger);
            if(Button("Yes", true))
            {
                Context.ConfirmCallback();
                Context.ConfirmMessage = string.Empty;
            }
            SetNextWidth((Console.WindowWidth - 24) / 2);
            SameLine(1);
            SetNextBackgroundColor(Style.Warning);
            SetNextForegroundColor(Style.Dark);
            SetNextTextColor(Style.Dark);
            if (Button("No", true))
            {
                Context.ConfirmMessage = string.Empty;
            }


            End();
        }


        if (!string.IsNullOrEmpty(Context.ProgressMessage))
        {
            SetNextWidth(Console.WindowWidth - 20);
            SetNextHeight(9);
            SetNextPosition(new Vec2 { X = 10, Y = 10 });
            Begin("Confirm", WindowFlags.Modal, 1);
            NewLine();
            Text(Context.ProgressMessage);
            NewLine();
            Text(Context.ProgressPercent + "%");
            End();
        }
    }


}
