using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class MenuPanel : MonoBehaviour {

	public Vector3 DeactivatedScale;
//	private Vector2 DeactivatedPosition;
	private Vector3 ActivatedScale;
	private Sequence ActivatingSequence;
	private Sequence DeactivatingSequence;
	private bool panelOn = true;
	private CanvasGroup content;

	public UnityEvent StartIntroEvent = new UnityEvent();
	public UnityEvent EndIntroEvent = new UnityEvent();
	public UnityEvent StartOutroEvent = new UnityEvent();
	public UnityEvent EndOutroEvent = new UnityEvent();


	void Awake() {
		content = GetComponentInChildren<CanvasGroup>();

		ActivatedScale = GetComponent<RectTransform>().localScale;
//		DeactivatedPosition = GetComponent<RectTransform>().anchoredPosition + DeactivatedRelativePosition;

		DeactivatingSequence = DOTween.Sequence();
		DeactivatingSequence.AppendCallback(()=>StartOutroEvent.Invoke());
		DeactivatingSequence.AppendCallback(()=>ActivateContent(false));
		DeactivatingSequence.Append(content.DOFade(0, 0.3f));
		DeactivatingSequence.Append(GetComponent<RectTransform>().DOScale(DeactivatedScale, 0.3f));
		DeactivatingSequence.AppendCallback(()=>gameObject.SetActive(false));
		DeactivatingSequence.AppendCallback(()=>EndOutroEvent.Invoke());
		DeactivatingSequence.SetAutoKill(false);

		ActivatingSequence = DOTween.Sequence();
		ActivatingSequence.AppendCallback(()=>StartIntroEvent.Invoke());
		ActivatingSequence.AppendCallback(()=>gameObject.SetActive(true));
		ActivatingSequence.Append(GetComponent<RectTransform>().DOScale(ActivatedScale, 0.3f));
		ActivatingSequence.AppendCallback(()=>ActivateContent(true));
		ActivatingSequence.Append(content.DOFade(1, 0.3f));
		ActivatingSequence.AppendCallback(()=>EndIntroEvent.Invoke());
		ActivatingSequence.SetAutoKill(false);

		GetComponent<RectTransform>().localScale = DeactivatedScale;
		content.alpha = 0;
		content.interactable = false;
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

	void ActivateContent(bool active) {
		content.interactable = active;
	}

	public void ActivatePanel() {
		if(!ActivatingSequence.IsPlaying() && GetComponent<RectTransform>().localScale != ActivatedScale) {
			panelOn = true;
			if(DeactivatingSequence.IsPlaying()) {
				DeactivatingSequence.PlayBackwards();
			} else {
				ActivatingSequence.Restart();
			}
		}
	}

	public void DeactivatePanel() {
		if(!DeactivatingSequence.IsPlaying() && GetComponent<RectTransform>().localScale != DeactivatedScale) {
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
