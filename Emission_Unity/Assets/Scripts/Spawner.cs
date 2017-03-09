using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

//	[HideInInspector]
//	public bool hasActiveMolenchon = false;

	private GameManager gm;
	private SpawnElement molenchonScript;

	void Awake() {
		gm = FindObjectOfType<GameManager>();
	}

	// Use this for initialization
	void Start () {
		GameObject molenchonObj = SimplePool.Spawn(gm.MolenchonPrefab);
		molenchonObj.SetActive(false);
		molenchonObj.transform.SetParent(transform);
		molenchonObj.transform.localPosition = Vector3.zero;
		molenchonScript = molenchonObj.GetComponent<SpawnElement>();
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void SpawnNew() {
//		GameObject molenchon = SimplePool.Spawn(gm.MolenchonPrefab);
//		molenchon.transform.SetParent(transform);
//		molenchon.GetComponent<SpawnElement>().InitMolenchon();
		molenchonScript.InitMolenchon();
	}

	public void Clean() {
//		foreach(Transform tr in transform) {
//			SimplePool.Despawn(tr.gameObject);
//		}
		if(molenchonScript != null) {
			molenchonScript.gameObject.SetActive(false);
		}
	}

	public bool HasActiveMolenchon() {
//		if(transform.GetComponentInChildren<SpawnElement>() != null && transform.GetComponentInChildren<SpawnElement>().gameObject.activeSelf) {
		if(molenchonScript != null && molenchonScript.gameObject.activeSelf) {
			return true;
		} else {
			return false;
		}
	}
}
