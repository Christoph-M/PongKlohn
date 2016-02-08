using UnityEngine;
using System.Collections;

public class AudioLoop : MonoBehaviour {
	public AudioSource monkIntroAudio;
	public AudioSource monkLoopAudio;
    /*public AudioSource monkLoopAudio;
    public AudioSource monkLoopAudio;
    public AudioSource monkLoopAudio;*/

    public AudioSource schrei1;
	public AudioSource schrei2;
	public AudioSource schrei3;
	public AudioSource schrei4;
	public AudioSource schrei5;
	public AudioSource schrei6;
	public AudioSource schrei7;
	public AudioSource schrei8;
    public AudioSource Dash_0;
    public AudioSource Dash_1;
    public AudioSource Dash_2;
    public AudioSource Dash_3;
    public AudioSource Block;
    public AudioSource Block_0_move;
    public AudioSource Block_1_move;
    public AudioSource Block_2_move;
    public AudioSource Block_3_move;
    public AudioSource Ball_fast_0;
    public AudioSource Ball_fast_1;
    public AudioSource Ball_fast_2;
    public AudioSource Ball_medium_0;
    public AudioSource Ball_medium_1;
    public AudioSource Ball_medium_2;
    public AudioSource Ball_slow_0;
    public AudioSource Ball_slow_1;
    public AudioSource Ball_slow_2;
    public AudioSource Ball_Background;

	
	public void PlayBallSound(int i)
    {
        int schreiSound =0;
        if (i == 0)
        {
            schreiSound = (int)Random.Range(0f, 3f);
        }
        else if (i == 1)
        {
            schreiSound = (int)Random.Range(4f, 7f);
        }

        switch (schreiSound)
        {
            case 0:
                schrei1.Play(); break;
            case 1:
                schrei2.Play(); break;
            case 2:
                schrei3.Play(); break;
            case 3:
                schrei4.Play(); break;
            case 4:
                schrei5.Play(); break;
            case 5:
                schrei6.Play(); break;
            case 6:
                schrei7.Play(); break;
            case 7:
                schrei8.Play(); break;
            default:
                break;
        }
    }
	
	public void SetSrei(int i)
	{
        int schreiSound =0;
        if (i==0)
        {
            schreiSound = (int)Random.Range(0f,3f);
        }else if (i == 1)
        {
            schreiSound = (int)Random.Range(4f, 7f);
        }

        switch (schreiSound)
        {
            case 0:
                schrei1.Play(); break;
            case 1:
                schrei2.Play(); break;
            case 2:
                schrei3.Play(); break;
            case 3:
                schrei4.Play(); break;
            case 4:
                schrei5.Play(); break;
            case 5:
                schrei6.Play(); break;
            case 6:
                schrei7.Play(); break;
            case 7:
                schrei8.Play(); break;
            default:
                break;
        }
    }
}
