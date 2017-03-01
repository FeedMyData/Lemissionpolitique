using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeechElement {
	public string text;
	public AudioClip audio;
}

public class SpeechList : MonoBehaviour {

	public SpeechElement[] SpeechArray;

	public SpeechElement ChooseSpeech() {
		if(SpeechArray.Length > 0) {
			return SpeechArray[Random.Range(0, SpeechArray.Length - 1)];
		}
		return null;
	}
	// Use this for initialization
//	void Start () {
//		
//	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
