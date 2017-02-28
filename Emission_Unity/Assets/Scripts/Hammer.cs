using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour {

	public Transform hammerRestZone;

	public float timeToGoToPosition = 0.2f;
	public float timeToGoToRest = 0.2f;

	private GameManager gm;

	private bool readyToCrushAgain = true;

	void Awake() {
		gm = FindObjectOfType<GameManager>();
	}

	// Use this for initialization
//	void Start () {
//		
//	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void ReceiveInputAction(Vector3 position) {
		if(readyToCrushAgain) {
			GoToCrushPosition(position);
		}
	}

	void GoToCrushPosition(Vector3 pos) {
		readyToCrushAgain = false;
	}

	void BackToRest() {
		// At the end, readyToCrushAgain is true
	}

	void Crush() {
		// Activate Collider during the animation and deactivate at the end
		// At the end, check if the hammer has crushed to make specific feedbacks?
	}

	void RemoveHammer() {
		
	}

	void OnTriggerEnter(Collider otherCollider) {
		if(otherCollider.GetComponent<SpawnElement>() != null) {
			otherCollider.GetComponent<SpawnElement>().ReceiveHit();
		}
	}
		
	void EnableDisableCollider(bool active) {
		GetComponent<BoxCollider>().enabled = active;
	}
}
