using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;
public class Style
{
    private static Color Back = ColorTranslator.FromHtml("#2B3035");
    private static Color Front = ColorTranslator.FromHtml("#B5B7B8");
    private static Color Primary = ColorTranslator.FromHtml("#1B6EC2");
    private static Color Dark = ColorTranslator.FromHtml("#212529");


    public Color WindowBackground { get; set; } = Back;
    public Color WindowForeground { get; set; } = Front;
    
    public Color ButtonBorder { get; set; } = Front;
    public Color ButtonCenter { get; set; } = Primary;
    public Color ButtonText { get; set; } = Front;
    public Color ButtonShadow { get; set; } = Color.Black;
    public Color InputBorder { get; set; } = Front;
    public Color InputCenter { get; set; } = Dark;
    public Color InputText { get; set; } = Front;

    public Color TabSelected { get; set; } = Primary;

}
