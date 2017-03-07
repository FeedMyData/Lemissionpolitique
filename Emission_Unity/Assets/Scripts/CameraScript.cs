using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraScript : MonoBehaviour {

	private Sequence shakeAnimSeq;

	void Awake() {
		shakeAnimSeq = DOTween.Sequence();
		shakeAnimSeq.SetAutoKill(false);
		shakeAnimSeq.Append(transform.DOShakePosition(0.4f, 0.1f, 20, 90));
	}

	// Use this for initialization
//	void Start () {
//		
//	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void CameraShake() {
		shakeAnimSeq.Restart();
	}
}
