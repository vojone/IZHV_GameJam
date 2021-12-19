using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Vector3 spawnPoint = new Vector3(2.0f, 1.0f, 1.0f);

    private bool gameLost = false;

    private bool gameStarted = true;

    private List<GameObject> players = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetSpawnPoint() {
        return spawnPoint;
    }


    /// Returns GO of closest player to given position (see IZHV 3th excercise)
    public GameObject NearestPlayer(Vector3 position, out float distance) {
        GameObject nearest = null;

        float closestDistance = float.MaxValue;

        foreach(var player in players) {
            var plPos = player.transform.position;
            var dist = (plPos - position).magnitude;

            if(closestDistance >= dist) {
                dist = closestDistance;
                nearest = player;
            }
        }

        distance = closestDistance;

        return nearest;
    }
}
