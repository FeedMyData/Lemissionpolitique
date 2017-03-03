using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreInfo : MonoBehaviour {

	public Text popularityText;
	public Text molenchonCrushed;
	public Text molenchonFinished;
	public Text totalHammer;

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

	public void UpdatePanelScoreText() {
		int pop = (int)(gm.currentPopularity * 100);
		popularityText.text = string.Format("{0}%", pop);
		molenchonCrushed.text = gm.totalMolenchonCrushed.ToString();
		molenchonFinished.text = gm.totalMolenchonEndedSpeech.ToString();
		totalHammer.text = gm.totalHammerCrushes.ToString();
	}

}
