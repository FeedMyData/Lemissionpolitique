using System.Collections;
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

	[HideInInspector]
	public float currentPopularity = 0.0f;
	[HideInInspector]
	public int totalMolenchonCrushed = 0;
	[HideInInspector]
	public int totalMolenchonEndedSpeech = 0;
//	[HideInInspector]
//	public int totalHammerCrushes = 0;

	public Spawner[] spawnPositions;

	public GameObject MolenchonPrefab;

	public GameObject HammerInteractionZone;

	private bool isGameRunning = false;

	private PanelMenuManager pmm;

	private Coroutine playingCoroutine;

	public MeshRenderer wallWithTexture;
	private Texture baseWallTexture;
	private Coroutine changingTexCoroutine;

	public UnityEngine.UI.Text textTimer;

	public UnityEngine.CanvasGroup canvasInGame;

	void Awake() {
		pmm = FindObjectOfType<PanelMenuManager>();
		if(wallWithTexture != null) {
			baseWallTexture = wallWithTexture.material.GetTexture("_MainTex");
		}
	}

	// Use this for initialization
	void Start () {
		canvasInGame.gameObject.SetActive(false);
		canvasInGame.alpha = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(isGameRunning) {
			UpdateTimer();
		}
	}


	// This function should be elsewhere but no more time running
	public void ChangeTexture(Texture newText) {
		if(newText != null) {
			if(changingTexCoroutine != null) {
				StopCoroutine(changingTexCoroutine);
			}
			changingTexCoroutine = StartCoroutine(AnimChangeTexture(newText));
		}
	}

	// This function should be elsewhere but no more time running
	IEnumerator AnimChangeTexture(Texture newTex) {
		if(wallWithTexture != null) {
			if(newTex != wallWithTexture.material.GetTexture("_MainTex")) {
				wallWithTexture.material.SetTexture("_MainTex", newTex);
			}
			yield return new WaitForSeconds(1.0f);
			wallWithTexture.material.SetTexture("_MainTex", baseWallTexture);
		}
	}

	public void StartNewGame() {
		ClearCurrentGameRunning();

		currentTimer = gameSecondsDuration;
		UpdateTextTimer();
		totalMolenchonCrushed = 0;
		totalMolenchonEndedSpeech = 0;
//		totalHammerCrushes = 0;
		currentPopularity = beginningPopularity;
		popularityScript.AnimChangePopularity(currentPopularity);
		ShowPlayingCanvas();
	}

	void LaunchPlayingRoutine() {
		playingCoroutine = StartCoroutine(PlayingRoutine());
	}

	void ShowPlayingCanvas() {
		canvasInGame.gameObject.SetActive(true);
		canvasInGame.alpha = 0;
		canvasInGame.DOFade(1, 2.0f).OnComplete(()=>LaunchPlayingRoutine()).Play();
	}

	void HidePlayingCanvas() {
		Sequence hideSeq = DOTween.Sequence();
		hideSeq.Append(canvasInGame.DOFade(0, 2.0f));
		hideSeq.AppendCallback(()=>canvasInGame.gameObject.SetActive(false));
		hideSeq.AppendCallback(()=>pmm.ShowEndGamePanels());
		hideSeq.Play();
	}

	IEnumerator PlayingRoutine() {
//		yield return new WaitForSeconds(1.0f);
		isGameRunning = true;
		while(isGameRunning) {
			SpawnMolenchons();
			float timeToWait = Random.Range(minTimeNewSpawn, maxTimeNewSpawn);
			yield return new WaitForSeconds(timeToWait);
		}
	}

	void UpdateTimer() {
		if(currentTimer <= 0.0f) {
//			StartCoroutine(EndGame());
			EndGame();
		} else {
			currentTimer -= Time.deltaTime;
		}
		UpdateTextTimer();
	}

	void EndGame() {
		isGameRunning = false;
		if(playingCoroutine != null) {
			StopCoroutine(playingCoroutine);
		}
		HidePlayingCanvas();
	}

	void ClearCurrentGameRunning() {
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

	public bool IsGameRunning() {
		return isGameRunning;
	}

	public void MolenchonCrushed() {
		if(isGameRunning) {
			totalMolenchonCrushed += 1;
			UpdatePopularity();
		}
	}

	public void MolenchonFinishedSpeech() {
		if(isGameRunning) {
			totalMolenchonEndedSpeech += 1;
			UpdatePopularity();
		}
	}

//	public void HammerHasCrushed() {
//		if(isGameRunning) {
//			totalHammerCrushes += 1;
//		}
//	}

	void UpdatePopularity() {
		float lifeTimePercentage = baseStepChangingPopularity * (totalMolenchonEndedSpeech - totalMolenchonCrushed) + beginningPopularity;
		lifeTimePercentage = Mathf.Clamp01(lifeTimePercentage);
		currentPopularity = DOVirtual.EasedValue(0, 1, lifeTimePercentage, Ease.InOutCirc);
		popularityScript.AnimChangePopularity(currentPopularity);
	}

	void UpdateTextTimer() {
		textTimer.text = string.Format("{0:00.0}", currentTimer);
	}
}
