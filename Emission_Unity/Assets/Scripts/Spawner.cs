using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

//	[HideInInspector]
//	public bool hasActiveMolenchon = false;

	private GameManager gm;

	void Awake() {
		gm = FindObjectOfType<GameManager>();
	}

	// Use this for initialization
//	void Start () {
//		
//	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void SpawnNew() {
		GameObject molenchon = SimplePool.Spawn(gm.MolenchonPrefab);
		molenchon.transform.SetParent(transform);
		molenchon.GetComponent<SpawnElement>().InitMolenchon();
	}

	public void Clean() {
		foreach(Transform tr in transform) {
			SimplePool.Despawn(tr.gameObject);
		}
	}

	public bool HasActiveMolenchon() {
		if(transform.GetComponentInChildren<SpawnElement>() != null && transform.GetComponentInChildren<SpawnElement>().gameObject.activeSelf) {
			return true;
		} else {
			return false;
		}
	}
}
