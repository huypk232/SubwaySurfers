using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public GameObject[] roadPrefabs;
    public Transform spawnPos;
    [SerializeField] private float length = 50f;
    private Player player;


    // 2 solution, create some level models before generate subway or spawn each object after a each delta time
    // 1st solution:
    public float spawnBugDeltaTime = 0f;

    void Start()
    {
        player = FindObjectOfType<Player>();
        InvokeRepeating("GenerateSubwayTracks", 0, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z - transform.position.z >= 500)
        {
            Destroy(gameObject);
        }
    }
}
