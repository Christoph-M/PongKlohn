using UnityEngine;
using System.Collections;

public class AudioLoop : MonoBehaviour {
	public AudioSource monkIntroAudio;
	public AudioSource monkLoopAudio;
	public AudioSource schrei1;
	public AudioSource schrei2;
	public AudioSource schrei3;
	public AudioSource schrei4;
	public AudioSource schrei5;
	public AudioSource schrei6;
	public AudioSource schrei7;
	public AudioSource schrei8;
	private int schreiSound = 0;
	private int schreiMerker = 0;
	
	void LateUpdate () {
		if (!monkIntroAudio.isPlaying && !monkLoopAudio.enabled) {
			monkIntroAudio.enabled = false;
			monkLoopAudio.enabled = true;
		}
		
		if(schreiMerker != schreiSound)
		{
			switch (schreiSound)
			{
				case 0:
				{
					schreiMerker = schreiSound;
					schrei1.Play();
					break;
				}
				
				case 1:
				{
					schreiMerker = schreiSound;
					schrei2.Play();
					break;
				}
				
				case 2:
				{
					schreiMerker = schreiSound;
					schrei3.Play();
					break;
				}
				
				case 3:
				{
					schreiMerker = schreiSound;
					schrei4.Play();
					break;
				}
				
				case 4:
				{
					schreiMerker = schreiSound;
					schrei5.Play();
					break;
				}
				
				case 5:
				{
					schreiMerker = schreiSound;
					schrei6.Play();
					break;
				}
				
				case 6:
				{
					schreiMerker = schreiSound;
					schrei7.Play();
					break;
				}
				
				case 7:
				{
					schreiMerker = schreiSound;
					schrei8.Play();
					break;
				}
				
				default:
				{
					schreiMerker = schreiSound;
					break;
				}
			}
		}
	}
	
	public void SetSrei()
	{
		schreiSound = (int)Random.Range(0f,30f);
	}
}
