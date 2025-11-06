using TMPro;
using UnityEngine;

public class Messanger : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Text;

    public void ShowMessage(string text)
    {
        Text.text = text;
    }
}
