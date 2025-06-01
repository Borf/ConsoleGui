using ConGui;
using System.Diagnostics;

Gui.CreateContext();

string text = "Hello";

while(true)
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
            Gui.Text("Hello World");
            Gui.Text("Second line");

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


                Gui.EndTab();
            }
            if (Gui.BeginTab("Browse"))
            {

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
