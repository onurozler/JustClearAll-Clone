namespace Assets.Scripts.Game
{

    // Indicate is Game Loaded

    public class LoadedGame
    {
        public static bool isLoaded = true;
    }
    

    // Indicate Game State

    public enum GameState
    {
        PLAYING,
        PAUSING,
    }

    // Indicate Mission

    public enum Mission
    {
        SUCCESSFULL,
        FAIL
    }
}


