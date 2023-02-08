using UnityEngine;

public enum GameState {
    ACTIVE,
    WAITING,
    OVER
}

public class GameManager : MonoBehaviour {
    private GameState gameState;
    private string localPlayerNick;

    public string GetLocalPlayerNick() {
        return localPlayerNick;
    }

    public GameState GetGameState() {
        return gameState;
    }
    private void Start() {
        gameState = GameState.ACTIVE;
    }
    private void RegisterPlayer() {

    }
}
