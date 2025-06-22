using ANSIConsole;
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
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConGui;

public static partial class Gui
{
    public const string IdSeperator = "/";
    private static Context Context { get; set; } = null!;
    public static Style Style { get; set; } = new Style();

    public static void CreateContext()
    {
        //if (!ANSIInitializer.Init(false)) ANSIInitializer.Enabled = false;

        Context = new Context();

        ConsoleHelper.Begin();
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Write(
            "\e[?1003h" + // MOUSE_TRACKING_ALL
            //"\u001B[?1005h" + // MOUSE_MODE_UTF8
            //"\u001B[?1002h" + // Cell Motion Mouse Tracking (only click)
            "\u001B[?1006h" + // MOUSE_MODE_SGR
            "\u001B[?1015h" + // MOUSE_MODE_URXVT                 
//            "\e[?25l" + // hide cursor
            ""
            );
    }

    public static LinkedList<double> FrameTimes = new();
    public static bool[] pressState = new bool[3];

    public static List<List<int>> MouseEventCodes = [
        [0, 8, 16, 24, 32, 36, 40, 48, 56],
        [1, 9, 17, 25, 33, 37, 41, 45, 49, 53, 57, 61],
        [2, 10, 14, 18, 22, 26, 30, 34, 42, 46, 50, 54, 58, 62]
    ];
    static int lastMouseEvent = 0;
    private static readonly Regex AnsiRegex = new Regex(@"\x1B\[([<>=?]?[0-9;]*[A-Za-z])", RegexOptions.Compiled);


    public static void BeginFrame()
    {
        //Context.PanelStack.Clear();
        Context.Windows.Clear();

        FrameTimes.AddLast(Environment.TickCount / 1000.0);
        while (FrameTimes.Count > 1000)
            FrameTimes.RemoveFirst();

//        while(!Console.KeyAvailable)
//           Thread.Sleep(1);

        //TODO: should this be in a seperate thread from the rendering thread?
        var result = "";
        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);
            result += key.KeyChar;
        }

        if(!string.IsNullOrEmpty(result))
        {
            var matches = AnsiRegex.Matches(result);
            KeyModifier modifier = KeyModifier.None;
            foreach (Match match in matches)
            {
                string code = match.Groups[1].Value;
                if (code.StartsWith("1;"))
                {
                    char key = code[2]; // 2 = shift, 3 = alt, 5 = ctrl
                    code = code[3..];

                    if (key == '2')
                        modifier |= KeyModifier.Shift;
                    else if (key == '3')
                        modifier |= KeyModifier.Alt;
                    else if (key == '5')
                        modifier |= KeyModifier.Ctrl;
                    else
                        throw new Exception("Unkown modifier: " + key);

                }
                if (code.StartsWith("<"))
                {
                    var pressed = code.EndsWith("M");
                    var released = code.EndsWith("m");
                    if (pressed || released)
                        code = code[0..^1];

                    var p = code[1..].Split(";");
                    Context.MousePos = new Vec2 { X = int.Parse(p[1]) - 1, Y = int.Parse(p[2]) - 1 };
                    int mouseEvent = int.Parse(p[0]);

                    //mouse movement
                    for (int i = 0; i < 3; i++)
                        if (MouseEventCodes[i].Contains(mouseEvent) && (pressed || released))
                        {
                            pressState[i] = pressed ? true : false;
                            lastMouseEvent = i;
                        }
                    if (mouseEvent == 35) //windows only???
                        pressState[lastMouseEvent] = pressed ? true : false;

                    if (mouseEvent == 64) // scroll up
                        Context.MouseScroll = -1;
                    else if (mouseEvent == 65) // scroll down
                        Context.MouseScroll = 1;
                }
                else if (code == ("A"))
                    Context.KeyButtonInput.Add((modifier, Key.Up));
                else if (code == ("B"))
                    Context.KeyButtonInput.Add((modifier, Key.Down));
                else if (code == ("C"))
                    Context.KeyButtonInput.Add((modifier, Key.Right));
                else if (code == ("D"))
                    Context.KeyButtonInput.Add((modifier, Key.Left));
                else if (code == ("F"))
                    Context.KeyButtonInput.Add((modifier, Key.End));
                else if (code == ("H"))
                    Context.KeyButtonInput.Add((modifier, Key.Home));
                else if (code == ("3~"))
                    Context.KeyButtonInput.Add((modifier, Key.Delete));

            }
            result = AnsiRegex.Replace(result, "");
        }

        if(result.Contains('\u001b'))
        {
            Context.KeyButtonInput.Add((KeyModifier.None, Key.Esc));
            Context.FocussedComponent = string.Empty; // clear focus on escape  
            result = result.Replace("\u001b", "");
            Console.Write("\e[?25l"); // hide cursor
        }
        if (result.Contains('\u007f'))
        {
            Context.KeyButtonInput.Add((KeyModifier.None, Key.Backspace));
            result = result.Replace("\u007f", "");
        }
        if (result.Contains('\u0001'))
        {
            Context.KeyButtonInput.Add((KeyModifier.Ctrl, Key.A));
            result = result.Replace("\u0001", "");
        }
        if (result.Contains('\t'))
        {
            Context.KeyButtonInput.Add((KeyModifier.None, Key.Tab));
            result = result.Replace("\t", "");
        }

        if (!string.IsNullOrEmpty(result))
        {
            foreach (var c in result)
            {
                if (c < 32)
                {
                    Debug.WriteLine("Removed character ID " + (int)c);
                    result.Replace(c + "", "");
                }
            }
            Context.KeyInput += result;
        }


        for (int i = 0; i < 3; i++)
        {
            if (pressState[i])
            {
                if (Context.MouseStates[i] == MouseState.JustPressed)
                    Context.MouseStates[i] = MouseState.Down;
                else if (Context.MouseStates[i] != MouseState.Down)
                    Context.MouseStates[i] = MouseState.JustPressed;
            }
            else //!pressed
            {
                if (Context.MouseStates[i] == MouseState.JustReleased)
                    Context.MouseStates[i] = MouseState.Up;
                else if (Context.MouseStates[i] != MouseState.Up)
                    Context.MouseStates[i] = MouseState.JustReleased;
            }
        }

        if (OperatingSystem.IsWindows())
        {
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth;
        }
    }



    public static void Render()
    {
        var frameBuffer = new FrameBuffer(Console.WindowWidth, Console.WindowHeight);

        foreach (var window in Context.Windows) //TODO: order by window order
            foreach(var drawCommand in window.DrawCommands)
                drawCommand.Draw(frameBuffer);


        //debug info
        new DrawTextCommand(Math.Round(FrameTimes.Count / (FrameTimes.Last!.Value - FrameTimes.First!.Value), 1) + " FPS", new Vec2 { X = 0, Y = Console.WindowHeight-1 }, new ElementProperties().SetBg(Color.Black).SetFg(Color.White)) { Id = "" }.Draw(frameBuffer);
        Context.HoveredComponent = (Context.MousePos.X < frameBuffer.Width && Context.MousePos.Y < frameBuffer.Height) ? frameBuffer.Elements[Context.MousePos.X, Context.MousePos.Y].ObjectId : "";
        string debugLine = Context.HoveredComponent + " ";
        for (int i = 0; i < 3; i++)
            debugLine += Context.MouseStates[i].ToString()[0];
        new DrawTextCommand(debugLine, new Vec2 { X = Console.WindowWidth - debugLine.Length - 1, Y = 0 }, new ElementProperties()) { Id = "" }.Draw(frameBuffer);
        new DrawTextCommand((Environment.TickCount % 1000 < 500) ? "▓" : "░", new Vec2 { X = Context.MousePos.X, Y = Context.MousePos.Y }, new ElementProperties()) { Id = "" }.Draw(frameBuffer);



        //if (Context.MouseFocus == null)
        {
            Console.Write("\e[?25l"); // hide cursor
            //Console.CursorVisible = false;
        }

        Console.Write("\x1b[s"); //save cursor
        frameBuffer.Draw();
        Console.Write("\x1b[u"); // restore cursor


        if (Context.MouseFocus != null)
        {
            //Console.SetCursorPosition(Context.MouseFocus.X, Context.MouseFocus.Y);
            Console.Write($"\e[{Context.MouseFocus.Y+1};{Context.MouseFocus.X+1}H");
            Console.Write("\e[?25h"); // show cursor

            //Console.CursorVisible = true;
        }


        Context.MouseScroll = 0;
        Context.KeyInput = string.Empty;
        Context.KeyButtonInput.Clear();
        Thread.Sleep(1);
    }


    public static void DestroyContext()
    {
        // we are C#, we don't need to dispose of things
    }


    public static void SameLine()
    {
        Context.NextFrameProperties.SameLine = true;
    }

    private static ComponentActivationState GetComponentActivationState(string? suffix = null)
    {
        var id = Context.CascadedStackFrame.Id;
        if(suffix != null)
            id += IdSeperator + suffix;
        var state = ComponentActivationState.Idle;
        if (Context.HoveredComponent == id)
        {
            state = ComponentActivationState.Hovered;
            if (Context.MouseStates[0] == MouseState.JustPressed)
                state = ComponentActivationState.Pressed;
            if (Context.MouseStates[0] == MouseState.Down)
                state = ComponentActivationState.Down;
            if (Context.MouseStates[0] == MouseState.JustReleased)
                state = ComponentActivationState.Released;
        }
        //TODO: add selected state
        return state;
    }


    private static bool HandleFocus()
    {
        if(Context.MouseStates[0] == MouseState.JustPressed)
        {
            if (Context.HoveredComponent == Context.CurrentId)
                Context.FocussedComponent = Context.CurrentId;
            else if(Context.FocussedComponent == Context.CurrentId)
                Context.FocussedComponent = string.Empty;
        }
        return Context.FocussedComponent == Context.CurrentId;
    }


    public static void PushId(string id) => Context.PushId(id);
    public static void PopId() => Context.PopId();

    public static void SetNextWidth(int nextWidth)
    {
        if(Context.NextFrameProperties.Size == null)
            Context.NextFrameProperties.Size = new Vec2 { X = nextWidth, Y = 0 };
        else
            Context.NextFrameProperties.Size = new Vec2 { X = nextWidth, Y = Context.NextFrameProperties.Size.Y };
    }
    public static void SetNextHeight(int nextHeight)
    {
        if (Context.NextFrameProperties.Size == null)
            Context.NextFrameProperties.Size = new Vec2 { X = 0, Y = nextHeight };
        else
            Context.NextFrameProperties.Size = new Vec2 { X = Context.NextFrameProperties.Size.X, Y = nextHeight };
    }

    public static void SetNextBackgroundColor(Color color) => Context.NextFrameProperties.BackgroundColor = color;
    public static void SetNextForegroundColor(Color color) => Context.NextFrameProperties.ForegroundColor = color;
    public static void SetNextTextColor(Color color) => Context.NextFrameProperties.TextColor = color;
    public static void SetNextCursor(Vec2 cursor) => Context.NextFrameProperties.Cursor = cursor;



    public static void SetNextBackgroundColorDefault(Color color) => Context.NextFrameProperties.BackgroundColor ??= color;
    public static void SetNextForegroundColorDefault(Color color) => Context.NextFrameProperties.ForegroundColor ??= color;
    public static void SetNextTextColorDefault(Color color) => Context.NextFrameProperties.TextColor ??= color;
    public static void SetNextCursorDefault(Vec2 cursor) => Context.NextFrameProperties.Cursor ??= cursor;
}

