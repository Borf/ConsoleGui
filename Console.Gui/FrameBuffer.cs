using ANSIConsole;
using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;
public class FrameBuffer
{
    public FrameBuffer(int width, int height)
    {
        Width = width;
        Height = height;
        Elements = new Element[width, height];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                Elements[i, j] = new Element() {  Character = ' ' };
    }
    public int Width { get; set; }
    public int Height { get; set; }
    public Element[,] Elements { get; set; }
    public bool Darken { get; set; } = false;

    public Element this[int x, int y]
    {
        get =>  Elements [x, y];
        set => Elements[x, y] = value;
    }

    public Vec2 Cursor { get; set; } = new() { X = 0, Y = 0 };
    public bool NoOverrideSelf { get; set; } = false; // this thing is nasty

    public void Write(string? id, string text, ElementProperties properties)
    {
        if (Darken && properties.FgColor.HasValue)
            properties.FgColor = properties.FgColor.Value.Darker(0.2f);
        if (Darken && properties.BgColor.HasValue)
            properties.BgColor = properties.BgColor.Value.Darker(0.2f);
        if (Darken)
            id = null;

        var firstPos = Cursor;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != '\n')
            {
                if (Cursor.X < Width && Cursor.Y < Height)
                {
                    var el = Elements[Cursor.X, Cursor.Y];
                    if (!NoOverrideSelf || (!el.ObjectId.StartsWith(id??string.Empty) && !(id??string.Empty).StartsWith(el.ObjectId)))
                    {
                        el.Character = text[i];
                        el.Properties = properties;
                        if (id != null)
                            el.ObjectId = id;
                    }
                    Cursor += new Vec2() { X = 1, Y = 0 };
                }
            }
            else if (text[i] == '\n')
            {
                Cursor = new() { X = firstPos.X, Y = Cursor.Y + 1 };
            }
        }
    }

    public void Draw()
    {
        StringBuilder screenBuffer = new();

        var last = Elements[0, 0];
        StringBuilder sb = new();
        for (int y = 0; y < Height; y++)
        {
            var begin = 0;
            screenBuffer.Append($"\x1b[{y+1};{0}H");
            for (int x = 0; x < Width; x++)
            {
                var current = Elements[x, y];
                if (!last.Properties.Equals(current.Properties))
                {
                    ANSIString str = new ANSIString(sb.ToString());

                    if (last.Properties.FgColor.HasValue)
                        str = str.Color(last.Properties.FgColor.Value);
                    if (last.Properties.BgColor.HasValue)
                        str = str.Background(last.Properties.BgColor.Value);
                    if (last.Properties.UnderLine.HasValue && last.Properties.UnderLine.Value == true)
                        str = str.Underlined();
                    if (last.Properties.OverLine.HasValue && last.Properties.OverLine.Value == true)
                        str = str.Overlined();
                    screenBuffer.Append(str);
                    sb.Clear();
                    begin = x;
                }
                sb.Append(current.Character);
                last = current;
            }

            {
                ANSIString str = new ANSIString(sb.ToString());
                if (last.Properties.FgColor.HasValue)
                    str = str.Color(last.Properties.FgColor.Value);
                if (last.Properties.BgColor.HasValue)
                    str = str.Background(last.Properties.BgColor.Value);
                if (last.Properties.UnderLine.HasValue && last.Properties.UnderLine.Value == true)
                    str = str.Underlined();
                if (last.Properties.OverLine.HasValue && last.Properties.OverLine.Value == true)
                    str = str.Overlined();
                screenBuffer.Append(str);
                sb.Clear();
            }
        }
        //Console.SetCursorPosition(0, 0);
        //Console.Write("\x1b[0;0H");
        Console.Write(screenBuffer);
    }
}

public class Element
{
    public char Character { get; set; }
    public ElementProperties Properties { get; set; } = new();
    public string ObjectId { get; set; } = string.Empty;
}


public class ElementProperties
{
    public Color? BgColor { get; set; }
    public Color? FgColor { get; set; }
    public bool? UnderLine { get; set; }
    public bool? OverLine { get; set; }
    public bool? Bold { get; set; }
    public override bool Equals(object? obj)
    {//TODO: this costs performance apparently
        return obj is ElementProperties properties &&
               BgColor?.ToArgb() == properties?.BgColor?.ToArgb() &&
               FgColor?.ToArgb() == properties?.FgColor?.ToArgb() &&
               UnderLine == properties.UnderLine &&
               OverLine == properties.OverLine &&
               Bold == properties.Bold;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BgColor, FgColor, UnderLine, OverLine, Bold);
    }


    public ElementProperties SetBg(Color bgColor) { this.BgColor = bgColor; return this; }
    public ElementProperties SetFg(Color fgColor) { this.FgColor = fgColor; return this; }
    public ElementProperties SetUnderLine(bool value = true) { this.UnderLine = value; return this; }
    public ElementProperties SetOverLine(bool value = true) { this.OverLine = value; return this; }

}