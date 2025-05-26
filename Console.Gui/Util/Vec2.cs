using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.Util;
public record Vec2
{
    public required int X { get; init; }
    public required int Y { get; init; }


    public static Vec2 Zero = new Vec2 { X = 0, Y = 0 };

    public static Vec2 operator +(Vec2 a, Vec2 b)
    {
        return new Vec2() { X = a.X + b.X, Y = a.Y + b.Y };
    }
    
    public static Vec2 operator -(Vec2 a, Vec2 b)
    {
        return new Vec2() { X = a.X - b.X, Y = a.Y - b.Y };
    }
}
