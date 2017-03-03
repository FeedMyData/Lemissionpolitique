using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuPanel : MonoBehaviour {

	public Vector2 DeactivatedRelativePosition;
	private Vector2 DeactivatedPosition;
	private Vector2 ActivatedPosition;
	private Sequence ActivatingSequence;
	private Sequence DeactivatingSequence;
	private bool panelOn = true;

	void Awake() {
		ActivatedPosition = GetComponent<RectTransform>().anchoredPosition;
		DeactivatedPosition = GetComponent<RectTransform>().anchoredPosition + DeactivatedRelativePosition;

		DeactivatingSequence = DOTween.Sequence();
		DeactivatingSequence.Append(GetComponent<RectTransform>().DOAnchorPos(DeactivatedPosition, 0.5f));
		DeactivatingSequence.AppendCallback(()=>gameObject.SetActive(false));
		DeactivatingSequence.SetAutoKill(false);

		ActivatingSequence = DOTween.Sequence();
		ActivatingSequence.AppendCallback(()=>gameObject.SetActive(true));
		ActivatingSequence.Append(GetComponent<RectTransform>().DOAnchorPos(ActivatedPosition, 0.5f));
		ActivatingSequence.SetAutoKill(false);

		GetComponent<RectTransform>().anchoredPosition = DeactivatedPosition;
		panelOn = false;
	}

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void ActivatePanel() {
		if(!ActivatingSequence.IsPlaying() && GetComponent<RectTransform>().anchoredPosition != ActivatedPosition) {
			panelOn = true;
			if(DeactivatingSequence.IsPlaying()) {
				DeactivatingSequence.PlayBackwards();
			} else {
				ActivatingSequence.Restart();
			}
		}
	}

	public void DeactivatePanel() {
		if(!DeactivatingSequence.IsPlaying() && GetComponent<RectTransform>().anchoredPosition != DeactivatedPosition) {
			panelOn = false;
			if(ActivatingSequence.IsPlaying()) {
				ActivatingSequence.PlayBackwards();
			} else {
				DeactivatingSequence.Restart();
			}
		}
	}

	public void TogglePanel() {
		if(panelOn) {
			DeactivatePanel();
		} else {
			ActivatePanel();
		}
	}
}
