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
    [SerializeField] private TMP_InputField inputField;

    void Start() {
        inputField.lineLimit = 1;
        inputField.characterLimit = 10;
        quitButton.onClick.AddListener(() => Application.Quit());
        hostButton.onClick.AddListener(() => 
        {
            SceneManager.LoadScene("MainScene");
            relayManager.CreateRelay();
        });
        joinButton.onClick.AddListener(() =>
        {
            string code = inputField.text;
            if (code == null) {
                Debug.Log("Enter game id first!");
            } else {
                SceneManager.LoadScene("MainScene");
                relayManager.JoinRelay(code);
            }
        });
    }

    void Update() {
        
    }
}
