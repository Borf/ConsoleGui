using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.DrawCommands;
public interface DrawCommand
{
    public void Draw(FrameBuffer buffer);
}
