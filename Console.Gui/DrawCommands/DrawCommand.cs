using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.DrawCommands;
public abstract class DrawCommand
{
    public string Id { get; set; } = string.Empty;

    public abstract void Draw(FrameBuffer buffer);
}
