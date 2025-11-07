using System.Collections;
using UnityEngine;

public class HexonView : MonoBehaviour
{
    private Hexon Hexon;
    private Controller Controller;
    private HexonStackView StackView;

    public void Initialize(Hexon hexon, Controller controller)
    {
        Hexon = hexon;
        Hexon.StackChanged += MoveTo;
        Hexon.Sold += OnSold;
        Controller = controller;
    }

    public void MoveTo(HexonStack newStack)
    {
        StackView = Controller.GetStackViewForMove(newStack);
        var hexonsCount = newStack.PeekAll().Length;
        var cellPosition = Controller.GetPositionForMove(newStack.Cell);
        transform.parent = StackView.transform;
        var newPosition = cellPosition + Vector3.up * (hexonsCount * 0.3f);
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
        StartCoroutine(SmoothShrinkAndDestroy(0.2f)); 
    }

    private IEnumerator SmoothShrinkAndDestroy(float duration)
    {
        float t = 0f;
        Vector3 startScale = transform.localScale;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Hexon.StackChanged -= MoveTo;
    }
}

