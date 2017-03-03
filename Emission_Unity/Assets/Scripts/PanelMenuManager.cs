using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMenuManager : MonoBehaviour {

	public MenuPanel playPanel;
	public MenuPanel infoPanel;
	public MenuPanel scorePanel;

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
		playPanel.ActivatePanel();
	}

	public void ShowEndGamePanels() {
//		scorePanel.GetComponent<ScoreInfo>().UpdatePanelScoreText();
		playPanel.ActivatePanel();
		scorePanel.ActivatePanel();
	}

	public void RemoveAllPanels() {
		playPanel.DeactivatePanel();
		infoPanel.DeactivatePanel();
//		scorePanel.DeactivatePanel();
	}

}
