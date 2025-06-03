using System.Drawing;
using System.Runtime.InteropServices;

namespace ConGui.Util;
public class ConsoleHelper
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
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr CreateFile(
        string lpFileName,
        int dwDesiredAccess,
        int dwShareMode,
        IntPtr lpSecurityAttributes,
        int dwCreationDisposition,
        int dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetCurrentConsoleFont(
        IntPtr hConsoleOutput,
        bool bMaximumWindow,
        [Out][MarshalAs(UnmanagedType.LPStruct)] ConsoleFontInfo lpConsoleCurrentFont);

    [StructLayout(LayoutKind.Sequential)]
    internal class ConsoleFontInfo
    {
        internal int nFont;
        internal Coord dwFontSize;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct Coord
    {
        [FieldOffset(0)]
        internal short X;
        [FieldOffset(2)]
        internal short Y;
    }

    private const int GENERIC_READ = unchecked((int)0x80000000);
    private const int GENERIC_WRITE = 0x40000000;
    private const int FILE_SHARE_READ = 1;
    private const int FILE_SHARE_WRITE = 2;
    private const int INVALID_HANDLE_VALUE = -1;
    private const int OPEN_EXISTING = 3;

    public static void Begin()
    {
        if (OperatingSystem.IsWindows())
        {
            var handleOut = ConsoleHelper.GetStdHandle(ConsoleHelper.STD_OUTPUT_HANDLE);
            var handleIn = ConsoleHelper.GetStdHandle(ConsoleHelper.STD_INPUT_HANDLE);
            uint mode;
            ConsoleHelper.GetConsoleMode(handleIn, out mode);
            // out: 7 by default enable_processed_input | enable_line_input | enable_echo_input
            // in: ? by default
            ConsoleHelper.SetConsoleMode(handleIn, 8 | 128 | 512);
            ConsoleHelper.SetConsoleMode(handleOut, 1 | 2 | 4 | 8);
        }
    }
}