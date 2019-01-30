using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartScreen : MonoBehaviour {
    public bool loadingGame = false;
    public string sceneToLoad;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown && !loadingGame) {
            loadingGame = true;
            loadGame();
        }
	}

    void loadGame() {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
