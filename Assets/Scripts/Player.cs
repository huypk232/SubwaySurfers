using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using TouchPhase = UnityEngine.TouchPhase;

[System.Serializable]

public enum Lane {
    Left, Mid, Right
}

public enum HitX {
    Left, Mid, Right, None
}
public enum HitY {
    Top, Mid, Down, None
}
public enum HitZ {
    Forward, Mid, Backward, None
}

public enum SwipeDirection
{
    None, SwipeUp, SwipeDown, SwipeLeft, SwipeRight
}

public class Player : MonoBehaviour
{
    public Lane _lane = Lane.Mid;
    public float NewXPos = 0f;
    [HideInInspector]
    public bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown;
    [FormerlySerializedAs("XValue")] public float DistanceEachLaneX;


    private float _runDistance = 50f;
    private float _deltaRunDistance;
    
    [Header("Component")]
    private PlayerInputManager playerInput;
    // [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    
    private float lerpX;
    public float speedDodge;
    private float jumpForce = 7f; 
    private float x;
    private float y;
    [SerializeField] private bool inJump;
    [SerializeField] private bool inRoll;
    [SerializeField] private Vector3 velocity;
    private float colHeight;
    private float colCenterY;

    [SerializeField] private HitX hitX = HitX.None;
    [SerializeField] private HitY hitY = HitY.None;
    [SerializeField] private HitZ hitZ = HitZ.None;

    [Header("Mobile Movement")]
    private Vector2 swipeDirection;
    private InputAction inputAction;

    private Vector2 touchDownPosition;
    private Vector2 touchUpPosition;
    private float swipeResist = 0.5f;

    private void Awake()
    {
        // todo add input action later
        // inputAction.Enable();
        // inputAction.performed += context =>
        // {
        //     StartCoroutine(MoveCoroutine(context.ReadValue<Vector2>()));
        // };
        // SwipeDetection.instance.swipePerformed += context =>
        // {
        //     StartCoroutine(MoveCoroutine(context));
        // };
    }

    void Start()
    {
        playerInput = new PlayerInputManager();
        playerInput.Player.Enable();
        playerInput.Player.Touch.canceled += ProcessTouchComplete;
        playerInput.Player.Swipe.performed += ProcessSwipeDelta;
        
        colHeight = characterController.height;
        colCenterY = characterController.center.y;
        _deltaRunDistance = _runDistance;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.touch)
        // Jump();
        // Roll();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        Roll();
    }

    private void Move()
    {
        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, 5 * Time.deltaTime);
        x = Mathf.Lerp(x, NewXPos, Time.deltaTime * speedDodge);
        characterController.Move(moveVector);
    }
    
    private void ProcessSwipeDelta(InputAction.CallbackContext context)
    {
        swipeDirection = context.ReadValue<Vector2>();
    }

    private void ProcessTouchComplete(InputAction.CallbackContext context)
    {
        if (Mathf.Abs(swipeDirection.magnitude) < swipeResist) return;
        Debug.Log(swipeDirection);
        var position = Vector3.zero;
        if (Mathf.Abs(swipeDirection.x) >= Mathf.Abs(swipeDirection.y))
        {
            if (swipeDirection.x > 0)
            {
                if(_lane == Lane.Mid)
                {
                    NewXPos = DistanceEachLaneX;
                    _lane = Lane.Right;
                    animator.Play("Dodge Right");
                } else if(_lane == Lane.Left)
                {
                    NewXPos = 0;
                    _lane = Lane.Mid;
                    animator.Play("Dodge Right");
                }
            }
            else if (swipeDirection.x < 0)
            {
                if(_lane == Lane.Mid)
                {
                    NewXPos = -DistanceEachLaneX;
                    _lane = Lane.Left;
                    animator.Play("Dodge Left");
                } else if(_lane == Lane.Right)
                {
                    NewXPos = 0;
                    _lane = Lane.Mid;
                    animator.Play("Dodge Left");
                }
            }
        }
        else
        {
            if (swipeDirection.y > 0)
            {
                y = jumpForce;
                animator.CrossFadeInFixedTime("Jump", 0.1f);
                inJump = true;
            }
            else if (swipeDirection.y < 0)
            {
                rollCounter = 0.5f;
                y -= 10f;
                characterController.center = new Vector3(0, colCenterY/2f, 0);
                characterController.height = colHeight/2f;
                animator.CrossFadeInFixedTime("Quick Roll To Run", 0.25f);
                inRoll = true;
                inJump = false;
            }
        }
        transform.position = position;
    }

    private void Jump(){
        if(characterController.isGrounded) {
            SwipeUp = Input.GetKeyDown(KeyCode.Space) || Input.GetAxisRaw("Vertical") > 0;
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Falling Idle")) {
                animator.Play("Landing");
                inJump = false;
            }
        } else {
            y -= jumpForce * 2 * Time.deltaTime;
            if(characterController.velocity.y < -0.1f) animator.Play("Falling Idle");
        }
    }

    // todo check ???
    internal float rollCounter;
    private void Roll() {
        rollCounter -= Time.deltaTime;
        if(rollCounter <= 0) {
            rollCounter = 0f;
            characterController.center = new Vector3(0, colCenterY, 0);
            characterController.height = colHeight;
            inRoll = false;
        }
    }

    public void OnCharacterColliderHit(Collider col) {
        hitX = GetHitX(col);
        hitY = GetHitY(col);
        hitZ = GetHitZ(col);
    }

    public HitX GetHitX(Collider col) {
        Bounds charBounds = characterController.bounds;
        Bounds colBounds = col.bounds;
        float minX = Mathf.Max(colBounds.min.x, charBounds.min.x);
        float maxX = Mathf.Min(colBounds.max.x, charBounds.max.x);
        float average = (minX + maxX) / 2f - colBounds.min.x;
        HitX hit;
        if(average > colBounds.size.x - 0.33f)
            hit = HitX.Right;
        else if (average < 0.33f)
            hit = HitX.Left;
        else
            hit = HitX.Mid;
        return hit;
    }

    public HitY GetHitY(Collider col) {
        Bounds charBounds = characterController.bounds;
        Bounds colBounds = col.bounds;
        float minY = Mathf.Max(colBounds.min.y, charBounds.min.y);
        float maxY = Mathf.Min(colBounds.max.y, charBounds.max.y);
        float average = (minY + maxY) / 2f - colBounds.min.y;
        HitY hit;
        if(average > colBounds.size.y - 0.33f)
            hit = HitY.Top;
        else if (average < 0.33f)
            hit = HitY.Down;
        else
            hit = HitY.Mid;
        return hit;
    }

    public HitZ GetHitZ(Collider col) {
        Bounds charBounds = characterController.bounds;
        Bounds colBounds = col.bounds;
        float minZ = Mathf.Max(colBounds.min.z, charBounds.min.z);
        float maxZ = Mathf.Min(colBounds.max.z, charBounds.max.z);
        float average = (minZ + maxZ) / 2f - colBounds.min.z;
        HitZ hit;
        if(average > colBounds.size.z - 0.33f)
            hit = HitZ.Forward;
        else if (average < 0.33f)
            hit = HitZ.Backward;
        else
            hit = HitZ.Mid;
        return hit;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Train"))
        {
            Debug.Log("collision");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            // increase coins
        }
    }
}
