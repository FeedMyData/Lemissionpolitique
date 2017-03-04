using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnElement : MonoBehaviour {

	public SpeechBubble speechBubble;
	public SpeechBubble hologramBubble;
	public Color hologramColor = new Color(0, 115.0f/255.0f, 172.0f/255.0f);
	public Color bubbleSpeechInterruptedColor = Color.red;
	public Color bubbleSpeechValidatedColor = Color.green;
	private SpeechList speechList;

	private bool isInvincible = false;
	private bool canBeCrushed = false;

	private Color baseMeshColor;
	private Color baseBubbleBgColor;
	private Color baseEmissiveColor;
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
		baseEmissiveColor = molenchonMesh.material.GetColor("_EmissionColor");
		baseScale = molenchonMesh.transform.localScale;
		speechBubbleBaseScale = speechBubble.GetComponent<RectTransform>().localScale;
		hologramBubble.gameObject.SetActive(false);
	}

	// Use this for initialization
	void Start () {
		hologramBubble.gameObject.SetActive(false);
	}
	
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
		molenchonMesh.transform.localScale = baseScale;
		molenchonMesh.material.color = baseMeshColor;
		molenchonMesh.material.SetColor("_EmissionColor", baseEmissiveColor);
		hologramBubble.gameObject.SetActive(false);
		MoveUp();
	}

	void MoveUp() {
		transform.DOLocalMoveY(2.0f, 0.2f).OnComplete(()=>BeginSpeech()).Play();
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
		endSeq.Append(speechBubble.background.DOColor(bubbleSpeechValidatedColor, 0.5f));
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
		crushSeq.Append(molenchonMesh.transform.DOScaleY(0.2f, 0.2f));
		crushSeq.Join(molenchonMesh.transform.DOScaleX(1.6f, 0.2f));
		crushSeq.Join(molenchonMesh.transform.DOScaleZ(1.6f, 0.2f));
		crushSeq.Append(speechBubble.background.DOColor(bubbleSpeechInterruptedColor, 0.2f));
		crushSeq.Append(molenchonMesh.material.DOFade(0, 0.2f));
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
		molenchonMesh.material.DOColor(molenchonMesh.material.color - new Color(0,0,0,0.2f), 0.5f).Play();
		molenchonMesh.material.DOColor(hologramColor, "_EmissionColor", 0.5f).Play();

		gm.sm.PlaySoundEffectElement("Hologram");

		Sequence holoBubSeq = DOTween.Sequence();
		hologramBubble.GetComponent<CanvasGroup>().alpha = 1;
		hologramBubble.gameObject.SetActive(true);
		holoBubSeq.AppendInterval(0.5f);
		holoBubSeq.Append(hologramBubble.GetComponent<CanvasGroup>().DOFade(0, 1.0f));
		holoBubSeq.AppendCallback(()=>hologramBubble.gameObject.SetActive(false));
		holoBubSeq.Play();

//		if(beginSpeechSeq != null && beginSpeechSeq.IsPlaying()) {
//			beginSpeechSeq.Pause();
//
//			Sequence stenchonSeq = DOTween.Sequence();
//			stenchonSeq.Append(speechBubble.speechText.DOText("Can't stenchon the Mélenchon !", 0.2f));
//			stenchonSeq.AppendInterval(0.8f);
//			stenchonSeq.OnComplete(()=>ResumeSpeechAtPosition(beginSpeechSeq.Elapsed() + stenchonSeq.Duration()));
//			stenchonSeq.Play();
//		}
	}

	void ResumeSpeechAtPosition(float position) {
		if(beginSpeechSeq != null) {
			beginSpeechSeq.Goto(position);
			beginSpeechSeq.Play();
		}
	}
}
