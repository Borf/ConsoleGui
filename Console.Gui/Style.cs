using ConGui.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConGui;
public class Style
{
    private static Color Back = ColorTranslator.FromHtml("#212529");
    private static Color Subtle = ColorTranslator.FromHtml("#1A1D20");
    private static Color Border = ColorTranslator.FromHtml("#343A3F");
    private static Color Text = ColorTranslator.FromHtml("#DEE2E6");

    private static Color Body = ColorTranslator.FromHtml("#212529");
    private static Color Secondary = ColorTranslator.FromHtml("#343A40");
    private static Color Tertiary = ColorTranslator.FromHtml("#2B3035");
    private static Color Front = ColorTranslator.FromHtml("#B5B7B8");
    
    private static Color Primary = ColorTranslator.FromHtml("#0D6EFD");
    private static Color PrimarySubtle = ColorTranslator.FromHtml("#031633");
    private static Color PrimaryBorder = ColorTranslator.FromHtml("#084298");
    private static Color PrimaryText = ColorTranslator.FromHtml("#6EA8FE");

    private static Color Success = ColorTranslator.FromHtml("#198754");
    private static Color SuccessSubtle = ColorTranslator.FromHtml("#051B11");
    private static Color SuccessBorder = ColorTranslator.FromHtml("#0F5132");
    private static Color SuccessText = ColorTranslator.FromHtml("#75B798");

    private static Color Danger = ColorTranslator.FromHtml("#DC3545");
    private static Color DangerSubtle = ColorTranslator.FromHtml("#2C0B0E");
    private static Color DangerBorder = ColorTranslator.FromHtml("#842029");
    private static Color DangerText = ColorTranslator.FromHtml("#EA868F");

    private static Color Warning = ColorTranslator.FromHtml("#FFC107");
    private static Color WarningSubtle = ColorTranslator.FromHtml("#332701");
    private static Color WarningBorder = ColorTranslator.FromHtml("#997404");
    private static Color WarningText = ColorTranslator.FromHtml("#FFDA6A");


    private static Color Info = ColorTranslator.FromHtml("#0DCAF0");
    private static Color InfoSubtle = ColorTranslator.FromHtml("#032830");
    private static Color InfoBorder = ColorTranslator.FromHtml("#087990");
    private static Color InfoText = ColorTranslator.FromHtml("#6EDFF6");

    private static Color Dark = ColorTranslator.FromHtml("#212529");


    public Color WindowBackground { get; set; } = Back;
    public Color WindowForeground { get; set; } = Front;

    public Color MenuBackground { get; set; } = PrimarySubtle;
    public Color MenuOpened { get; set; } = Primary;


    public Color ButtonBorder { get; set; } = Front;
    public Color ButtonCenter { get; set; } = Primary;
    public Color ButtonText { get; set; } = Front;
    public Color ButtonShadow { get; set; } = Color.Black;
    public Color InputBorder { get; set; } = Front;
    public Color InputCenter { get; set; } = Dark;
    public Color InputText { get; set; } = Front;

    public Color TabSelected { get; set; } = Primary;

    public Color ListOutline { get; set; } = Info;
    public Color ListBackground { get; set; } = Subtle;
    public Color ListScrollbar { get; set; } = InfoSubtle;
    public Color ListScrollbarTracker { get; set; } = InfoText;

    public Color ListSelectionText { get; set; } = Dark;
    public Color ListSelectionBackground { get; set; } = Info;

}
