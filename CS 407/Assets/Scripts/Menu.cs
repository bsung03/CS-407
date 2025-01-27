﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
public class Menu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject characterMenu;
    public GameObject globalData;
    public AudioMixer audioMixer;
    public GameObject player;
    public GameObject melee;
    public GameObject gunner;
    public GameObject miniscene;
    public GameObject ExitDoor;
    public GameObject ED;
    public TextMeshProUGUI gold_text;
    public TextMeshProUGUI score_text;

    public TextMeshProUGUI timerText;
    private float startTime;
    public static List<int> Rooms = new List<int>();
    public static int exitRoom;
    public static int currRoomID, roomToLoad;
    public static int StageNum;
    public static bool initstart;

    void Start()
    {

        player = GameObject.FindWithTag("Player");
        startTime = Time.time;
        GameIsPaused = false;
        initstart = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)){
            LoadNextStage();
        }

        if(player == null)
        {
            //select melee player
            player = GameObject.FindWithTag("Player");

        }
        if (!GameIsPaused)
        {
            
            float t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2");

            if (timerText != null)
            {
                timerText.SetText(minutes + ":" + seconds);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
     
                    Pause();
   
            }
        }
        else
        {
            if (gold_text != null && score_text != null)
            {
                gold_text.SetText("Gold: " + player.GetComponent<PlayerController>().gold.ToString());
                score_text.SetText("Score: " + player.GetComponent<PlayerController>().score.ToString());
            }
        }
        if(player != null && player.GetComponent<PlayerController>().stats[0] <= 0)
        {
            Pause();
        }

    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void showCharacterSelect() {
        pauseMenuUI.SetActive(false);
        characterMenu.SetActive(true);
    }
    public void StartGame()
    {
        // before starting the game, randomize the order of the rooms grid
        randomizeRooms();
        GameIsPaused = false;
        Time.timeScale = 1f;
        
        System.Random random = new System.Random();
        exitRoom = random.Next(1, 10);
        // center room is the one stored at index 4
        currRoomID = Rooms[4];
        roomToLoad = currRoomID;
        SceneManager.LoadScene(roomToLoad);
        //SpawnExitDoor();

    }

    public void Tutorial()
    {
        SceneManager.LoadScene(10);
    }

    public void randomizeRooms()
    {
        // Add all room numbers and shuffle the list
        Rooms.Clear();
        for (int i = 1; i < 10; i++)
        {
            Rooms.Add(i);
        }
        Rooms.Shuffle();

        // The following printing is just for Testing
        Debug.Log("shuffled list:");
        foreach (int k in Rooms)
        {
            Debug.Log(k);
        }

        System.Random random = new System.Random();
        int r = random.Next(1, 8); // creates a number between 1 and 7 for the center room
        Debug.Log("random number: " + r);
        Debug.Log("random index: " + Rooms.IndexOf(r));

        Rooms[Rooms.IndexOf(r)] = Rooms[4];
        Rooms[4] = r;

        // The following printing is just for Testing
        Debug.Log("changing center room list:");
        foreach (int k in Rooms)
        {
            Debug.Log(k);
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void returnToMainMenu() {
        Destroy(player);
        SceneManager.LoadScene("Menu");
    }
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void meleeClick() {
        player = Instantiate(melee, new Vector3(0,0,-2), Quaternion.identity, null);
        StartGame();
        StageNum = 1;
    }
    public void gunnerClick() {
        player = Instantiate(gunner, new Vector3(0, 0, -2), Quaternion.identity, null);
        StartGame();
        StageNum = 1;
    }

    Vector3 GeneratedRandomPosition()
    {
        System.Random random = new System.Random();
        int min = -10;
        int max = 10;
        int x = random.Next(min, max);  
        int y = random.Next(min, max);  
        int z = -2; 
        return new Vector3(x,y,z);
    }

    public void SpawnExitDoor(){

        System.Random random = new System.Random();
        int r = random.Next(1, 10);
        string exitRoom = r.ToString();
        Instantiate(ExitDoor, GeneratedRandomPosition(), Quaternion.identity, null);
        SceneManager.MoveGameObjectToScene(ExitDoor,SceneManager.GetSceneByName("4"));

    }

    public void LoadNextStage(){
        //SceneManager.LoadScene("Loading");
        for(int i = 1; i <= 9; i++){
            bool v = SceneManager.UnloadScene(i.ToString());
        }
        StageNum++;
        initstart = false;
        StartGame();
    }
}
