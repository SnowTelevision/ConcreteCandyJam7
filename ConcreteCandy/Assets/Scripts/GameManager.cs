using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject startScreen;
    public GameObject endScreen;
    public Text scoreText;
    public Text endText;
    public Text scoreMulText;

    public Player player;
    public GameObject[] platforms;
    public bool gameOver;
    bool gameStarted;

    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Use this for initialization
    void Start () {
        gameOver = false;
        startScreen.SetActive(true);
        endScreen.SetActive(false);
        Time.timeScale = 0.0f;
        gameStarted = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        scoreMulText.text = "x" + platforms.Length;
        if (gameStarted && !gameOver)
        {
            scoreText.text = "Score: " + player.score;
            CheckEndGame();
        }
	}

    public void StartGame()
    {
        startScreen.SetActive(false);
        gameStarted = true;
        Time.timeScale = 1.0f;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }


    void ActivateEndGameScreen()
    {
        endScreen.SetActive(true);
    }

    void CheckEndGame()
    {
        platforms = GameObject.FindGameObjectsWithTag("Platform");

        if(platforms.Length == 1)
        {
            Destroy(platforms[0]);
            Destroy(player, 2.5f);
            gameOver = true;
            endText.text = "Candies Eaten: " + player.score;
            ActivateEndGameScreen();
        }
    }
}
