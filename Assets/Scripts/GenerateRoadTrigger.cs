using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoadTrigger : MonoBehaviour
{
    private RoadGenerator roadGenerator;

    private void Start()
    {
        roadGenerator = FindObjectOfType<RoadGenerator>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Player player))
        {
            roadGenerator.GenerateSubwayTracks();
        }
    }
}
