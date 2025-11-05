using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class CellView : MonoBehaviour
{        
    public Cell Cell { private set; get; }

    public void Initialize(Cell cell)
    {
        Cell = cell;
    }
   
    public bool PlaceStack(HexonStackView stack)
    {
        if (Cell.IsOccupied)
        {            
            return false;
        }            
                    
        StartCoroutine(SmoothMove(stack.transform, transform.position + Vector3.up * 0.5f, 0.15f));
        return true;
    }

    private IEnumerator SmoothMove(Transform obj, Vector3 target, float duration)
    {
        Vector3 start = obj.position;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            obj.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }

    public static Vector2 GetPointerScreenPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            return Touchscreen.current.primaryTouch.position.ReadValue();
        }
        if (Mouse.current != null)
        {
            return Mouse.current.position.ReadValue();
        }

        return Vector2.zero;        
    }

    public static bool IsPointerOverThisCell(CellView cell)
    {        
        var ray = Camera.main.ScreenPointToRay(GetPointerScreenPosition());
        if (Physics.Raycast(ray, out var hit))
        {
            return hit.collider != null && hit.collider.gameObject == cell.gameObject;
        }

        return false;
    }   
}
