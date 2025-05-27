using ConGui.DrawCommands;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.Components;
public abstract class Component
{
    public string Id { get; set; } = string.Empty;
    public Vec2 Pos { get; set; } = new() { X = 0, Y = 0 };
    public Vec2 Size { get; set; } = new() { X = 10, Y = 10 };
    public List<Component> Components { get; set; } = new();

    public Vec2 Margin { get; set; } = Vec2.Zero;

    public void Add(Component component)
    {
        Components.Add(component);
    }


    public abstract void Render(Context context, Vec2 parentPos);

}
