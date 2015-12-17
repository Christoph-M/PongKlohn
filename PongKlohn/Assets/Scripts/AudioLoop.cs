using UnityEngine;
using System.Collections;

public class AudioLoop : MonoBehaviour {
	public AudioSource monkIntroAudio;
	public AudioSource monkLoopAudio;
	
	void LateUpdate () {
		if (!monkIntroAudio.isPlaying && !monkLoopAudio.enabled) {
			monkIntroAudio.enabled = false;
			monkLoopAudio.enabled = true;
		}
	}
}
