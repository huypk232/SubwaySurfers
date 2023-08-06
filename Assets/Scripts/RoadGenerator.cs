using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject[] trackPrefab;

    // 2 solution, create some level models before generate subway or spawn each object after a each delta time
    // 1st solution:
    private Player player;

    void Start()
    {
        GenerateSubwayTracks();
        InvokeRepeating("GenerateSubwayTracks", 10f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position += Vector3.back * 5 * Time.deltaTime;
    }

    public void GenerateSubwayTracks()
    {
        player = GameObject.FindObjectOfType<Player>();
        int pickIndex = Random.Range(0, trackPrefab.Length - 1);
        Vector3 genneratePos = new Vector3(0, 0, player.transform.position.z + 100);
        Instantiate(trackPrefab[pickIndex], genneratePos, Quaternion.identity);

    }

    public void GenerateManually(Vector3 position)
    {
        int pickIndex = Random.Range(0, trackPrefab.Length - 1);
        Debug.Log("" + pickIndex);
        Debug.Log(trackPrefab.Length);
        Instantiate(trackPrefab[pickIndex], position, Quaternion.identity);
    }
}
