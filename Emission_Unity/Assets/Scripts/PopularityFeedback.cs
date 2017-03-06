using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopularityFeedback : MonoBehaviour {

	private RectTransform textRect;
	private Sequence changingTextSequence;
	private float minHeight = 150.0f;
	private float maxHeight = 700.0f;


	void Awake() {
		textRect = GetComponentInChildren<UnityEngine.UI.Text>().GetComponent<RectTransform>();
		changingTextSequence = DOTween.Sequence();
		changingTextSequence.Append(textRect.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.5f, 2, 1.0f));
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
		UpdateBar(newPop);
		changingTextSequence.Restart();
	}

	public void UpdateText(float newPop) {
		int pop = (int)(newPop * 100);
		string newText = string.Format("{0}%", pop);
		textRect.GetComponent<UnityEngine.UI.Text>().text = newText;
	}

	void UpdateBar(float pop) {
		float barHeight = pop * (maxHeight - minHeight) + minHeight;
		GetComponent<RectTransform>().DOSizeDelta(new Vector2(GetComponent<RectTransform>().sizeDelta.x, barHeight), 0.5f).SetEase(Ease.OutBack).Play();
	}

}
