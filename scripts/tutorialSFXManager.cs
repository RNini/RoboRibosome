using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialSFXManager : MonoBehaviour
{
    public AudioSource playerAudio;

    // For gameplay sound effects
    public AudioClip[] moveSFX;
        public AudioClip[] matchSFX;
        public AudioClip mismatchSFX;

    public float moveFXVolume;
        public float matchFXVolume;
        public float switchFXVolume;

    // For tutorial sound effects
    public AudioClip buttonSFX;
        public AudioClip[] tvChangeSFX;

    public float buttonFXVolume;
        public float tvFXVolume;

    public AudioSource sfxAudio;

    
    //Volume settings modifiers
    public float soundFXVolMod;
    public float bgMusicMod;

    // For insuring continuity in volume preferences
    public GameObject bgMusicManager;

    public void Start()
    {
        // Assigns the bg volume slider to the current bg music source
        bgMusicManager = GameObject.Find("BG Music");

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            moveFXVolume = PlayerPrefs.GetFloat("sfxVolume");
        }
        
    }

    // Gameplay sound effects functions
    public void moveSound()
    {
        playerAudio.PlayOneShot(moveSFX[Random.Range(0, moveSFX.Length)], moveFXVolume * soundFXVolMod);
    }

        public void switchSound()
        {
            playerAudio.PlayOneShot(moveSFX[Random.Range(0, moveSFX.Length)], moveFXVolume * soundFXVolMod);
        }

        public void matchSound(int letterIndex)
        {
            playerAudio.PlayOneShot(matchSFX[letterIndex], matchFXVolume * soundFXVolMod);
        }

        public void mismatchSound()
        {
            playerAudio.PlayOneShot(mismatchSFX, matchFXVolume * soundFXVolMod);
        }

    // Tutorial sound effects functions
    public void buttonSound()
    {
        playerAudio.PlayOneShot(buttonSFX, buttonFXVolume * soundFXVolMod);
    }

        public void tvSound()
        {
            playerAudio.PlayOneShot(tvChangeSFX[Random.Range(0, tvChangeSFX.Length)], tvFXVolume * soundFXVolMod);
        }

}
