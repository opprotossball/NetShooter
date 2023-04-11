using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuUI : MonoBehaviour
{
    [SerializeField] private RelayManager relayManager;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button howToButton;
    [SerializeField] private TMP_InputField gameIdInput;
    [SerializeField] private TMP_InputField nickField;

    private void Start() {
        gameIdInput.lineLimit = 1;
        gameIdInput.characterLimit = 10;
        quitButton.onClick.AddListener(() => Application.Quit());
        hostButton.onClick.AddListener(() => CreateGame());
        joinButton.onClick.AddListener(() => JoinGame());
    }

    private void CreateGame() {
        string nick = nickField.text;
        if (nick == null) {
            Debug.Log("Enter your nick!");
            return;
        }
        LocalPlayerInfo.nick = nick;
        SceneManager.LoadScene("MainScene");
        relayManager.CreateRelay();
    }

    private void JoinGame() {
        string code = gameIdInput.text;
        string nick = nickField.text;
        if (code == null || nick == null) {
            Debug.Log("Enter game id and your nick!");
        } else {
            LocalPlayerInfo.nick = nick;
            SceneManager.LoadScene("MainScene");
            relayManager.JoinRelay(code);
        }
    }

}
