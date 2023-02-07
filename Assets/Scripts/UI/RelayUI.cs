using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelayUI : MonoBehaviour
{
    [SerializeField] private RelayManager relayManger;
    [SerializeField] private Button createRelayButton;
    [SerializeField] private Button joinRelayButton;
    [SerializeField] private TMP_InputField inputField;

    private void Awake() {
        createRelayButton.onClick.AddListener(() => 
        {
            relayManger.CreateRelay();
        });
        joinRelayButton.onClick.AddListener(() =>
        {
            string code = inputField.text;
            if (code == null) {
                Debug.Log("Enter join code first!");
            }
            relayManger.JoinRelay(code);
        });
    }
}
