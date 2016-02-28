//Created: Jan 25, 2016

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameDifficulty
{
    public int minAsteroids;
    public int maxAsteroids;
    public float alienSpan;
    /// <summary>
    /// the possibility of spawning stronger small alien ship
    /// </summary>
    public float alienTwoWeight;

    public GameDifficulty() { }

    public GameDifficulty(int minAsteroids, int maxAsteroids, float alienSpan, float alienTwoWeight)
    {
        this.minAsteroids = minAsteroids;
        this.maxAsteroids = maxAsteroids;
        this.alienSpan = alienSpan;
        this.alienTwoWeight = alienTwoWeight;
    }

}

public class GameplayLogic : MonoBehaviour {

    public GameObject asteroidsSpawnPrefab;

    public GameObject playerPrefab;
    public Vector3 playerStart;
    public Vector3 playerStartRotation;
    public GameBoundary2D boundary;

    public GUIText scoreText;
    public GUIText livesText;
    public GUIText gameOverText;
    public Canvas gameOverCanvas;
    public UnityEngine.UI.InputField gameOverNameInput;


    public int score = 0;
    public int lives = 3;
    

    public List<GameDifficulty> difficulties;

    public GameObject largeAlienPrefab;
    public GameObject smallAlienPrefab;

    public GameObject mainCamera;
    public GameObject playerCamera;

    private GameObject playerShip;
    private int currentDifficultyLevel;
    private bool isGameOver;
    private float alienTimerRemaining;

    // Use this for initialization
    void Start () {
        currentDifficultyLevel = -1;
        isGameOver = false;
        // temporary hard coded difficulties - aka "levels"
        addDifficulties();

        spawnPlayer();
        spawnAsteroids();
        if (gameOverCanvas)
            gameOverCanvas.enabled = false;
        if (gameOverText)
            gameOverText.enabled = false;

       
    }

    private void addDifficulties()
    {
        this.difficulties.Clear();
        // hard coded temporarily
        this.difficulties.Add(new GameDifficulty(1, 1, -1, 0));
        this.difficulties.Add(new GameDifficulty(4, 4, 15, 0.1f));
        this.difficulties.Add(new GameDifficulty(6, 8, 12, 0.3f));
        this.difficulties.Add(new GameDifficulty(10, 12, 8f, 0.6f));
        this.difficulties.Add(new GameDifficulty(15, 20, 6f, 0.6f));
        this.difficulties.Add(new GameDifficulty(15, 25, 4.0f, 0.8f));
        this.difficulties.Add(new GameDifficulty(30, 40, 2.0f, 1f));
    }

    void FixedUpdate()
    {
        if (isGameOver)
        {
            if (Input.GetButtonUp("Submit"))
            {
                SubmitScore();
            }
        }

        if (Input.GetButton("Fire2"))
        {
            mainCamera.GetComponent<Camera>().enabled = false;
            if (playerCamera)
            playerCamera.GetComponent<Camera>().enabled = true;
        }
        else
        {
            mainCamera.GetComponent<Camera>().enabled = true;
            if (playerCamera)
                playerCamera.GetComponent<Camera>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!isGameOver)
        {
            if (!playerShip)
            {
                if (lives > 0)
                    spawnPlayer();
                else
                    gameOver();

            }

            int num_asteroid = GameObject.FindGameObjectsWithTag("Asteroid").Length;
            int num_alien = GameObject.FindGameObjectsWithTag("AlienShip").Length;

            if (num_asteroid + num_alien <= 0)
            {
                spawnAsteroids();
            }

            if (alienTimerRemaining > 0)
            {
                alienTimerRemaining -= Time.deltaTime;
            }
            else if (num_alien < 2 && num_asteroid > 0 && difficulties[currentDifficultyLevel].alienSpan > 0)
            {
                alienTimerRemaining += difficulties[currentDifficultyLevel].alienSpan;
                spawnAlien();
            }
        }
	}

    void gameOver()
    {
        if (isGameOver)
            return;
        isGameOver = true;
        gameOverText.text = "Score: " + score;
        gameOverText.enabled = true;
        gameOverCanvas.enabled = true;
        gameOverNameInput.Select();
        gameOverNameInput.ActivateInputField();
        
    }

    void spawnPlayer()
    {
        if (isGameOver)
            return;

        this.lives -= 1;
        refreshUIText();
        playerShip = (GameObject)Instantiate(playerPrefab, playerStart, Quaternion.Euler(playerStartRotation));
        var player_wraping = playerShip.GetComponent<WarpingBehavior>();
        if (player_wraping)
            player_wraping.boundary = this.boundary;

        playerCamera = GameObject.FindGameObjectWithTag("FPCamera");
    }

    void spawnAsteroids()
    {
        if (isGameOver)
            return;

        currentDifficultyLevel += 1;
        if (currentDifficultyLevel >= difficulties.Count)
            currentDifficultyLevel = difficulties.Count - 1;

        int asteroidCount = Random.Range(difficulties[currentDifficultyLevel].minAsteroids
            , difficulties[currentDifficultyLevel].maxAsteroids);

        for (int i = 0; i< asteroidCount; i++)
        {
            var pos = Random.Range(10, 20) * AsteroidsMathHelper.randomDirectionXZ();

            if (playerShip)
            {
                pos += playerShip.transform.position;
            }

            GameObject ast = (GameObject)Instantiate(asteroidsSpawnPrefab, pos, Quaternion.identity);
            var ast_wraping = ast.GetComponent<WarpingBehavior>();
            if (ast_wraping)
                ast_wraping.boundary = this.boundary;
        }

        alienTimerRemaining = difficulties[currentDifficultyLevel].alienSpan;

    }

    public void spawnAlien()
    {
        GameObject alien;
        if (Random.value <= difficulties[currentDifficultyLevel].alienTwoWeight)
        {
            alien = (GameObject)Instantiate(smallAlienPrefab, new Vector3(100,0,0), Quaternion.identity);
        }
        else
        {
            alien = (GameObject)Instantiate(largeAlienPrefab, new Vector3(100, 0, 0), Quaternion.identity);
        }

        var alien_wraping = alien.GetComponent<AlienShipBehavior>();
        if (alien_wraping)
            alien_wraping.boundary = this.boundary;
    }

    void refreshUIText()
    {
        this.scoreText.text = "Score: " + this.score.ToString();
        this.livesText.text = "Lives: " + this.lives.ToString();
    }

    public void addScore(int score)
    {
        if (isGameOver)
            return;
        var pre_score = this.score;
        this.score += score;

        if ((this.score / 10000) > (pre_score / 10000))
        {
            lives += 1;
            // TODO: should find a "ding" sound
        }

        refreshUIText();
    }

    public void SubmitScore()
    {
        if (!isGameOver)
        {
            return;
        }
            
        string player_name = "Player";
        if (gameOverNameInput.text != "")
        {
            player_name = gameOverNameInput.text;
        }

        LeaderboardDataManager.Load();
        LeaderboardDataManager.addScore(new LeaderboardEntry(player_name, score));
        LeaderboardDataManager.Save();

        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        
    }

}
