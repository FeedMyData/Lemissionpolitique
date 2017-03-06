using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UserSpeechBubble : MonoBehaviour {

	private SpeechBubble speechBubble;
	private Vector3 baseScale;
	private Sequence animationSeq;
	private GameManager gm;
	private string audioGroupNameToPlay = "";

	void Awake() {
		speechBubble = GetComponent<SpeechBubble>();
		baseScale = GetComponent<RectTransform>().localScale;
		animationSeq = DOTween.Sequence();
		animationSeq.AppendCallback(()=>gm.sm.PlaySoundEffectElement(audioGroupNameToPlay));
		animationSeq.Append(GetComponent<RectTransform>().DOScale(baseScale, 0.5f));
		animationSeq.AppendInterval(0.5f);
		animationSeq.Append(GetComponent<RectTransform>().DOScale(Vector3.zero, 0.5f));
		animationSeq.AppendCallback(()=>Despawn());
		animationSeq.SetAutoKill(false);
		gm = FindObjectOfType<GameManager>();
	}

	// Use this for initialization
//	void Start () {
//	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void InitUserBubble(HammerUser user) {
		GetComponent<RectTransform>().localScale = Vector3.zero;
		SpeechElement speech = user.speechList.ChooseSpeech();
		if(speech != null) {
			speechBubble.speechText.text = speech.text;
//			speechBubble.profilePicture.sprite = user.profilePicture;
			audioGroupNameToPlay = speech.audioGroupName;
			animationSeq.Restart();
			if(speech.wallTexture != null && gm != null) {
				gm.ChangeTexture(speech.wallTexture);
			}
		}
	}

	void Despawn() {
		SimplePool.Despawn(gameObject);
	}
}
