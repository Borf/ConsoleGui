using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConGui.Util;
public class ConsoleMode
{
    public const int STD_OUTPUT_HANDLE = -11;
    public const int STD_INPUT_HANDLE = -10;
    public const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    public static void Begin()
    {
        if (OperatingSystem.IsWindows())
        {
            var handleOut = ConsoleMode.GetStdHandle(ConsoleMode.STD_OUTPUT_HANDLE);
            var handleIn = ConsoleMode.GetStdHandle(ConsoleMode.STD_INPUT_HANDLE);
            uint mode;
            ConsoleMode.GetConsoleMode(handleIn, out mode);
            // out: 7 by default enable_processed_input | enable_line_input | enable_echo_input
            // in: ? by default
            ConsoleMode.SetConsoleMode(handleIn, 8 | 128 | 512);
            ConsoleMode.SetConsoleMode(handleOut, 1 | 2 | 4 | 8);
        }
    }
}