using UnityEngine;
using System.Collections;

public class MainMenuBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        if (Input.GetButtonUp("Submit"))
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Leaderboard()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
