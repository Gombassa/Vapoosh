// In GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private DiceController diceController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AIController aiController;

    public BoardManager Board => boardManager;
    public enum GameState { WaitingForRoll, WaitingForMove, AITurn, GameOver }
    public GameState currentState;

    [SerializeField] private int numberOfPlayers = 2;
    private bool[] isPlayerAI;
    private int currentPlayerID;
    private int playerToSkip = -1;
    private bool getsAnotherGo = false;
    private Vector2Int coordinateOption1;
    private Vector2Int coordinateOption2;

    void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }
    
    void OnEnable() { SceneManager.sceneUnloaded += OnSceneUnloaded; }
    void OnDisable() { SceneManager.sceneUnloaded -= OnSceneUnloaded; }

    void Start()
    {
        Time.timeScale = 1f;
        int playersToStart = GameSettings.NumberOfPlayers;
        bool vsComputer = GameSettings.IsVsComputer;
        if (playersToStart == 0) { playersToStart = numberOfPlayers; }
        if (playersToStart < 2) { playersToStart = 2; }
        InitializeGame(playersToStart, vsComputer);
    }

    public void InitializeGame(int players, bool vsComputer)
    {
        numberOfPlayers = players;
        isPlayerAI = new bool[numberOfPlayers];
        if (vsComputer && players >= 2) { isPlayerAI[1] = true; }
        boardManager.CreateBoard();
        currentPlayerID = 1;
        playerToSkip = -1;
        StartTurn();
    }

    private void StartTurn()
    {
        getsAnotherGo = false;
        if (playerToSkip == currentPlayerID)
        {
            uiManager.UpdateStatusText($"Player {currentPlayerID} misses a turn!", 0);
            playerToSkip = -1;
            NextTurn();
            return;
        }
        uiManager.UpdateStatusText($"Player {currentPlayerID}'s Turn\nRoll the Dice!", currentPlayerID);
        uiManager.HideDiceDisplay();
        currentState = GameState.WaitingForRoll;
        if (isPlayerAI[currentPlayerID - 1])
        {
            currentState = GameState.AITurn;
            OnRollDiceButtonPressed();
        }
    }
    
    public bool IsCurrentPlayerAI()
    {
        if (isPlayerAI == null || isPlayerAI.Length == 0) return false;
        return isPlayerAI[currentPlayerID - 1];
    }

    public void OnRollDiceButtonPressed()
    {
        if (currentState != GameState.WaitingForRoll && currentState != GameState.AITurn) return;
        if (diceController.RollDice())
        {
            uiManager.UpdateStatusText($"VAPOOSH!\nPlayer {currentPlayerID} wins instantly!", currentPlayerID);
            uiManager.ShowVapooshEffect();
            uiManager.HideDiceDisplay();
            currentState = GameState.GameOver;
            return;
        }

        coordinateOption1 = diceController.GetCoordinateOption1();
        coordinateOption2 = diceController.GetCoordinateOption2();
        
        uiManager.UpdateDiceDisplay(diceController.PinkDie1, diceController.PinkDie2, diceController.GreenDie1, diceController.GreenDie2);
        
        string statusMessage = $"Player {currentPlayerID}: Place Your Counter";

        getsAnotherGo = (coordinateOption1.x == coordinateOption1.y);
        if (getsAnotherGo)
        {
            statusMessage += "\nSquare Roll! You get another go!";
        }
        uiManager.UpdateStatusText(statusMessage, currentPlayerID);

        int state1 = boardManager.GetTileState(coordinateOption1.x - 1, coordinateOption1.y - 1);
        int state2 = boardManager.GetTileState(coordinateOption2.x - 1, coordinateOption2.y - 1);
        if (state1 == currentPlayerID && state2 == currentPlayerID)
        {
            int nextPlayer = (currentPlayerID % numberOfPlayers) + 1;
            uiManager.UpdateStatusText($"Both squares occupied.\nPlayer {nextPlayer} will miss a turn.", 0);
            playerToSkip = nextPlayer;
            NextTurn();
            return;
        }
        if (isPlayerAI[currentPlayerID - 1])
        {
            aiController.TakeTurn(coordinateOption1, coordinateOption2, state1, state2, currentPlayerID);
        }
        else
        {
            currentState = GameState.WaitingForMove;
        }
    }

    public void ProcessPlayerMove(int x, int y)
    {
        if (currentState != GameState.WaitingForMove && currentState != GameState.AITurn) return;
        Vector2Int chosenIndex = new Vector2Int(x, y);
        Vector2Int validIndex1 = new Vector2Int(coordinateOption1.x - 1, coordinateOption1.y - 1);
        Vector2Int validIndex2 = new Vector2Int(coordinateOption2.x - 1, coordinateOption2.y - 1);
        if (chosenIndex == validIndex1 || chosenIndex == validIndex2)
        {
            boardManager.PlaceCounter(x, y, currentPlayerID);
            if (CheckForWin(x, y, currentPlayerID))
            {
                uiManager.UpdateStatusText($"Player {currentPlayerID} wins!", currentPlayerID);
                AudioManager.Instance.PlaySound(AudioManager.Instance.winnerSound);
                uiManager.HideDiceDisplay();
                currentState = GameState.GameOver;
            }
            else
            {
                if (getsAnotherGo)
                {
                    StartTurn();
                }
                else
                {
                    NextTurn();
                }
            }
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.wrongTileSound);
            uiManager.UpdateStatusText($"Wrong Tile! Try Again!", currentPlayerID);
        }
    }
    
    private void NextTurn()
    {
        currentPlayerID = (currentPlayerID % numberOfPlayers) + 1;
        StartTurn();
    }
    
    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == "RulesScene")
        {
            Time.timeScale = 1f;
            uiManager.ToggleGameUI(true);
        }
    }

    public bool TestMoveForWin(int x, int y, int playerID)
    {
        int originalState = boardManager.GetTileState(x, y);
        boardManager.Test_SetState(x, y, playerID);
        bool isWin = CheckForWin(x, y, playerID);
        boardManager.Test_SetState(x, y, originalState);
        return isWin;
    }
    
    public bool TestMoveForThreat(int x, int y, int playerID)
    {
        int originalState = boardManager.GetTileState(x,y);
        boardManager.Test_SetState(x, y, playerID);
        bool isThreat = CheckForThreat(x, y, playerID);
        boardManager.Test_SetState(x, y, originalState);
        return isThreat;
    }

    private bool CheckForWin(int x, int y, int playerID)
    {
        for (int i = 0; i < 4; i++) { if (CountMatchesInDirection(x - i, y, 1, 0, playerID) >= 4) return true; }
        for (int i = 0; i < 4; i++) { if (CountMatchesInDirection(x, y - i, 0, 1, playerID) >= 4) return true; }
        for (int i = 0; i < 4; i++) { if (CountMatchesInDirection(x - i, y - i, 1, 1, playerID) >= 4) return true; }
        for (int i = 0; i < 4; i++) { if (CountMatchesInDirection(x - i, y + i, 1, -1, playerID) >= 4) return true; }
        for (int i = 0; i < 2; i++) { for (int j = 0; j < 2; j++) { if(IsSquareAt(x-i, y-j, playerID)) return true; } }
        return false;
    }

    private bool CheckForThreat(int x, int y, int playerID)
    {
        for (int i = 0; i < 4; i++) { if (CountMatchesInDirection(x - i, y, 1, 0, playerID) == 3) return true; }
        for (int i = 0; i < 4; i++) { if (CountMatchesInDirection(x, y - i, 0, 1, playerID) == 3) return true; }
        for (int i = 0; i < 4; i++) { if (CountMatchesInDirection(x - i, y - i, 1, 1, playerID) == 3) return true; }
        for (int i = 0; i < 4; i++) { if (CountMatchesInDirection(x - i, y + i, 1, -1, playerID) == 3) return true; }
        return false;
    }

    private int CountMatchesInDirection(int startX, int startY, int dx, int dy, int playerID)
    {
        int count = 0;
        for (int i = 0; i < 4; i++) { if (boardManager.GetTileState(startX + i * dx, startY + i * dy) == playerID) count++; }
        return count;
    }

    private bool IsSquareAt(int startX, int startY, int playerID)
    {
        for (int i = 0; i < 2; i++) { for (int j = 0; j < 2; j++) { if (boardManager.GetTileState(startX + i, startY + j) != playerID) return false; } }
        return true;
    }
}