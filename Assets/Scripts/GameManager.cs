using UnityEngine;
using System.Collections.Generic;

public enum GameState {
    ACTIVE,
    WAITING,
    OVER
}



public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private GameState gameState;
    private string localPlayerNick;
    [SerializeField] private List<Transform> startingPositions;
    private int playerCount = 0;


    public string GetLocalPlayerNick() {
        return localPlayerNick;
    }

    public GameState GetGameState() {
        return gameState;
    }

    private void Start() {
        gameState = GameState.WAITING;
    }

    public void RegisterPlayer(GameObject player) {
        int position = Random.Range(0, startingPositions.Count - 1);
        PlayerScript playerScript = player.GetComponent<PlayerScript>();
        Vector3 pos = startingPositions[position].position;
        player.GetComponent<Rigidbody2D>().position = pos;
        playerScript.Direction = pos.x > 0 ? -1 : 1;
        startingPositions.RemoveAt(position);
        playerCount++;
        if (playerCount == 2)
        {
            gameState = GameState.ACTIVE;
        }
    }

    public void RegisterDeath(PlayerScript player)
    {
        gameState = GameState.OVER;
        StartCoroutine(MainSceneUI.instance.ShowEnd(player.IsLocal()));
    }

    private void Awake()
    {
        instance = this;
    }

}
