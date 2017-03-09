using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelMenuManager : MonoBehaviour {

	public MenuPanel playPanel;
	public MenuPanel tutoPanel;
	public MenuPanel infoPanel;
	public MenuPanel scorePanel;
	public UnityEngine.UI.Image startingForeground;

//	private bool IsStartMenu = false;
//	private bool IsActionToPlay = false;

	private string menuState = "start_menu";
	private string actionState = "";

	private GameManager gm;

	void Awake() {
		gm = FindObjectOfType<GameManager>();
	}

	// Use this for initialization
	void Start () {
		Sequence introForeground = DOTween.Sequence();
		introForeground.Append(startingForeground.DOFade(0, 0.5f));
		introForeground.AppendCallback(()=>startingForeground.gameObject.SetActive(false));
		introForeground.AppendCallback(()=>DelayedStart());
		introForeground.Play();
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	void DelayedStart() {
		gm.sm.PlayAudioSource("Intro 1");
//		IsStartMenu = true;
		menuState = "start_menu";
		playPanel.ActivatePanel();
	}

	public void ShowEndGamePanels() {
//		IsStartMenu = false;
		menuState = "end_menu";
		if(scorePanel.GetComponent<ScoreInfo>() != null) {
			scorePanel.GetComponent<ScoreInfo>().UpdatePanelScoreText();
		}
		scorePanel.ActivatePanel();
	}

	public void CheckMenu() {
		switch(menuState) {
		case "start_menu":
			playPanel.ActivatePanel();
			break;
		case "end_menu":
			scorePanel.ActivatePanel();
			break;
		}
//		if(IsStartMenu) {
//			playPanel.ActivatePanel();
//		} else {
//			scorePanel.ActivatePanel();
//		}
	}

	public void SetActionToPlay() {
//		IsActionToPlay = true;
		actionState = "Play";
		gm.sm.StopAndFadeOutAudioSource("Intro 1", 0.0f, 1.0f);
	}

	public void SetActionToInfo() {
//		IsActionToPlay = false;
		actionState = "Info";
	}

	public void SetActionToTutorial() {
		actionState = "Tuto";
	}

	public void CheckAction() {
		switch(actionState) {
		case "Play":
			gm.StartNewGame();
			break;
		case "Info":
			infoPanel.ActivatePanel();
			break;
		case "Tuto":
			tutoPanel.ActivatePanel();
			break;
		}
//		if(IsActionToPlay) {
//			gm.StartNewGame();
//		} else {
//			infoPanel.ActivatePanel();
//		}
	}

}
