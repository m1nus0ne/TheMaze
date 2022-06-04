using Timer = System.Windows.Forms.Timer;

namespace TheMaze;

public static class EventsHandler
{
    public static Timer InputTimer;
    public static Timer DisplayTimer;
    public static Timer ScoreTimer;
    private static MainForm _mainForm;


    public static void TimerInitialize(MainForm mainForm)
    {
        _mainForm = mainForm;
        InputTimer = new Timer {Interval = 5};
        DisplayTimer = new Timer {Interval = 5};
        ScoreTimer = new Timer() {Interval = 1000};
        ScoreTimer.Tick += (sender, args) => mainForm.Handler.Score += 1; 
        mainForm.KeyDown += (sender, args) => ContorStateMachine.OnPress(args);
        mainForm.KeyUp += (sender, args) => ContorStateMachine.OnRelease(args);
        
        InputTimer.Tick += (sender, args) =>
        {
            mainForm.Handler.UpdatePos(ContorStateMachine.GetState());
            if (mainForm.Handler.IsSolved)
            {
                ScoreTimer.Stop();
            }
        };
        DisplayTimer.Tick += (sender, args) => mainForm.Invalidate();
        DisplayTimer.Start();
        InputTimer.Start();
        ScoreTimer.Start();
    }

    public static void Restart()
    {
        Thread.Sleep(500);
        Application.Restart();
    }
    
}