using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donut : MonoBehaviour
{

    public Transform rotatePilar;

    void Update()
    {
        transform.RotateAround(rotatePilar.position, Vector3.up, 60 * Time.deltaTime);
    }
}
