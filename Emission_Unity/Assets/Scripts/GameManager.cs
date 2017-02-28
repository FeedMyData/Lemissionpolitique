using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float gameSecondsDuration = 10.0f;
	private float currentTimer = 0.0f;
	public float minTimeNewSpawn = 1.0f;
	public float maxTimeNewSpawn = 3.0f;
	public int minMolenchonsToSpawnAtTheSameTime = 1;
	public int maxMolenchonsToSpawnAtTheSameTime = 4;
	[Range(0.0f, 1.0f)]
	public float probabilityOfInvincibleMolenchon = 0.5f;
	[Range(0.0f, 1.0f)]
	public float beginningPopularity = 0.5f;

	private float currentPopularity = 0.0f;
	private int totalMolenchonCrushed = 0;
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
//		StartNewGame();
	}
	
	// Update is called once per frame
	void Update () {
		if(isGameRunning) {
			UpdateTimer();
		}
	}

	void StartNewGame() {
		foreach(Spawner sp in spawnPositions) {
			sp.Clean();
		}
		totalMolenchonCrushed = 0;
		currentPopularity = beginningPopularity;
		StopCoroutine(PlayingRoutine());

		isGameRunning = true;
		StartCoroutine(PlayingRoutine());
	}

	IEnumerator PlayingRoutine() {
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
	}

	void SpawnMolenchons() {
		int molenchonsToSpawn = Random.Range(minMolenchonsToSpawnAtTheSameTime, maxMolenchonsToSpawnAtTheSameTime);
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
			if(!sp.hasActiveMolenchon) {
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
	}
}
