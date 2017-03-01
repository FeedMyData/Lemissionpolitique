using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnElement : MonoBehaviour {

	public SpeechBubble speechBubble;

	public SpeechList speechList;

	private bool isInvincible = false;
	private bool canBeCrushed = false;

	private Color baseMeshColor;
	private Color baseBubbleBgColor;

	private GameManager gm;

	void Awake() {
		gm = FindObjectOfType<GameManager>();
		speechBubble.gameObject.SetActive(false);
		baseMeshColor = GetComponent<MeshRenderer>().material.color;
		baseBubbleBgColor = speechBubble.background.color;
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
		speechBubble.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
		speechBubble.speechText.text = "";
		speechBubble.background.color = baseBubbleBgColor;
		speechBubble.gameObject.SetActive(false);
		transform.localScale = new Vector3(1, 1, 1);
		GetComponent<MeshRenderer>().material.color = baseMeshColor;
		MoveUp();
	}

	void MoveUp() {
		transform.DOLocalMoveY(2.5f, 0.5f).OnComplete(()=>BeginSpeech()).Play();
	}

	void BeginSpeech() {
		canBeCrushed = true;
		speechBubble.gameObject.SetActive(true);
		SpeechElement speech = ChooseSpeech();
		if(speech != null) {
			speechBubble.speechText.DOText(speech.text, gm.timePerCharacterMolenchonSpeech * speech.text.Length).OnComplete(()=>EndOfSpeech()).Play();
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

	SpeechElement ChooseSpeech() {
		if(speechList.SpeechArray.Length > 0) {
			return speechList.SpeechArray[Random.Range(0, speechList.SpeechArray.Length - 1)];
		}
		return null;
	}

	void MoveDown() {
		transform.DOLocalMoveY(0, 0.5f).OnComplete(()=>Despawn()).Play();
	}

	void Despawn() {
		SimplePool.Despawn(gameObject);
	}

	void Crush() {
		canBeCrushed = false;
		Sequence crushSeq = DOTween.Sequence();
		crushSeq.Append(transform.DOScaleY(0.2f, 0.5f));
		crushSeq.Join(transform.DOScaleX(1.6f, 0.5f));
		crushSeq.Join(transform.DOScaleZ(1.6f, 0.5f));
		crushSeq.Append(speechBubble.background.DOColor(Color.red, 0.5f));
		crushSeq.Append(GetComponent<MeshRenderer>().material.DOFade(0, 1.0f));
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
		Color newColor = GetComponent<MeshRenderer>().material.color;
		newColor.a = 0.5f;
		GetComponent<MeshRenderer>().material.color = newColor;
		Debug.Log("Can't stenchon the Melenchon !");
	}
}
