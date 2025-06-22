using ConGui;
using ConGui.Util;
using System.ComponentModel;
using System.Diagnostics;

Gui.CreateContext();

string text = "Hello";
string[] comboValues = ["Value 1", "Value 2", "Value 3", "Value 4", "Value 5"];
string comboValue = "Click me!";


while (true)
{
    Gui.BeginFrame();

    Gui.Begin("#main", WindowFlags.TopWindow | WindowFlags.HasMenu);
    {
        Gui.BeginMenuBar();
        if(Gui.BeginMenu("File"))
        {
            if (Gui.MenuItem("Open"))
                Debug.WriteLine("Open clicked");
            if (Gui.MenuItem("Save"))
                Debug.WriteLine("Save clicked");
            if (Gui.MenuItem("Quit"))
                Environment.Exit(0);
            Gui.EndMenu();
        }

        if (Gui.BeginMenu("View"))
        {
            Gui.EndMenu();
        }

        if (Gui.BeginMenu("Options"))
        {
            Gui.EndMenu();
        }
        Gui.EndMenuBar();


        Gui.Split("Split", true, 30);
        {
            Gui.BeginList("Tables");
            {
                for (int i = 0; i < 100; i++)
                {
                    if (Gui.ListEntry("Entry " + i, ListChangedEvent.OnSelect))
                    {

                    }
                }
            }
            Gui.EndList();
        }
        Gui.NextSplit();
        {
            Gui.BeginTabPanel("Tabs");

            if (Gui.BeginTab("Structure"))
            {
                if (Gui.Button("A button", true))
                {
                    Debug.WriteLine("Button pressed");
                }

                if (Gui.Button("A small button", false))
                {
                    Debug.WriteLine("Button pressed");
                }

                if (Gui.InputText("Position", true, ref text))
                {
                    Debug.WriteLine("Text Changed!");
                }
                if (Gui.InputText("Position 2", false, ref text))
                {
                    Debug.WriteLine("Text Changed!");
                }

                if(Gui.BeginComboBox("Combo!", comboValue))
                {
                    foreach(var entry in comboValues)
                        if (Gui.ComboBoxEntry(entry, entry == comboValue))
                            comboValue = entry;
                    Gui.EndComboBox();
                }


                Gui.EndTab();
            }
            if (Gui.BeginTab("Browse"))
            {
                Gui.Text("Hello World");
                Gui.Text("Second line");

                Gui.EndTab();
            }
            if (Gui.BeginTab("Search"))
            {

                Gui.EndTab();
            }



            Gui.EndTabPanel();



        }
        Gui.EndSplit();
    }
    Gui.End();


    Gui.Render();

    await Task.Delay(1);
}
