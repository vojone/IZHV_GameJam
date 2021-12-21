using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Vector3 spawnPoint = new Vector3(2.0f, 1.0f, 1.0f);

    public GameObject mainTimeCheckpoint = null;

    public GameObject playerPrefab;

    public GameObject mainCamera;

    public bool gameLost = false;

    public bool gameStarted = false;

    private bool gamePaused = false;

    public static GameManager Instance;

    public GameObject UI;

    private List<GameObject> players = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        var player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);

        player.GetComponent<Player>().mainTimeCheckpoint = mainTimeCheckpoint;
        players.Add(player);

        mainCamera.GetComponent<CameraController>().SetPlayer(player);

        gameStarted = false;
        gamePaused = false;

        //WaitForUIManager
        while(!UI.GetComponent<UIManager>().initialized) {
        }

        TogglePause();
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
        if(GetPrimaryPlayer().GetComponent<Player>().HP <= 0.0f && gameStarted && !gameLost) {
            GameOver();
        }
    }

    public void Restart() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

        Start();
    }

    public void GameOver() {
        gameStarted = false;
        gameLost = true;

        Pause();

        //TODO
        Destroy(GetPrimaryPlayer());
        players.Clear();

        Restart();
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


    public GameObject GetPrimaryPlayer() {
        GameObject primary = null;

        if(players.Count > 0) {
            primary = players[0];
        }

        return primary;
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

    public void StartGame() {
        gameStarted = true;
        TogglePause();
    }

    public void OnExit() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBGL
            Application.ExternalEval("window.location.href=\"about:blank\";");
        #else
            Application.Quit();
        #endif
    }

    public void TogglePause() {
        if(gamePaused && gameStarted) {
            Resume();
        }
        else {
            Pause();
        }
    }

    void Pause() {
        Time.timeScale = 0;
        gamePaused = true;

        UI.GetComponent<UIManager>().setUIPage(0);
    }

    void Resume() {
        Time.timeScale = 1;
        gamePaused = false;

        UI.GetComponent<UIManager>().setUIPage(1);
    }
}
