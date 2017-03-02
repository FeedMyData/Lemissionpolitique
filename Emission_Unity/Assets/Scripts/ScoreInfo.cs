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
//		popularityText.text
//		molenchonCrushed.text
//		molenchonFinished.text
//		totalHammer.text
	}

}
