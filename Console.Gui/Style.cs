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
    public static Color Back = ColorTranslator.FromHtml("#212529");
    public static Color Subtle = ColorTranslator.FromHtml("#1A1D20");
    public static Color Border = ColorTranslator.FromHtml("#343A3F");
    public static Color Text = ColorTranslator.FromHtml("#DEE2E6");

    public static Color Body = ColorTranslator.FromHtml("#212529");
    public static Color Secondary = ColorTranslator.FromHtml("#343A40");
    public static Color Tertiary = ColorTranslator.FromHtml("#2B3035");
    public static Color Front = ColorTranslator.FromHtml("#B5B7B8");

    public static Color Primary = ColorTranslator.FromHtml("#0D6EFD");
    public static Color PrimarySubtle = ColorTranslator.FromHtml("#031633");
    public static Color PrimaryBorder = ColorTranslator.FromHtml("#084298");
    public static Color PrimaryText = ColorTranslator.FromHtml("#6EA8FE");

    public static Color Success = ColorTranslator.FromHtml("#198754");
    public static Color SuccessSubtle = ColorTranslator.FromHtml("#051A10");
    public static Color SuccessBorder = ColorTranslator.FromHtml("#0F5132");
    public static Color SuccessText = ColorTranslator.FromHtml("#75B798");

    public static Color Danger = ColorTranslator.FromHtml("#DC3545");
    public static Color DangerSubtle = ColorTranslator.FromHtml("#2C0B0E");
    public static Color DangerBorder = ColorTranslator.FromHtml("#842029");
    public static Color DangerText = ColorTranslator.FromHtml("#EA868F");

    public static Color Warning = ColorTranslator.FromHtml("#FFC107");
    public static Color WarningSubtle = ColorTranslator.FromHtml("#332701");
    public static Color WarningBorder = ColorTranslator.FromHtml("#997404");
    public static Color WarningText = ColorTranslator.FromHtml("#FFDA6A");

    public static Color Info = ColorTranslator.FromHtml("#0DCAF0");
    public static Color InfoSubtle = ColorTranslator.FromHtml("#032830");
    public static Color InfoBorder = ColorTranslator.FromHtml("#087990");
    public static Color InfoText = ColorTranslator.FromHtml("#6EDFF6");

    public static Color Dark = ColorTranslator.FromHtml("#212529");


    public Color WindowBackground { get; set; } = Back;
    public Color WindowForeground { get; set; } = Front;

    public Color MenuBackground { get; set; } = PrimarySubtle;
    public Color MenuOpened { get; set; } = Primary;


    public Color ButtonBorder { get; set; } = Front;
    public Color ButtonCenter { get; set; } = Primary;
    public Color ButtonText { get; set; } = Front;
    public Color ButtonShadow { get; set; } = Color.Black;
    
    public Color InputBorder { get; set; } = WarningBorder;
    public Color InputCenter { get; set; } = Dark;
    public Color InputHovered { get; set; } = Back;

    public Color InputBorderSelected { get; set; } = Warning;
    public Color InputSelected { get; set; } = Secondary;
    public Color InputText { get; set; } = Front;

    public Color TabSelected { get; set; } = Primary;

    public Color ListOutline { get; set; } = Info;
    public Color ListBackground { get; set; } = Subtle;
    public Color ListScrollbar { get; set; } = InfoSubtle;
    public Color ListScrollbarTracker { get; set; } = InfoText;

    public Color ListSelectionText { get; set; } = Dark;
    public Color ListSelectionBackground { get; set; } = Info;

    public Color ComboBoxbackground { get; set; } = Info;
    public Color ComboBoxButtonBackground { get; set; } = Primary;
    public Color ComboBoxText { get; set; } = Dark;
    
    
    public Color CheckboxBackground { get; set; } = SuccessBorder;
    public Color CheckboxText { get; set; } = Front;


    
}
