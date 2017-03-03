using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMenuManager : MonoBehaviour {

	public MenuPanel playPanel;
	public MenuPanel infoPanel;
	public MenuPanel scorePanel;

	private bool IsStartMenu = false;
	private bool IsActionToPlay = false;

	private GameManager gm;

	void Awake() {
		gm = FindObjectOfType<GameManager>();
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(DelayedStart());
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	IEnumerator DelayedStart() {
		yield return new WaitForSeconds(0.5f);
		IsStartMenu = true;
		playPanel.ActivatePanel();
	}

	public void ShowEndGamePanels() {
		IsStartMenu = false;
		if(scorePanel.GetComponent<ScoreInfo>() != null) {
			scorePanel.GetComponent<ScoreInfo>().UpdatePanelScoreText();
		}
		scorePanel.ActivatePanel();
	}

	public void CheckBinaryMenu() {
		if(IsStartMenu) {
			playPanel.ActivatePanel();
		} else {
			scorePanel.ActivatePanel();
		}
	}

	public void SetActionToPlay() {
		IsActionToPlay = true;
	}

	public void SetActionToInfo() {
		IsActionToPlay = false;
	}

	public void CheckBinaryAction() {
		if(IsActionToPlay) {
			gm.StartNewGame();
		} else {
			infoPanel.ActivatePanel();
		}
	}

}
