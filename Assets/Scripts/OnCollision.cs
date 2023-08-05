using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public Player player;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
            return;
        player.OnCharacterColliderHit(collision.collider);
    }
}
