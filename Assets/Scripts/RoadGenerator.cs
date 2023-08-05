using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject[] trackPrefab;
    // public GameObject wallPrefab;
    // public Transform spawnPos;

    // 2 solution, create some level models before generate subway or spawn each object after a each delta time
    // 1st solution:
    private Player player;

    void Start()
    {
        // InvokeRepeating("GenerateSubwayTracks", 0f, 9f);
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
        player = GameObject.FindObjectOfType<Player>();
        int pickIndex = Random.Range(0, trackPrefab.Length - 1);
        Vector3 genneratePos = new Vector3(0, 0, player.transform.position.z + 100);
        Instantiate(trackPrefab[pickIndex], genneratePos, Quaternion.identity);

    }

    public void GenerateManually(Vector3 position)
    {
        int pickIndex = Random.Range(0, trackPrefab.Length - 1);
        Instantiate(trackPrefab[pickIndex], position, Quaternion.identity);
    }
}
