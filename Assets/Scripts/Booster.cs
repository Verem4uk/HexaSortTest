using TMPro;
using UnityEngine;
using UnityEngine.Events; 

public class Booster : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Text;

    [SerializeField] 
    private int Count = 1;

    [SerializeField] 
    private UnityEvent OnUse;

    private void Start()
    {
        Text.text = Count.ToString();
    }

    public void Use()
    {
        if (Count < 1)
        {
            return;
        }                   

        Count--;
        Text.text = Count.ToString();
        OnUse?.Invoke();
    }
}
