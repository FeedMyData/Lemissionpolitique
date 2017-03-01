using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	private GameManager gm;
	private Hammer hammer;

	void Awake() {
		gm = FindObjectOfType<GameManager>();
		hammer = FindObjectOfType<Hammer>();
	}

	// Use this for initialization
//	void Start () {
//		
//	}
	
	// Update is called once per frame
	void Update () {
//		if(gm.IsGameRunning()) {
			CheckTouchOrMouseInteraction();
//		}
	}

	void CheckTouchOrMouseInteraction() {
		if(Input.touchCount > 0) {
			if(Input.GetTouch(0).phase == TouchPhase.Began){
				CheckInteraction(Input.GetTouch(0).position);
			}
		} else if(Input.GetMouseButtonDown(0)) {
			CheckInteraction(Input.mousePosition);
		}
	}

	void CheckInteraction(Vector3 pos) {
//		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(pos);
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow, 5);
		RaycastHit[] hits = Physics.RaycastAll(ray);
		foreach(RaycastHit hitResult in hits) {
			if (gm.HammerInteractionZone != null && hitResult.transform.gameObject == gm.HammerInteractionZone) {
				Debug.DrawRay(hitResult.point, Vector3.up * 2, Color.red, 5);
				hammer.ReceiveInputAction(hitResult.point);
				break;
			}
		}
//		if (Physics.Raycast(ray, out hit)) {
//			if (gm.HammerInteractionZone != null && hit.transform.gameObject == gm.HammerInteractionZone) {
//				Debug.DrawRay(hit.point, Vector3.up * 2, Color.red, 5);
//				hammer.ReceiveInputAction(hit.point);
//			}
//		}
	}
}
