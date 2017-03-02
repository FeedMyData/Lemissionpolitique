﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour {

	public float gameSecondsDuration = 10.0f;
	private float currentTimer = 0.0f;
	public float minTimeNewSpawn = 1.0f;
	public float maxTimeNewSpawn = 3.0f;
	public int minMolenchonsToSpawnAtTheSameTime = 1;
	public int maxMolenchonsToSpawnAtTheSameTime = 4;
	[Range(0.0f, 1.0f)]
	public float probabilityOfInvincibleMolenchon = 0.5f;
	public float timePerCharacterMolenchonSpeech = 0.05f;
	[Range(0.0f, 1.0f)]
	public float beginningPopularity = 0.5f;
	[Range(0.0f, 1.0f)]
	public float baseStepChangingPopularity = 0.05f;
	public PopularityFeedback popularityScript;

	public SpeechList molenchonSpeechList;

	private float currentPopularity = 0.0f;
	private int totalMolenchonCrushed = 0;
	private int totalMolenchonEndedSpeech = 0;
//	private int totalHammerLaunched = 0;

	public Spawner[] spawnPositions;

	public GameObject MolenchonPrefab;

	public GameObject HammerInteractionZone;

	private bool isGameRunning = false;

//	void Awake() {
//		
//	}

	// Use this for initialization
	void Start () {
		StartNewGame();
	}
	
	// Update is called once per frame
	void Update () {
		if(isGameRunning) {
			UpdateTimer();
		}
	}

	void StartNewGame() {
		currentTimer = gameSecondsDuration;
		totalMolenchonCrushed = 0;
		totalMolenchonEndedSpeech = 0;
		currentPopularity = beginningPopularity;
		popularityScript.UpdateText(currentPopularity);
		InitialCountdown();
	}

	void InitialCountdown() {
		StartCoroutine(PlayingRoutine());
	}

	IEnumerator PlayingRoutine() {
		isGameRunning = true;
		while(isGameRunning) {
			float timeToWait = Random.Range(minTimeNewSpawn, maxTimeNewSpawn);
			yield return new WaitForSeconds(timeToWait);
			SpawnMolenchons();
		}
	}

	void UpdateTimer() {
		if(currentTimer <= 0.0f) {
			EndGame();
		} else {
			currentTimer -= Time.deltaTime;
		}
	}

	void EndGame() {
		isGameRunning = false;
		ClearCurrentGameRunning();
	}

	void ClearCurrentGameRunning() {
		StopCoroutine(PlayingRoutine());
		foreach(Spawner sp in spawnPositions) {
			sp.Clean();
		}
	}

	void SpawnMolenchons() {
		int molenchonsToSpawn = Random.Range(minMolenchonsToSpawnAtTheSameTime, maxMolenchonsToSpawnAtTheSameTime + 1);
		for(int i = 0; i <= molenchonsToSpawn; i++) {
			List<Spawner> availableSpawners = GetAvailableSpawners();
			if(availableSpawners.Count > 0) {
				int indexToSpawn = Random.Range(0, availableSpawners.Count);
				availableSpawners[indexToSpawn].SpawnNew();
			} else {
				break;
			}
		}
	}

	List<Spawner> GetAvailableSpawners() {
		List<Spawner> availableSpawners = new List<Spawner>();
		foreach(Spawner sp in spawnPositions) {
			if(!sp.HasActiveMolenchon()) {
				availableSpawners.Add(sp);
			}
		}
		return availableSpawners;
	}

	public float GetTimer() {
		return currentTimer;
	}

	public bool IsGameRunning() {
		return isGameRunning;
	}

	public void MolenchonCrushed() {
		totalMolenchonCrushed += 1;
		UpdatePopularity();
	}

	public void MolenchonFinishedSpeech() {
		totalMolenchonEndedSpeech += 1;
		UpdatePopularity();
	}

	void UpdatePopularity() {
		float lifeTimePercentage = baseStepChangingPopularity * (totalMolenchonEndedSpeech - totalMolenchonCrushed) + beginningPopularity;
		lifeTimePercentage = Mathf.Clamp01(lifeTimePercentage);
		currentPopularity = DOVirtual.EasedValue(0, 1, lifeTimePercentage, Ease.InOutCirc);
		popularityScript.AnimChangePopularity(currentPopularity);
	}
}
