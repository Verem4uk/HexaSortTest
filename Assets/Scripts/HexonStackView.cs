using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class HexonStackView : MonoBehaviour
{
    private HexonStack Stack;    
    private bool IsDragging;
    private Vector3 Offset;
    private Vector3 StartPosition;
    private Camera MainCamera;

    private Controller Controller;

    private void Start()
    {
        MainCamera = Camera.main;
    }

    public void Initialize(HexonStack stack, Controller controller)
    {
        Stack = stack;
        stack.Placed += OnPlaced;        
        Controller = controller;
    }

    public void OnPlaced(HexonStack hexon, Cell cell)
    {
        var newPosition = Controller.GetPositionForMove(cell);
        Stack.Placed -= OnPlaced;
        StartCoroutine(SmoothMove(transform, newPosition + Vector3.up * 0.5f, 0.15f));        
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

    private void Update()
    {    
        Vector2 pointerPos = GetPointerScreenPosition();

        if (GetPointerPressedThisFrame())
        {
            if (TryPickUnderCursor(pointerPos, out var hit) && hit.collider.gameObject == gameObject)
            {
                IsDragging = true;
                StartPosition = transform.position;
                Offset = transform.position - GetMouseWorldPos(pointerPos);
            }
        }

        if (IsDragging)
        {
            Vector3 target = GetMouseWorldPos(pointerPos) + Offset;
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 15f);

            if (GetPointerReleasedThisFrame() || !GetPointerIsPressed())
            {
                IsDragging = false;
                var collider = GetComponent<BoxCollider>();
                collider.enabled = false;

                var targetCell = GetHoveredCell(pointerPos);
                if (targetCell == null || targetCell.Cell.Type == Cell.CellType.Blocked || 
                    !Controller.Place(Stack, targetCell.Cell))
                {
                    collider.enabled = true;
                    transform.position = StartPosition;
                }                 
                                 
            }
        }
    }

    private static bool GetPointerIsPressed()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            return true;
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            return true;
        return false;
    }

    private static bool GetPointerPressedThisFrame()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            return true;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            return true;
        return false;
    }

    private static bool GetPointerReleasedThisFrame()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
            return true;
        if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame)
            return true;
        return false;
    }

    private static Vector2 GetPointerScreenPosition()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            return Touchscreen.current.primaryTouch.position.ReadValue();
        }
        
        if(Mouse.current != null)
        {
            return Mouse.current.position.ReadValue();
        }

        return Vector2.zero;         
    }

    private Vector3 GetMouseWorldPos(Vector2 screenPos)
    {
        var ray = MainCamera.ScreenPointToRay(screenPos);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        plane.Raycast(ray, out float enter);
        return ray.GetPoint(enter);
    }

    private bool TryPickUnderCursor(Vector2 screenPos, out RaycastHit hit)
    {
        var ray = MainCamera.ScreenPointToRay(screenPos);
        return Physics.Raycast(ray, out hit);
    }

    private CellView GetHoveredCell(Vector2 screenPos)
    {        
        var ray = MainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out var hit))
        {            
            return hit.collider.GetComponent<CellView>();
        }           
        
        return null;
    }
}
