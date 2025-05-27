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

    public Element this[int x, int y]
    {
        get =>  Elements [x, y];
        set => Elements[x, y] = value;
    }

    public Vec2 Cursor { get; set; } = new() { X = 0, Y = 0 };

    public void Write(string? id, string text, ElementProperties properties)
    {
        var firstPos = Cursor;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] != '\n')
            {
                if (Cursor.X < Width && Cursor.Y < Height)
                {
                    var el = Elements[Cursor.X, Cursor.Y];
                    el.Character = text[i];
                    el.Properties = properties;
                    if (id != null)
                        el.ObjectId = id;
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
        Console.SetCursorPosition(0, 0);
        for(int y = 0; y < Height; y++)
        {
            var sb = new StringBuilder();
            var begin = 0;
            var last = Elements[0, y];
            for (int x = 0; x < Width; x++)
            {
                var current = Elements[x, y];
                if (!last.Properties.Equals(current.Properties) || x == Width-1)
                {
                    if (x == Width - 1)
                        sb.Append(current.Character);
                    Console.SetCursorPosition(begin, y);
                     
                    ANSIString str = new ANSIString(sb.ToString());
                    if (last.Properties.FgColor.HasValue)
                        str = str.Color(last.Properties.FgColor.Value);
                    if (last.Properties.BgColor.HasValue)
                        str = str.Background(last.Properties.BgColor.Value);
                    if (last.Properties.UnderLine.HasValue && last.Properties.UnderLine.Value == true)
                        str = str.Underlined();
                    if (last.Properties.OverLine.HasValue && last.Properties.OverLine.Value == true)
                        str = str.Overlined();
                    Console.Write(str);
                    sb.Clear();
                    begin = x;
                }
                if(x != Width-1)// if this is the last character, we already printed it so we can skip it
                    sb.Append(current.Character);
                last = current;
            }

        }
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
    {
        return obj is ElementProperties properties &&
               BgColor.Equals(properties.BgColor) &&
               FgColor.Equals(properties.FgColor) &&
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