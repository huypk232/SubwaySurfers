using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public Player player;

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.name);
        // Debug.Log(collision.gameObject.tag);
        if (!collision.gameObject.CompareTag("Obstacle"))
            return;
        player.OnCharacterColliderHit(collision.collider);
    }
}
