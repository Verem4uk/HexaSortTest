using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class HexStackView : MonoBehaviour
{
    private Stack<Hexon> logicStack;    
    private bool _isDragging;
    private Vector3 _offset;
    private Vector3 _startPosition;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void Initialize(Stack<Hexon> stack)
    {
        logicStack = stack;
    }

    private void Update()
    {    
        Vector2 pointerPos = GetPointerScreenPosition();

        if (GetPointerPressedThisFrame())
        {
            if (TryPickUnderCursor(pointerPos, out var hit) && hit.collider.gameObject == gameObject)
            {
                _isDragging = true;
                _startPosition = transform.position;
                _offset = transform.position - GetMouseWorldPos(pointerPos);
            }
        }

        if (_isDragging)
        {
            Vector3 target = GetMouseWorldPos(pointerPos) + _offset;
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 15f);

            if (GetPointerReleasedThisFrame() || !GetPointerIsPressed())
            {
                _isDragging = false;
                var collider = GetComponent<BoxCollider>();
                collider.enabled = false;

                var targetCell = GetHoveredCell(pointerPos);
                if (targetCell != null)
                {
                    targetCell.PlaceStack(this);
                }                    
                else
                {
                    collider.enabled = true;
                    transform.position = _startPosition;
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
        var ray = _mainCamera.ScreenPointToRay(screenPos);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        plane.Raycast(ray, out float enter);
        return ray.GetPoint(enter);
    }

    private bool TryPickUnderCursor(Vector2 screenPos, out RaycastHit hit)
    {
        var ray = _mainCamera.ScreenPointToRay(screenPos);
        return Physics.Raycast(ray, out hit);
    }

    private HexCellView GetHoveredCell(Vector2 screenPos)
    {        
        var ray = _mainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out var hit))
        {
            Debug.Log("Try get hovered Cell success"+hit.collider.gameObject.name);
            return hit.collider.GetComponent<HexCellView>();
        }
           
        Debug.Log("Try get hovered Cell fail");
        return null;
    }
}
