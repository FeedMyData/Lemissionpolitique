using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	[HideInInspector]
	public bool hasActiveMolenchon = false;

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
		Clean();
		hasActiveMolenchon = true;
		GameObject molenchon = SimplePool.Spawn(gm.MolenchonPrefab);
		molenchon.transform.SetParent(transform);
		molenchon.GetComponent<SpawnElement>().InitMolenchon();
	}

	public void Clean() {
		hasActiveMolenchon = false;
		foreach(GameObject obj in transform) {
			//Despawn
			SimplePool.Despawn(obj);
		}
	}
}
