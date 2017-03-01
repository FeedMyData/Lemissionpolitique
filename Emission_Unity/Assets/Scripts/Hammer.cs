﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hammer : MonoBehaviour {

	public Transform hammerRestZone;
	public GameObject baseHammerToUse;
	public Transform usersContainer;
	public SpeechBubble speechBubble;

	public float timeToGoToPosition = 0.2f;
	public float timeToGoToRest = 0.2f;

	private GameManager gm;
	private HammerUser[] hammerUsers;
	private HammerUser specificCurrentUser;

	private bool readyToCrushAgain = true;

	void Awake() {
		gm = FindObjectOfType<GameManager>();
		hammerUsers = usersContainer.GetComponentsInChildren<HammerUser>();
		speechBubble.gameObject.SetActive(false);
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
		transform.DOMove(pos, 0.5f).OnComplete(()=>DoCrush()).Play();
	}

	void BackToRest() {
		// At the end, readyToCrushAgain is true
		transform.DOMove(hammerRestZone.position, 0.5f).OnComplete(()=>readyToCrushAgain = true).Play();
	}

	void DoCrush() {
		// Still need asign user, speech and specific hammer
		Sequence doCrush = DOTween.Sequence();
		doCrush.Append(transform.DOLocalMoveY(-1.0f, 0.2f).SetRelative());
		doCrush.InsertCallback(0,()=>EnableDisableCollider(true));
		doCrush.AppendCallback(()=>EnableDisableCollider(false));
		doCrush.Append(transform.DOLocalMoveY(1.0f, 0.2f).SetRelative());
		doCrush.AppendCallback(()=>BackToRest());
		doCrush.Play();
	}

	void OnTriggerEnter(Collider otherCollider) {
		if(otherCollider.GetComponent<SpawnElement>() != null) {
			otherCollider.GetComponent<SpawnElement>().ReceiveHit();
		}
	}
		
	void EnableDisableCollider(bool active) {
		GetComponent<BoxCollider>().enabled = active;
	}

	void AssignSpecificUser() {
		specificCurrentUser = ChooseUser();
	}

	void DisplaySpecificHammer() {
		if(specificCurrentUser != null) {
			baseHammerToUse.SetActive(false);
			specificCurrentUser.SpecificHammerToUse.SetActive(true);
		}
	}
		
	void DisplayBaseHammer() {
		if(specificCurrentUser != null) {
			specificCurrentUser.SpecificHammerToUse.SetActive(false);
		}
		baseHammerToUse.SetActive(true);
	}

	void RemoveSpecificUser() {
		DisplayBaseHammer();
		specificCurrentUser = null;
	}

	HammerUser ChooseUser() {
		List<HammerUser> listForProbability = MakeProbabilityList();
		HammerUser userChosen = null;
		if(listForProbability.Count > 0) {
			userChosen = listForProbability[Random.Range(0, listForProbability.Count - 1)];
		}
		return userChosen;
	}

	List<HammerUser> MakeProbabilityList() {
		List<HammerUser> listForProbability = new List<HammerUser>();
		foreach(HammerUser user in hammerUsers) {
			for(int i = 0; i < user.weightProbabilityToBeTheUser; i++) {
				listForProbability.Add(user);
			}
		}
		return listForProbability;
	}
}
