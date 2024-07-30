using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;
using UnityEngine.Serialization;

public class SwipeDetection : MonoBehaviour
{
    public static SwipeDetection instance;
    public delegate void Swipe(Vector2 direction);
    public event Swipe swipePerformed;
    [SerializeField] private InputAction position, press;
    [SerializeField] private float swipeResistance = 100;
    private Vector2 initialPosition;
    private Vector2 currentPosition => position.ReadValue<Vector2>();
    
    private void Awake()
    {
        position.Enable();
        press.Enable();
        press.performed += _ => { initialPosition = currentPosition; };
        press.canceled += _ => DetectSwipe();
        instance = this;
    }

    private void DetectSwipe()
    {
        Vector2 delta = currentPosition - initialPosition;
        Vector2 direction = Vector2.zero;
        if (Mathf.Abs(delta.x) > swipeResistance)
        {
            direction.x = Mathf.Clamp(delta.x, -1, 1);
        }
        if (Mathf.Abs(delta.y) > swipeResistance)
        {
            direction.y = Mathf.Clamp(delta.y, -1, 1);
        }

        if (direction != Vector2.zero & swipePerformed != null)
        {
            swipePerformed(direction);
        }
    }
}
