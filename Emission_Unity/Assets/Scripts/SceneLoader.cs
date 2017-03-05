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
//			if(asyncLoading.isDone) {
			if(asyncLoading.progress >= 0.88f) {
				changingScene = true;
				FinishedLoading();
			} else {
				UpdateProgressBar();
			}
		}
	}

	void UpdateProgressBar() {
		if(asyncLoading != null) {
			progressBar.DOSizeDelta(new Vector2(Screen.width * asyncLoading.progress, progressBar.sizeDelta.y), 0.5f).Play();
		}
	}

	void FinishedLoading() {
		Sequence finishSeq = DOTween.Sequence();
		finishSeq.Append(progressBar.DOSizeDelta(new Vector2(Screen.width, progressBar.sizeDelta.y), 0.5f));
		finishSeq.Append(container.DOFade(0, 0.5f));
		finishSeq.AppendCallback(()=>ChangeScene()).Play();
	}

	void ChangeScene() {
		asyncLoading.allowSceneActivation = true;
	}
}
