using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;
public class StackFrame
{
    public required string Id { get; set; } = string.Empty;
    public string? FrameType { get; set; } = null;
    public Vec2? Cursor { get; set; } = null;

    public Vec2? ScreenPos { get; set; } = null;
    public Vec2? Size { get; set; } = null;

    //TODO: style?
}
