using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hammer : MonoBehaviour {

	private Vector3 hammerRestPosition;
	public GameObject baseHammerToUse;
	public Transform usersContainer;
	public Transform userBubblesContainer;
	public GameObject prefabUserSpeechBubble;
	public Transform hammerContainer;

	public float timeToGoToPosition = 0.1f;
	public float timeToGoToRest = 0.1f;
	public float timetoCrushRotate = 5f;
	public float timeToCrushMoveDown = 0.05f;
	public float timeToCrushMoveUp = 0.05f;

	public Vector3 inputPositionOffset;
	public Vector3 userBubbleOffset;

	private GameManager gm;
	private HammerUser[] hammerUsers;
	private HammerUser specificCurrentUser;
	private GameObject currentHammerUsed;

	private Sequence backToRestSeq;

	private bool readyToCrushAgain = true;

	void Awake() {
		gm = FindObjectOfType<GameManager>();
		hammerUsers = usersContainer.GetComponentsInChildren<HammerUser>();
//		speechBubble.gameObject.SetActive(false);
		EnableDisableCollider(false);
		hammerRestPosition = transform.position;
		DisplaySpecificHammer();
	}

	// Use this for initialization
//	void Start () {
//		
//	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void ReceiveInputAction(Vector3 position) {
		if(readyToCrushAgain) {
			GoToCrushPosition(position);
		}
	}

	void GoToCrushPosition(Vector3 pos) {
		if(backToRestSeq != null && backToRestSeq.IsPlaying()) {
			backToRestSeq.Kill();
		}
		readyToCrushAgain = false;
		AssignSpecificUser();
		DisplaySpecificHammer();
		AnimateSpecificUser();
		pos += inputPositionOffset;
		transform.DOMove(pos, timeToGoToPosition).OnComplete(()=>DoCrush()).Play();
	}

	void BackToRest() {
		readyToCrushAgain = true;
		backToRestSeq = DOTween.Sequence();
		backToRestSeq.AppendInterval(0.2f);
		backToRestSeq.Append(transform.DOMove(hammerRestPosition, timeToGoToRest));
//		backToRestSeq.AppendCallback(()=>ArriveAtRest());
		backToRestSeq.Play();
	}

	void ArriveAtRest() {
//		DisplayBaseHammer();
	}

	void DoCrush() {
		Sequence doCrush = DOTween.Sequence();
		doCrush.AppendCallback(()=>SpawnSpeechBubble());
		doCrush.Append(hammerContainer.DOLocalRotate(new Vector3(0,0,50), timetoCrushRotate));
		doCrush.Append(hammerContainer.DOLocalRotate(new Vector3(0,0,-20), timetoCrushRotate));
		doCrush.Append(transform.DOLocalMoveY(-1.0f, timeToCrushMoveDown).SetRelative());
		doCrush.InsertCallback(0,()=>EnableDisableCollider(true));
		doCrush.AppendCallback(()=>gm.sm.PlaySoundEffectElement("HammerHit"));
		doCrush.AppendCallback(()=>EnableDisableCollider(false));
		doCrush.Append(transform.DOLocalMoveY(1.0f, timeToCrushMoveUp).SetRelative());
		doCrush.Append(hammerContainer.DOLocalRotate(new Vector3(0,0,00), timetoCrushRotate));
		doCrush.AppendCallback(()=>BackToRest());
		doCrush.Play();
//		gm.HammerHasCrushed();
	}

//	void PlayHitHammerSound() {
//		
//	}

	void SpawnSpeechBubble() {
		if(specificCurrentUser != null) {
			GameObject userBubble = SimplePool.Spawn(prefabUserSpeechBubble);
			userBubble.transform.SetParent(userBubblesContainer);
			userBubble.transform.position = transform.position + userBubbleOffset;
			userBubble.GetComponent<UserSpeechBubble>().InitUserBubble(specificCurrentUser);
		}
	}

	void OnTriggerEnter(Collider otherCollider) {
		if(otherCollider.GetComponent<SpawnElement>() != null) {
			otherCollider.GetComponent<SpawnElement>().ReceiveHit();
		}
	}

	void EnableDisableCollider(bool active) {
		GetComponent<BoxCollider>().enabled = active;
	}

	void AssignSpecificUser() {
		specificCurrentUser = ChooseUser();
	}

	void AnimateSpecificUser() {
		if(specificCurrentUser != null) {
			specificCurrentUser.SpeakAnimation();
		}
	}

	void DisplaySpecificHammer() {
		GameObject hammerToUse = baseHammerToUse;
		if(specificCurrentUser != null) {
			hammerToUse = specificCurrentUser.SpecificHammerToUse;
		}
		if(hammerToUse != currentHammerUsed) {
			foreach(Transform hammer in hammerContainer) {
				if(hammer.gameObject == hammerToUse) {
					hammer.gameObject.SetActive(true);
				} else {
					hammer.gameObject.SetActive(false);
				}
			}
			currentHammerUsed = hammerToUse;
		}
	}
		
//	void DisplayBaseHammer() {
//		if(specificCurrentUser != null) {
//			specificCurrentUser.SpecificHammerToUse.SetActive(false);
//		}
//		baseHammerToUse.SetActive(true);
//	}

//	void RemoveSpecificUser() {
//		DisplayBaseHammer();
//		specificCurrentUser = null;
//	}

	HammerUser ChooseUser() {
		List<HammerUser> listForProbability = MakeProbabilityList();
		HammerUser userChosen = null;
		if(listForProbability.Count > 0) {
			userChosen = listForProbability[Random.Range(0, listForProbability.Count)];
		}
		return userChosen;
	}

	List<HammerUser> MakeProbabilityList() {
		List<HammerUser> listForProbability = new List<HammerUser>();
		foreach(HammerUser user in hammerUsers) {
			for(int i = 0; i < user.weightProbabilityToBeTheUser; i++) {
				listForProbability.Add(user);
			}
		}
		return listForProbability;
	}
}
