using System.Collections;
using UnityEngine;

public class HexonView : MonoBehaviour
{
    private Hexon Hexon;
    private Controller Controller;
    public void Initialize(Hexon hexon, Controller controller)
    {
        Hexon = hexon;
        Hexon.StackChanged += MoveTo;
        Hexon.Sold += OnSold;
        Controller = controller;
    }

    public void MoveTo(HexonStack newStack)
    {
        var newPosition = Controller.GetPositionForMove(newStack);
        StartCoroutine(SmoothMove(newPosition, 0.15f));
    }

    private IEnumerator SmoothMove(Vector3 target, float duration)
    {        
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(transform.position, target, t);
            yield return null;
        }
    }

    private void OnSold()
    {
        Hexon.Sold -= OnSold;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Hexon.StackChanged -= MoveTo;
    }
}

