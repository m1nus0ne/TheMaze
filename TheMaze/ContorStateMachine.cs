using System.Numerics;
using TheMaze;


public static class ContorStateMachine
{
    private static Vector2 _direction;
    private static float _deltaAnlge;


    public static void OnPress(KeyEventArgs e)
    {
        switch (e.KeyValue)
        {
            case (int) Keys.A:
                _direction.X = 1;
                break;
            case (int) Keys.D:
                _direction.X = -1;
                break;
            case (int) Keys.S:
                _direction.Y = -1;
                break;
            case (int) Keys.W:
                _direction.Y = 1;
                break;
            case (int) Keys.E:
                _deltaAnlge = 1;
                break;
            case (int) Keys.Q:
                _deltaAnlge = -1;
                break;
        }
        if (e.KeyValue == (int) Keys.Space) CFG.MoveStep = CFG.RunStep;
    }

    public static void OnRelease(KeyEventArgs e)
    {
        switch (e.KeyValue)
        {
            case (int) Keys.A:
                _direction.X = 0;
                break;
            case (int) Keys.D:
                _direction.X = 0;
                break;
            case (int) Keys.S:
                _direction.Y = 0;
                break;
            case (int) Keys.W:
                _direction.Y = 0;
                break;
            case (int) Keys.E:
                _deltaAnlge = 0;
                break;
            case (int) Keys.Q:
                _deltaAnlge = 0;
                break;
            
        }

        if (e.KeyValue == (int) Keys.Space) CFG.MoveStep = CFG.WalkStep;
        if (e.KeyValue == (int) Keys.R) EventsHandler.Restart();
        
    }
    
    
    public static (Vector2, float) GetState() => (_direction, _deltaAnlge);
}