using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopularityFeedback : MonoBehaviour {

	private Sequence changingTextSequence;

	void Awake() {
		changingTextSequence = DOTween.Sequence();
		changingTextSequence.Append(GetComponent<RectTransform>().DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.5f, 2, 1.0f));
		changingTextSequence.SetAutoKill(false);
	}

	// Use this for initialization
//	void Start () {
//		
//	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void AnimChangePopularity(float newPop) {
		UpdateText(newPop);
		changingTextSequence.Restart();
	}

	public void UpdateText(float newPop) {
		int pop = (int)(newPop * 100);
		string newText = string.Format("{0}%", pop);
		GetComponent<UnityEngine.UI.Text>().text = newText;
	}

}
