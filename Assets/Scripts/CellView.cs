using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class CellView : MonoBehaviour
{        
    public Cell Cell { private set; get; }

    public void Initialize(Cell cell)
    {
        Cell = cell;
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
