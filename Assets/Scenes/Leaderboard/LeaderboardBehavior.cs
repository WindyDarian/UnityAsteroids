using UnityEngine;
using System.Collections;

public class LeaderboardBehavior : MonoBehaviour {

    public GUIText leaderboardText;
    

	// Use this for initialization
	void Start () {
	    string lb = string.Format("{0,-10} {1,-10}\n", "Name", "Score");

        LeaderboardDataManager.Load();
        foreach (var score in LeaderboardDataManager.highScores)
        {
            lb += string.Format("{0,-10} {1,-10}\n", score.name, score.score);
        }

        leaderboardText.text = lb;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	    if (Input.GetButtonUp("Submit") || Input.GetButtonUp("Cancel"))
        {
            returnToMenu();
        }
	}

    public void returnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
