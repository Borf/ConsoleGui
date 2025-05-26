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
        Context.CurrentWindow = null;
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
        FixLayout(Console.BufferWidth, Console.BufferHeight, null);


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
                renderComponent(cc, offset + c.Margin);
            }
        };


        foreach(var window in Context.Windows)
        {
            renderComponent(window, Vec2.Zero);
        }
        Context.DrawCommands.Add(new DrawTextCommand("",Math.Round(FrameTimes.Count / (FrameTimes.Last.Value - FrameTimes.First.Value),1) + " FPS", new Vec2 { X = 0, Y = 0 }, new ElementProperties().SetBg(Color.Black).SetFg(Color.White)));

        var frameBuffer = new FrameBuffer(Console.WindowWidth, Console.WindowHeight);
        foreach (var command in Context.DrawCommands)
        {
            command.Draw(frameBuffer);
        }

        frameBuffer.Draw();

        Console.SetCursorPosition(MouseX, MouseY);
        if(Environment.TickCount % 1000 < 500)
            Console.Write("▓");
        else
            Console.Write("░");



    }

    private static void FixLayout(int parentWidth, int parentHeight, Component? component)
    {
        if (component == null)
        {
            foreach (var w in Context.Windows)
            {
                w.CalculatedSize = new Vec2 { X = parentWidth - w.Pos.X, Y = parentHeight - w.Pos.Y };
            }
        }
    }

    public static void DestroyContext()
    {

    }


    public static void Begin(string title, WindowFlags flags = 0)
    {
        if (Context.CurrentWindow != null)
            throw new Exception("Can't begin when there's a window open already");

        //TODO: check if window already exists
        Context.CurrentWindow = new Window() { Title = title, Flags = flags, Id = title };
    }
    public static void End()
    {
        if(Context.CurrentWindow == null)
            throw new Exception("Can't end when there's no window open");

        Context.Windows.Add(Context.CurrentWindow);
        Context.CurrentWindow = null;
    }

    public static void Text(string text)
    {
        Context.CurrentWindow.Add(new Label(text, Context.CurrentWindow.Cursor));
        Context.CurrentWindow.Cursor += new Vec2 { X = 0, Y = 1 };
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
        Context.CurrentWindow.Add(new Label(label, Context.CurrentWindow.Cursor + new Vec2 { X = 0, Y = big ? 1 : 0 }));
        Context.CurrentWindow.Cursor += new Vec2 { X = 20, Y = 0 };


        var btn = new TextInput(ref value, Context.CurrentWindow.Cursor, big) { Id = label };
        btn.CalculatedSize = new Vec2 { X = 15, Y = btn.CalculatedSize.Y };
        Context.CurrentWindow.Add(btn);
        Context.CurrentWindow.Cursor = new Vec2 { X = 0, Y = Context.CurrentWindow.Cursor.Y + btn.CalculatedSize.Y };

        return false;
    }
    public static bool Button(string text, bool big)
    {
        var btn = new Button(text, Context.CurrentWindow.Cursor, big) { Id = text };
        Context.CurrentWindow.Add(btn);
        Context.CurrentWindow.Cursor += new Vec2 { X = 0, Y = btn.CalculatedSize.Y };
        return false;
    }



}

[Flags]
public enum WindowFlags
{
    TopWindow = 1<<0,
}