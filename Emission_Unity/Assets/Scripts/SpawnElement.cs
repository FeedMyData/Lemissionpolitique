using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnElement : MonoBehaviour {

	public SpeechBubble speechBubble;
	private SpeechList speechList;

	private bool isInvincible = false;
	private bool canBeCrushed = false;

	private Color baseMeshColor;
	private Color baseBubbleBgColor;
	private MeshRenderer molenchonMesh;
	private Vector3 baseScale;
	private Vector3 speechBubbleBaseScale;
	private Sequence beginSpeechSeq;

	private GameManager gm;

	void Awake() {
		gm = FindObjectOfType<GameManager>();
		speechList = gm.molenchonSpeechList;
		molenchonMesh = GetComponentInChildren<MeshRenderer>();
		baseMeshColor = molenchonMesh.material.color;
		baseBubbleBgColor = speechBubble.background.color;
		baseScale = transform.localScale;
		speechBubbleBaseScale = speechBubble.GetComponent<RectTransform>().localScale;
	}

	// Use this for initialization
//	void Start () {
//		
//	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void InitMolenchon() { // Should instead extend SpawnElement and have specific methods for each kind of element
		transform.localPosition = Vector3.zero;
		if(gm.probabilityOfInvincibleMolenchon == 0.0f) {
			isInvincible = false;
		} else {
			isInvincible = Random.value <= gm.probabilityOfInvincibleMolenchon ? true : false;
		}
		speechBubble.GetComponent<RectTransform>().localScale = Vector3.zero;
		speechBubble.speechText.text = "";
		speechBubble.background.color = baseBubbleBgColor;
		transform.localScale = baseScale;
		molenchonMesh.material.color = baseMeshColor;
		MoveUp();
	}

	void MoveUp() {
		transform.DOLocalMoveY(2.5f, 0.2f).OnComplete(()=>BeginSpeech()).Play();
	}

	void BeginSpeech() {
		canBeCrushed = true;
		SpeechElement speech = speechList.ChooseSpeech();
		if(speech != null) {
			beginSpeechSeq = DOTween.Sequence();
			beginSpeechSeq.Append(speechBubble.GetComponent<RectTransform>().DOScale(speechBubbleBaseScale, 0.2f));
			beginSpeechSeq.Append(speechBubble.speechText.DOText(speech.text, gm.timePerCharacterMolenchonSpeech * speech.text.Length));
			beginSpeechSeq.OnComplete(()=>EndOfSpeech()).Play();
		}
	}

	void EndOfSpeech() {
		canBeCrushed = false;
		gm.MolenchonFinishedSpeech();
		Sequence endSeq = DOTween.Sequence();
		endSeq.Append(speechBubble.background.DOColor(Color.green, 0.5f));
		endSeq.Append(speechBubble.GetComponent<RectTransform>().DOScale(0, 0.5f));
		endSeq.AppendCallback(()=>MoveDown()).Play();
	}

	void MoveDown() {
		transform.DOLocalMoveY(0, 0.5f).OnComplete(()=>Despawn()).Play();
	}

	void Despawn() {
		SimplePool.Despawn(gameObject);
	}

	void Crush() {
		canBeCrushed = false;
		if(beginSpeechSeq != null && beginSpeechSeq.IsPlaying()) {
			beginSpeechSeq.Pause();
		}
		Sequence crushSeq = DOTween.Sequence();
		crushSeq.Append(transform.DOScaleY(0.2f, 0.5f));
		crushSeq.Join(transform.DOScaleX(1.6f, 0.5f));
		crushSeq.Join(transform.DOScaleZ(1.6f, 0.5f));
		crushSeq.Append(speechBubble.background.DOColor(Color.red, 0.5f));
		crushSeq.Append(molenchonMesh.material.DOFade(0, 1.0f));
		crushSeq.AppendCallback(()=>Despawn()).Play();
	}

	public void ReceiveHit() {
		if(isInvincible) {
			// Feedback for invicible Hologram Molenchon
			ShowHologram();
		} else if (canBeCrushed) {
			// Crush this one
			gm.MolenchonCrushed();
			Crush();
		}
	}

	void ShowHologram() {
		Color newColor = molenchonMesh.material.color;
		newColor.a = 0.8f;
		molenchonMesh.material.color = newColor;

		if(beginSpeechSeq != null && beginSpeechSeq.IsPlaying()) {
			beginSpeechSeq.Pause();

			Sequence stenchonSeq = DOTween.Sequence();
			stenchonSeq.Append(speechBubble.speechText.DOText("Can't stenchon the Melenchon !", 0.2f));
			stenchonSeq.AppendInterval(0.8f);
			stenchonSeq.OnComplete(()=>ResumeSpeechAtPosition(beginSpeechSeq.Elapsed() + stenchonSeq.Duration()));
			stenchonSeq.Play();
		}
	}

	void ResumeSpeechAtPosition(float position) {
		if(beginSpeechSeq != null) {
			beginSpeechSeq.Goto(position);
			beginSpeechSeq.Play();
		}
	}
}
