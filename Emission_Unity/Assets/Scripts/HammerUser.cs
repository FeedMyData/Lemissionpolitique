using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HammerUser : MonoBehaviour {

	public GameObject SpecificHammerToUse;
//	public Sprite profilePicture;
	public int weightProbabilityToBeTheUser = 5;
	public SpeechList speechList;
	private Sequence speakAnimSeq;

	void Awake() {
		speakAnimSeq = DOTween.Sequence();
		speakAnimSeq.SetAutoKill(false);
//		transform.DOPunchPosition(new Vector3(0,2,0),0.2f).Play();
//		transform.DOShakePosition(0.5f, 5, 20, 90).Play();
//		speakAnimSeq.Append(transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.5f, 2, 1.0f));
		speakAnimSeq.Append(transform.DOShakePosition(0.5f, 1, 10, 90));
	}

	// Use this for initialization
//	void Start () {
//		
//	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void SpeakAnimation() {
		speakAnimSeq.Restart();
	}
}
