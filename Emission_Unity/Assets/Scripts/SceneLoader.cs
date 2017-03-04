using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour {

	private AsyncOperation asyncLoading;
	public CanvasGroup container;
	public UnityEngine.RectTransform progressBar;
	private bool changingScene = false;

	void Awake() {
		container.alpha = 0;
	}

	// Use this for initialization
	void Start () {
		container.DOFade(1, 0.5f).OnComplete(()=>StartLoading()).SetDelay(0.5f).Play();
	}

	void StartLoading() {
		asyncLoading = SceneManager.LoadSceneAsync(1);
		asyncLoading.allowSceneActivation = false;
	}

	// Update is called once per frame
	void Update () {
		if(asyncLoading != null && !changingScene) {
			if(asyncLoading.isDone) {
				changingScene = true;
				FinishedLoading();
			} else {
				UpdateProgressBar();
			}
		}
	}

	void UpdateProgressBar() {
		if(asyncLoading != null) {
			progressBar.DOSizeDelta(new Vector2(1920.0f * asyncLoading.progress, progressBar.sizeDelta.y), 0.5f, true).Play();
		}
	}

	void FinishedLoading() {
		progressBar.DOSizeDelta(new Vector2(1920.0f, progressBar.sizeDelta.y), 0.5f, true).Play();
		container.DOFade(0, 0.5f).OnComplete(()=>ChangeScene()).Play();
	}

	void ChangeScene() {
		if(asyncLoading != null) {
			asyncLoading.allowSceneActivation = true;
		}
	}
}
