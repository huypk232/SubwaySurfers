using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subway : MonoBehaviour
{
    public GameObject[] roadPrefabs;
    public Transform spawnPos;



    // 2 solution, create some level models before generate subway or spawn each object after a each delta time
    // 1st solution:
    public float spawnBugDeltaTime = 0f;

    void Start()
    {
        InvokeRepeating("GenerateSubwayTracks", 0, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position += Vector3.back * 5 * Time.deltaTime;
    }

    public void GenerateSubwayTracks()
    {
        // Vector3 localizeSpawnPos = new Vector3(spawnPos.position.x, 0, spawnPos.position.z);
        // Instantiate(trackPrefab, localizeSpawnPos, Quaternion.identity, transform);
        // Instantiate(trackPrefab, localizeSpawnPos + Vector3.left * 3, Quaternion.identity, transform);
        // Instantiate(trackPrefab, localizeSpawnPos + Vector3.right * 3, Quaternion.identity, transform);
        int randomIndex = Random.Range(0, roadPrefabs.Length);
    }
}
