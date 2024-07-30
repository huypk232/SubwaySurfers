using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Transform target;
    private Vector3 offset;
    private float y;
    public float speedFollow = 5f;

    void Start()
    {
        offset = transform.position;    
    }

    void LateUpdate()
    {
        Vector3 followPos = target.position + offset;
        RaycastHit hit;
        if (Physics.Raycast(target.position, Vector3.down, out hit, 3f))
        {
            y = Mathf.Lerp(y, hit.point.y, Time.deltaTime * speedFollow);
        }
        else
        {
            y = Mathf.Lerp(y, target.position.y, Time.deltaTime * speedFollow);
        }      
        followPos.y = offset.y + y;
        transform.position = followPos;
    }
}
