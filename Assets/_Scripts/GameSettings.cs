// In GameSettings.cs
public static class GameSettings
{
    public enum AIDifficulty { Easy, Medium, Hard }
    public static int NumberOfPlayers { get; set; }
    public static bool IsVsComputer { get; set; }
    public static AIDifficulty Difficulty { get; set; }
    public static string PreviousScene { get; set; } // To remember where we came from
}
