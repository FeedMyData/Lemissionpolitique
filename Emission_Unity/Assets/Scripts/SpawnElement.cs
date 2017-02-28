﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnElement : MonoBehaviour {

	public UnityEngine.UI.Text speechText;

	private bool isInvincible = false;
	private bool canBeHit = false;

	private GameManager gm;

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

	public void InitMolenchon() {
		transform.localPosition = Vector3.zero;
		if(gm.probabilityOfInvincibleMolenchon == 0.0f) {
			isInvincible = false;
		} else {
			isInvincible = Random.value <= gm.probabilityOfInvincibleMolenchon ? true : false;
		}
	}

	void MoveUp() {
		
	}

	void BeginSpeech() {
		canBeHit = true;
	}

	void MoveDown() {
		
	}

	void Hit() {
		// Call Crush or something else if invicible?
	}

	void Crush() {
		canBeHit = false;
	}

	void EndOfSpeech() {
		canBeHit = false;
	}
}
