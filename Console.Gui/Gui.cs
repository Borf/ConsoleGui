using ANSIConsole;
using ConGui.Components;
using ConGui.DrawCommands;
using ConGui.Util;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace ConGui;

public static partial class Gui
{
    private static Context Context { get; set; } = null!;

    public static void CreateContext()
    {
        if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;

        Context = new Context();

        ConsoleMode.Begin();
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Write(
            "\e[?1003h" + // MOUSE_TRACKING_ALL
            //"\u001B[?1005h" + // MOUSE_MODE_UTF8
            //"\u001B[?1002h" + // Cell Motion Mouse Tracking (only click)
            "\u001B[?1006h" + // MOUSE_MODE_SGR
            "\u001B[?1015h" + // MOUSE_MODE_URXVT                 
            "\e[?25l" + // hide cursor
            ""
            );
    }


    public static void BeginFrame()
    {
        Context.PanelStack.Clear();
        Context.Windows.Clear();
    }

    public static LinkedList<double> FrameTimes = new();
    public static int MouseX = 0;
    public static int MouseY = 0;
    public static void Render()
    {
        FrameTimes.AddLast(Environment.TickCount / 1000.0);
        while (FrameTimes.Count > 1000)
            FrameTimes.RemoveFirst();

        var result = "";
        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);
            result += key.KeyChar;
        }

        while (result.Length > 1 && result[0] == '\e')
        {
            string code = result[1..];
            if(code.Contains('\e'))
                code = code[0..(code.IndexOf('\e')-1)];

            if(code.StartsWith("[<"))
            {
                var pressed = code.EndsWith("M");
                var released = code.EndsWith("m");
                if (pressed || released)
                    code = code[0..^1];

                var p = code[2..].Split(";");
                MouseX = int.Parse(p[1])-1;
                MouseY = int.Parse(p[2])-1;
                //TODO: 
            }
            if (result.Length > code.Length + 2)
                result = result[(code.Length + 2)..];
            else
                result = "";
        }



        



        if (OperatingSystem.IsWindows())
        {
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
        }


        Context.DrawCommands.Clear();
        //Console.Write("\e[38;5;243m"); //bg color
        Console.Write("\e[48;5;234m"); //bg color
        //Console.BackgroundColor = Color.DarkBlue;
        //Console.ForegroundColor = Color.Whi;
        Console.CursorVisible = false;

        Action<Component, Vec2> renderComponent = (a,b) => { };
        renderComponent = (Component c, Vec2 offset) =>
        {
            c.Render(Context, offset);
            foreach(var cc in c.Components)
            {
                renderComponent(cc, c.Pos + offset + c.Margin);
            }
        };


        foreach(var window in Context.Windows)
        {
            renderComponent(window, Vec2.Zero);
        }
        Context.DrawCommands.Add(new DrawTextCommand("", Math.Round(FrameTimes.Count / (FrameTimes.Last.Value - FrameTimes.First.Value), 1) + " FPS", new Vec2 { X = 0, Y = 0 }, new ElementProperties().SetBg(Color.Black).SetFg(Color.White)));
        var frameBuffer = new FrameBuffer(Console.WindowWidth, Console.WindowHeight);
        foreach (var command in Context.DrawCommands)
        {
            command.Draw(frameBuffer);
        }

        var hoveredComponent = (MouseX < frameBuffer.Width && MouseY < frameBuffer.Height) ? frameBuffer.Elements[MouseX, MouseY].ObjectId : "";
        new DrawTextCommand(null, hoveredComponent, new Vec2 { X = Console.WindowWidth - hoveredComponent.Length - 1, Y = 0 }, new ElementProperties()).Draw(frameBuffer);
        new DrawTextCommand(null, (Environment.TickCount % 1000 < 500) ? "▓" : "░", new Vec2 { X = MouseX, Y = MouseY }, new ElementProperties()).Draw(frameBuffer);



        frameBuffer.Draw();
    }


    public static void DestroyContext()
    {

    }


    public static void Begin(string title, WindowFlags flags = 0)
    {
        if (Context.PanelStack.Count > 0)
            throw new Exception("Can't begin when there's a window open already");

        //TODO: check if window already exists
        var w = new Window() { Title = title, Flags = flags, Id = title, Parent = null };

        if(flags.HasFlag(WindowFlags.TopWindow))
            w.Size = new Vec2 { X = Console.WindowWidth, Y = Console.WindowHeight };

        Context.PanelStack.AddLast(w);
        Context.Windows.Add(w);
    }
    public static void End()
    {
        if(Context.LastPanel as Window == null)
            throw new Exception("Can't end when there's no window open");

        Context.Windows.Add((Window)Context.LastPanel);
        Context.PanelStack.RemoveLast();
    }

    public static void Text(string text)
    {
        Context.LastPanel.Add(new Label(text, Context.LastPanel.Cursor) { Id = text, Parent = Context.LastPanel });
        Context.LastPanel.Cursor += new Vec2 { X = 0, Y = 1 };
    }

    public static bool CheckBox(string label, ref bool value)
    {
        return false;
    }

    public static void SameLine()
    {

    }

    public static bool InputText(string label, bool big, ref string value)
    {
        Context.LastPanel.Add(new Label(label, Context.LastPanel.Cursor + new Vec2 { X = 0, Y = big ? 1 : 0 }) { Id = label, Parent = Context.LastPanel });
        Context.LastPanel.Cursor += new Vec2 { X = 20, Y = 0 };


        var btn = new TextInput(ref value, Context.LastPanel.Cursor, big) { Id = label, Parent = Context.LastPanel };
        btn.Size = new Vec2 { X = 15, Y = btn.Size.Y };
        Context.LastPanel.Add(btn);
        Context.LastPanel.Cursor = new Vec2 { X = 0, Y = Context.LastPanel.Cursor.Y + btn.Size.Y };

        return false;
    }
    public static bool Button(string text, bool big)
    {
        var btn = new Button(text, Context.LastPanel.Cursor, big) { Id = text, Parent = Context.LastPanel };
        Context.LastPanel.Add(btn);
        Context.LastPanel.Cursor += new Vec2 { X = 0, Y = btn.Size.Y };
        return false;
    }
    public static void Split(string title, bool horizontal, int size)
    {
        var sp = new SplitPanel(horizontal) { Id = title, Parent = Context.LastPanel };
        Context.LastPanel.Add(sp);
        Context.PanelStack.AddLast(sp);
        NextSplit(size);
    }

    public static void NextSplit(int size = -1)
    {
        if(!(Context.LastPanel is SplitPanel))
            Context.PanelStack.RemoveLast();
        var p = new Panel() { Id = Context.LastPanel.Title + "#" + (Context.LastPanel.Components.Count + 1), Border = false, Margin = new Vec2 { X = 1, Y = 1 }, Parent = Context.LastPanel };
        ((SplitPanel)Context.LastPanel).Split.Add(size);
        Context.LastPanel.Add(p);
        Context.PanelStack.AddLast(p);
    }

    public static void EndSplit()
    {
        Context.PanelStack.RemoveLast();
        Context.PanelStack.RemoveLast();
    }


}

[Flags]
public enum WindowFlags
{
    TopWindow = 1<<0,
}