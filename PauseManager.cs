using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Keeps track of pause state
    public bool pauseBool;

    // UI animation-related variables
    public Animator pausePanel;

    // Helps accomodate for multiple inputs
    public float keyDelay = 3f;  // 3 seconds
        private float timePassed = 0f;

    // Music variables
    public GameObject bgMusicManager;
        public GameObject sfxManager;

    // Start is called before the first frame update
    void Start()
    {
        // Initializes the pause bool
        pauseBool = false;

        timePassed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.unscaledTime;

        // Will only check input if at least 3 seconds have passed since last input
        if (timePassed > keyDelay)
        {
            
            // Actions for when the game is unpaused and the player presses escape
            if (Input.GetKeyDown(KeyCode.Escape) && pauseBool == false)
            {
                Time.timeScale = 0;
                pauseMenuRoutine();
            }

            // Actions for when the game is paused and the player presses escape
            else if (Input.GetKeyDown(KeyCode.Escape) && pauseBool == true)
            {
                Time.timeScale = 1;
                unpauseMenuRoutine();       
            }

            // resets the delay timer
            timePassed = 0f;
        }
        
    }

    public void pauseMenuRoutine()
    {
        // Shows the pause panel
        pausePanel.Play("Panel_Show");

        // Reset the pause bool
        pauseBool = true;
    }

    public void  unpauseMenuRoutine()
    {
        // Shows the pause panel
        pausePanel.Play("Panel_Hide");

        // Reset the pause bool
        pauseBool = false;
    }

}
