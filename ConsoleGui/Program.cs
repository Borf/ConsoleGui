using ConGui;
using System.Diagnostics;

Gui.CreateContext();

string text = "Hello";

while(true)
{
    Gui.BeginFrame();

    Gui.Begin("Main Window", WindowFlags.TopWindow);
    {
        Gui.Text("Hello World");

        Gui.Text("Second line");

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
        if (Gui.InputText("Position", false, ref text))
        {
            Debug.WriteLine("Text Changed!");
        }
    }
    Gui.End();


    Gui.Render();

    await Task.Delay(1);
}
