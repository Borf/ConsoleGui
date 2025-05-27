using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.Components;
public class SplitPanel : Panel
{
    public bool Horizontal { get; set; } = false;
    public List<int> Split { get; set; } = new();
    public SplitPanel(bool horizontal)
    {
        Horizontal = horizontal;
    }

    public override void Render(Context context, Vec2 parentPos)
    {
        int left = 0;
        int i = 0;
        foreach(var c in Components)
        {
            c.Pos = parentPos + new Vec2 { X = left, Y = 0 };

            var size = i < Split.Count ? Split[i] : this.Size.X - left;
            if (size == -1)
                size = this.Size.X - left;

            if (Horizontal)
                c.Size = new Vec2 { X = size, Y = Size.Y };
            else
                c.Size = new Vec2 { X = Size.X, Y = size };
            left += size;
            i++;
            if (i < Components.Count)
            {
                if (Horizontal)
                {
                    context.DrawCommands.Add(new DrawTextCommand(Id, "╦", parentPos + new Vec2 { X = left, Y = 0 }, new ElementProperties()));
                    for (int ii = 1; ii < c.Size.Y - 1; ii++)
                        context.DrawCommands.Add(new DrawTextCommand(Id, "║", parentPos + new Vec2 { X = left, Y = ii }, new ElementProperties()));
                    context.DrawCommands.Add(new DrawTextCommand(Id, "╩", parentPos + new Vec2 { X = left, Y = c.Size.Y - 1 }, new ElementProperties()));
                }
                else
                {
                    //╠╣
                }
            }


        }





    }
}
