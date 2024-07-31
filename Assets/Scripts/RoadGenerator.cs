using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject[] trackPrefab;
    
    [SerializeField] private Player player;
    private float trackOrder = 1;
    private const float FirstTotalRoads = 5;
    
    void Start()
    {
        for (int i = 0; i < FirstTotalRoads; i++)
        {
            GenerateSubwayTracks();
        }
    }

    public void GenerateSubwayTracks()
    {
        int pickIndex = Random.Range(0, trackPrefab.Length);
        Vector3 generatePos = new Vector3(0, 0, 100 * trackOrder);
        Instantiate(trackPrefab[pickIndex], generatePos, Quaternion.identity, transform);
        trackOrder++;
    }
}
