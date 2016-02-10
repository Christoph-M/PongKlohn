using UnityEngine;
using System.Collections;

public class AudioLoop : MonoBehaviour
{
    public AudioSource monkIntroAudio;
    public AudioSource monkLoopAudio;
    /*public AudioSource monkLoopAudio;
    public AudioSource monkLoopAudio;
    public AudioSource monkLoopAudio;*/

    public AudioSource schrei1;
    public AudioSource schrei2;
    public AudioSource schrei3;
    public AudioSource schrei4;

    public AudioSource Dash_0;
    public AudioSource Dash_1;
    public AudioSource Dash_2;
    public AudioSource Dash_3;
    public AudioSource Dash_4;
    public AudioSource Dash_5;
    public AudioSource Dash_6;
    public AudioSource Dash_7;
    public AudioSource Dash_8;

    public AudioSource Block;

    public AudioSource Block_0_move;
    public AudioSource Block_1_move;
    public AudioSource Block_2_move;
    public AudioSource Block_3_move;
    public AudioSource Block_4_move;
    public AudioSource Block_5_move;
    public AudioSource Block_6_move;
    public AudioSource Block_7_move;


//	void LateUpdate() {
//		if (!monkIntroAudio.isPlaying && !monkLoopAudio.enabled) {
//			monkLoopAudio.enabled = true;
//		}
//	}

    public void PlaySchreiSound()
    {

        int schreiSound = (int)Random.Range(0f, 4f);


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
            default:
                break;
        }
    }

    public void PlayBlockMoveSound()
    {

        int blockMoveSound = (int)Random.Range(0f, 8f);


        switch (blockMoveSound)
        {
            case 0:
                Block_0_move.Play(); break;
            case 1:
                Block_1_move.Play(); break;
            case 2:
                Block_2_move.Play(); break;
            case 3:
                Block_3_move.Play(); break;
            case 4:
                Block_4_move.Play(); break;
            case 5:
                Block_5_move.Play(); break;
            case 6:
                Block_6_move.Play(); break;
            case 7:
                Block_7_move.Play(); break;
            default:
                break;
        }
    }

    public void PlayDashSound()
    {

        int dashSound = (int)Random.Range(0f, 9f);


        switch (dashSound)
        {
            case 0:
                Dash_0.Play(); break;
            case 1:
                Dash_1.Play(); break;
            case 2:
                Dash_2.Play(); break;
            case 3:
                Dash_3.Play(); break;
            case 4:
                Dash_4.Play(); break;
            case 5:
                Dash_5.Play(); break;
            case 6:
                Dash_6.Play(); break;
            case 7:
                Dash_7.Play(); break;
            default:
                break;
        }
    }

    public void PlayerBlockSound()
    {
        Block.Play();
    }


}
