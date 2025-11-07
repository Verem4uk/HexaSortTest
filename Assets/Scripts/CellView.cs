using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class CellView : MonoBehaviour
{            
    public Cell Cell { private set; get; }

    private Controller Controller;

    public void Initialize(Cell cell, Controller controller)
    {
        Cell = cell;
        Controller = controller;
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

    private void Update()
    {
        if (WasClicked() && IsPointerOverThisCell(this))
        {
            HandleClick();
        }
    }

    private bool WasClicked()
    {        
        if (Mouse.current?.leftButton.wasPressedThisFrame == true)
            return true;
                
        if (Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true)
            return true;

        return false;
    }

    public void HandleClick()
    {
        Debug.Log($"Clicked on cell {Cell}");
        if (Controller.IsHummerMode())
        {
            Cell.Stack.Delete();
            Controller.DisableHummerMode();
        }        
    }
}
