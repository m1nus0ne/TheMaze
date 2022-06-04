namespace TheMaze;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        var mainForm = new MainForm();
        EventsHandler.TimerInitialize(mainForm);
        Application.Run(mainForm);
    }    
}