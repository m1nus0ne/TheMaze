namespace TheMaze;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        // var main = new Form1();
        // main.KeyDown += (sender, args) => ContorStateMachine.OnPress(args);
        // main.KeyUp += (sender, args) => ContorStateMachine.OnRelease(args);
        // var keyResponse = new Timer();
        // keyResponse.Interval = 50;
        // keyResponse.Tick += (sender, args) => main.Handler.Move(ContorStateMachine.GetState(),2f);
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }    
}