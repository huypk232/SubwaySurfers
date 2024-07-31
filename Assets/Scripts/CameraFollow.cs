using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [FormerlySerializedAs("startIdlePosition")] [SerializeField] private Transform startIdleTransform;
    [FormerlySerializedAs("runningPosition")] [SerializeField] private Transform playGameTransform;
    private Vector3 offset;
    private float y;
    public float speedFollow = 5f;
    private GameManager gameManager;
    
    void Start()
    {
        offset = transform.position;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void LateUpdate()
    {
        if (gameManager.state == GameState.Ready)
        {
            transform.position = startIdleTransform.position;
            transform.rotation = startIdleTransform.rotation;
        } else if (gameManager.state == GameState.Play)
        {
            Vector3 followPos = target.position + offset;
            if (Physics.Raycast(target.position, Vector3.down, out RaycastHit hit, 3f))
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

    private void OnStartGame()
    {
        transform.position = playGameTransform.position;
        transform.rotation = playGameTransform.rotation;
    }
}
