using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgMusicManager : MonoBehaviour
{
    public AudioSource bgMusic;
        public AudioClip bgMusicIntro;
        public AudioClip bgMusicLoop;

    private bool playBool;
    
    // Start is called before the first frame update
    void Awake()
    {
        bgMusic = this.gameObject.GetComponent<AudioSource>();

        bgMusic.PlayOneShot(bgMusicIntro);

        playBool = true;

        // Protects this audio source and removes extras when moving between scenes
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.GetComponent<AudioSource>().isPlaying)
        {
            playBool = false;
        }

        if (playBool == false)
        {
            playBool = true;

                bgMusic.loop = true;
                bgMusic.clip = bgMusicLoop;

                bgMusic.Play();
        }
    }
}
