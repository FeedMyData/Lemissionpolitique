using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CommentElement {
	public float includeMinPop;
	public float excludeMaxPop;
	public string text;
}

public class ScoreInfo : MonoBehaviour {

	public Text popularityText;
	public Text commentText;

	public CommentElement[] commentArray;

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
		string chosenCommentText = "Can't stenchon the Mélenchon !";
		foreach(CommentElement el in commentArray) {
			if(gm.currentPopularity >= el.includeMinPop && gm.currentPopularity < el.excludeMaxPop) {
				chosenCommentText = el.text;
				break;
			}
		}
		commentText.text = chosenCommentText;
	}

}
