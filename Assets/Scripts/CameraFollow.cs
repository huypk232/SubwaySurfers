using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    private Vector3 offset;
    private float y;
    public float SpeedFollow = 5f;

    void Start()
    {
        offset = transform.position;    
    }

    void LateUpdate()
    {
        Vector3 followPos = Target.position + offset;
        RaycastHit hit;
        if(Physics.Raycast(Target.position, Vector3.down, out hit, 2.5f))
            y = Mathf.Lerp(y, hit.point.y, Time.deltaTime * SpeedFollow);
        else y = Mathf.Lerp(y, Target.position.y, Time.deltaTime * SpeedFollow);      
        followPos.y = offset.y + y;
        transform.position = followPos;
    }
}
