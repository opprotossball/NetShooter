using TMPro;
using UnityEngine;

public class LimitChars : MonoBehaviour
{   
    [SerializeField] private int limit;
    [SerializeField] private TMP_InputField inputField;
    private void Start() {
        inputField.lineLimit = 1;
        inputField.characterLimit = limit;
    }
}
