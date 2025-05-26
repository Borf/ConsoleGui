using ConGui.DrawCommands;
using ConGui.Sizes;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.Components;
public class Panel : Component
{
    public string Title { get; set; } = string.Empty;
    public Vec2 Cursor { get; set; } = new Vec2 { X = 0, Y = 0 };

    public Panel()
    {
        Size.Width = Dim.Fill;
        Margin = new Vec2 { X = 2, Y = 1 };
    }

    public override void Render(Context context, Vec2 parentPos)
    {
        context.DrawCommands.Add(new DrawBorderCommand(Pos, CalculatedSize, Id, DrawBorderCommand.BorderType.Double, context.Style.WindowBackground, context.Style.WindowForeground, context.Style.WindowBackground, null));
        if (!string.IsNullOrEmpty(Title))
            context.DrawCommands.Add(new DrawTextCommand(Id, $" {Title} ", Pos + new Vec2 { X = (CalculatedSize.X - Title.Length) / 2, Y = 0 }, new ElementProperties().SetBg(context.Style.WindowBackground).SetFg(context.Style.WindowForeground)));
    }

}
