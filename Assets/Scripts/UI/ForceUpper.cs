using UnityEngine;
using TMPro;

public class ForceUpper : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    void Start() {
        inputField.onValidateInput += delegate (string s, int i, char c) { return char.ToUpper(c); };
    }

}
