using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class SoundEffectElement {
	public string groupName;
	public AudioClip audioClip;
}

public class SoundManager : MonoBehaviour {

	public SoundEffectElement[] soundEffectArray;

	private Dictionary<string, AudioSource> audioSourceDic = new Dictionary<string, AudioSource>();

	public UnityEngine.UI.Image soundImageButtonWithOnSprite;
	public Sprite soundOffSprite;
	private Sprite soundOnSprite;
	private bool soundOn = true;

	void Awake() {
		// Here AudioSources on different parents are not possible for the same AudioClip :(
		AudioSource[] components = GetComponentsInChildren<AudioSource>();
		foreach(AudioSource comp in components) {
			if(comp.clip != null && !audioSourceDic.ContainsKey(comp.clip.name)) {
				audioSourceDic.Add(comp.clip.name, comp);
			}
		}
		AudioListener.volume = 1.0f;
		soundOnSprite = soundImageButtonWithOnSprite.sprite;
	}

	// Use this for initialization
//	void Start () {
//	}

	public void PlaySoundEffectElement(string groupName, float volume = 1.0f , Vector3 worldPosition = default(Vector3)) {
		List<AudioClip> randomList = new List<AudioClip>();
		foreach(SoundEffectElement se in soundEffectArray) {
			if(se.groupName == groupName) {
				randomList.Add(se.audioClip);
			}
		}
		if(randomList.Count > 0) {
			AudioSource.PlayClipAtPoint(randomList[Random.Range(0, randomList.Count)], worldPosition, volume);
		} else {
			Debug.LogWarningFormat("No sound effect found to play with group name: {0}", groupName);
		}
	}

	public void PlayAudioSource(string clipName, float volume = 1.0f) {
		AudioSource aSource;
		if(audioSourceDic.TryGetValue(clipName, out aSource)) {
			aSource.volume = volume;
			aSource.Play();
		} else {
			Debug.LogWarningFormat("No audio source found to play with clip name: {0}", clipName);
		}
	}

	public void StopAndFadeOutAudioSource(string clipName, float endValue, float fadeTime) {
		AudioSource aSource;
		if(audioSourceDic.TryGetValue(clipName, out aSource)) {
			if(aSource.isPlaying) {
				aSource.DOFade(endValue, fadeTime).OnComplete(()=>aSource.Stop()).Play();
			}
		} else {
			Debug.LogWarningFormat("No audio source found to stop with clip name: {0}", clipName);
		}
	}

	public void ToggleMute() {
		soundOn = !soundOn;
		soundImageButtonWithOnSprite.sprite = soundOn ? soundOnSprite : soundOffSprite;
		AudioListener.volume = 1.0f - AudioListener.volume;
	}


	// Update is called once per frame
//	void Update () {
//		
//	}
}
