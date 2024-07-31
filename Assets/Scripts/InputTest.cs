using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    private PlayerInputManager _controls;

    [SerializeField] private float minimumSwipeMagnitude;
    private Vector2 _swipeDirection;
    
    void Start()
    {
        _controls = new PlayerInputManager();
        _controls.Player.Enable();
        _controls.Player.Touch.canceled += ProcessTouchComplete;
        _controls.Player.Swipe.performed += ProcessSwipeDelta;
    }

    private void ProcessSwipeDelta(InputAction.CallbackContext context)
    {
        _swipeDirection = context.ReadValue<Vector2>();
    }

    private void ProcessTouchComplete(InputAction.CallbackContext context)
    {
        Debug.Log("Touch complete");
        if (Mathf.Abs(_swipeDirection.magnitude) < minimumSwipeMagnitude) return;
        Debug.Log("Swipe detected");

        var position = Vector3.zero;

        if (_swipeDirection.x > 0)
        {
            Debug.Log("Swiping Right");
            position.x = 1;
        }
        else if (_swipeDirection.x < 0)
        {
            Debug.Log("Swiping Left");
            position.x = -1;
        }
        else if (_swipeDirection.y > 0)
        {
            Debug.Log("Swiping Up");
            position.y = 1;
        }
        else if (_swipeDirection.y < 0)
        {
            Debug.Log("Swiping Down");
            position.y = -1;
        }

        transform.position = position;
    }
}
