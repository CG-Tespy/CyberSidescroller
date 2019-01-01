using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour {

	public GameObject spawnPoint;
    
    public float stageBottom, stageLeftBound, stageRightBound;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < stageBottom) {
			Respawn ();
		}
		if (transform.position.x > stageRightBound) {
			Respawn ();
		}
		if (transform.position.x < stageLeftBound) {
			Respawn ();
		}

	}


	void Respawn(){
		transform.position = spawnPoint.transform.position;
	}

	void OnTriggerEnter(Collider checkpoint){
		if (checkpoint.tag == "SpawnPoint") {
			if (checkpoint.gameObject != spawnPoint) {
				spawnPoint = checkpoint.gameObject;
				Debug.Log ("Checkpoint found.");
			}
		}
	}
}
