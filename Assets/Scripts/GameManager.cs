using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Vector3 spawnPoint = new Vector3(2.0f, 1.0f, 1.0f);

    public GameObject mainTimeCheckpoint = null;

    public GameObject playerPrefab;

    public GameObject mainCamera;

    private bool gameLost = false;

    private bool gameStarted = false;

    public static GameManager Instance;

    private List<GameObject> players = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        var player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);

        player.GetComponent<Player>().mainTimeCheckpoint = mainTimeCheckpoint;
        players.Add(player);

        mainCamera.GetComponent<CameraController>().SetPlayer(player);

        gameStarted = true;
    }

    void Awake()
    {
        if(Instance == null) {
            //Debug.Log("Created Instance");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != this && Instance != null) {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver() {

    }

    public void SetSpawnPoint(Vector3 position) {
        spawnPoint = position;
    }

    public Vector3 GetSpawnPoint() {
        return spawnPoint;
    }

    public void FastCamMove() {
        mainCamera.GetComponent<CameraController>().setFastMode(true);
        mainCamera.GetComponent<CameraController>().Adjust(true);
        mainCamera.GetComponent<CameraController>().setFastMode(false);
    }


    /// Returns GO of closest player to given position (see IZHV 3th excercise)
    public GameObject NearestPlayer(Vector3 position, out float distance) {
        GameObject nearest = null;

        float closestDistance = float.MaxValue;

        foreach(var player in players) {
            var plPos = player.transform.position;
            var dist = (plPos - position).magnitude;
            
            if(closestDistance >= dist) {
                closestDistance = dist;
                nearest = player;
            }
        }

        distance = closestDistance;

        return nearest;
    }
}
