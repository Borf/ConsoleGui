using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.DrawCommands;
public abstract class DrawCommand
{
    public string Id { get; set; } = string.Empty;
    public Color BackgroundColor { get; set; }
    public Color ForegroundColor { get; set; }
    public Color TextColor { get; set; }

    public abstract void Draw(FrameBuffer buffer);
}
