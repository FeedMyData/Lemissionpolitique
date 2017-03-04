using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSourceElement {
	public string groupName;
	public AudioSource audioSource;
}

public class SoundManager : MonoBehaviour {

	public AudioSourceElement[] audioArray;

	public AudioClip[] audioClipArray;
	private Dictionary<string, AudioClip> audioClipDic = new Dictionary<string, AudioClip>();

	void Awake() {
		foreach(AudioClip ac in audioClipArray) {
			audioClipDic.Add(ac.name, ac);
		}
		audioClipArray = null;
	}

	// Use this for initialization
//	void Start () {
//	}

	public void PlayAudioClip(string name, float volume = 1.0f , Vector3 worldPosition = default(Vector3)) {
		AudioClip ac;
		if(audioClipDic.TryGetValue(name, out ac)) {
			AudioSource.PlayClipAtPoint(ac, worldPosition, volume);
		}
	}

	public void PlayAudioClip(AudioClip audioClip, float volume = 1.0f, Vector3 worldPosition = default(Vector3)) {
		AudioSource.PlayClipAtPoint(audioClip, worldPosition, volume);
	}

	// Update is called once per frame
//	void Update () {
//		
//	}
}
