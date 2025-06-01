using ConGui;
using System.Diagnostics;

Gui.CreateContext();

string text = "Hello";

while(true)
{
    Gui.BeginFrame();

    Gui.Begin("", WindowFlags.TopWindow | WindowFlags.HasMenu);
    {
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
