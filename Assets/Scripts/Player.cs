using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Player : MonoBehaviour
{
    public Lane _lane = Lane.Mid;
    public float NewXPos = 0f;
    [HideInInspector]
    public bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown;
    public float XValue;
    public float speed;

    private Animator animator;

    private float _runDistance = 50f;
    private float _deltaRunDistance;

    private CharacterController characterController;
    
    private float lerpX;
    public float SpeedDodge;
    private float jumpForce = 7f; 
    private float x;
    private float y;
    [SerializeField] bool inJump;
    [SerializeField] bool inRoll;
    private float colHeight;
    private float colCenterY;

    // move rollCounter here
    public HitX HitX = HitX.None;
    public HitY HitY = HitY.None;
    public HitZ HitZ = HitZ.None;


    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        colHeight = characterController.height;
        colCenterY = characterController.center.y;
        _deltaRunDistance = _runDistance;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Roll();
    }

    private void Move()
    {
        SwipeLeft = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
        SwipeRight = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
        if(SwipeLeft)
        {
            if(_lane == Lane.Mid)
            {
                NewXPos = -XValue;
                _lane = Lane.Left;
                // animator.SetTrigger("TurnLeft");
                animator.Play("Dodge Left");
            } else if(_lane == Lane.Right)
            {
                NewXPos = 0;
                _lane = Lane.Mid;
                animator.Play("Dodge Left");
            }
        } else if(SwipeRight)
        {
            if(_lane == Lane.Mid)
            {
                NewXPos = XValue;
                _lane = Lane.Right;
                // animator.SetTrigger("TurnRight");
                animator.Play("Dodge Right");
            } else if(_lane == Lane.Left)
            {
                NewXPos = 0;
                _lane = Lane.Mid;
                animator.Play("Dodge Right");
            }
        }
        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, speed * Time.deltaTime);
        x = Mathf.Lerp(x, NewXPos, Time.deltaTime * SpeedDodge);
        characterController.Move(moveVector);
    }

    private void Jump(){
        if(characterController.isGrounded) {
            SwipeUp = Input.GetKeyDown(KeyCode.Space);
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Falling Idle")) {
                animator.Play("Landing");
                inJump = false;
            }
            if(SwipeUp)
            {
                y = jumpForce;
                animator.CrossFadeInFixedTime("Jump", 0.1f);
                inJump = true;
            }
        } else {
            y -= jumpForce * 2 * Time.deltaTime;
            if(characterController.velocity.y < -0.1f) animator.Play("Falling Idle");
        }
        
    }

    // todo check ???
    internal float rollCounter;
    public void Roll() {
        rollCounter -= Time.deltaTime;
        if(rollCounter <= 0) {
            rollCounter = 0f;
            characterController.center = new Vector3(0, colCenterY, 0);
            characterController.height = colHeight;
            inRoll = false;
        }
        SwipeDown = Input.GetKeyDown(KeyCode.DownArrow);
        if(SwipeDown) {
            rollCounter = 0.5f;
            y -= 10f;
            characterController.center = new Vector3(0, colCenterY/2f, 0);
            characterController.height = colHeight/2f;
            animator.CrossFadeInFixedTime("Quick Roll To Run", 0.25f);
            inRoll = true;
            inJump = false;
        }
    }

    public void OnCharacterColliderHit(Collider col) {
        Debug.Log("OnCharacterColliderHit");
        HitX = GetHitX(col);
        HitY = GetHitY(col);
        HitZ = GetHitZ(col);
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
        } else if(other.TryGetComponent<GenerateRoadTrigger>(out GenerateRoadTrigger trig))
        {
            Vector3 genPos = new Vector3(0, 0, transform.position.z +  200);
            FindObjectOfType<RoadGenerator>().GenerateManually(genPos);
        }
    }
}
