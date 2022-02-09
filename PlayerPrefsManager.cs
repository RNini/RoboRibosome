using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsManager : MonoBehaviour
{
    // Variables for the robot parent objects and their animators
    public GameObject rp1;
        public string[] rp1Animations;
        private Animator rp1Animator;

    public GameObject rp2;
        public string[] rp2Animations;
        private Animator rp2Animator;

    public GameObject rp3;
        public string[] rp3Animations;
        private Animator rp3Animator;

    public int roboChasisIndex;
        public int roboAnimIndex;
    
    // Variables for player name
    public InputField nameInput;

    public Text flavorText;

    public void Start()
    {
        // Assign animator components
        rp1Animator = rp1.GetComponent<Animator>();
            rp2Animator = rp2.GetComponent<Animator>();
            rp3Animator = rp3.GetComponent<Animator>();

        roboChasisIndex = 0;
            roboAnimIndex = 0;
    }

    public void switchChassis()
    {
        if (roboChasisIndex == 0)
        {
            roboChasisIndex = 2;
        }

        else
        {
            roboChasisIndex--;
        }

        if (roboAnimIndex == 2)
        {
            roboAnimIndex = 0;
        }

        else
        {
            roboAnimIndex++;
        }

        // Plays animations to move robots clockwise
        rp1Animator.Play(rp1Animations[roboAnimIndex]);
            rp2Animator.Play(rp2Animations[roboAnimIndex]);
            rp3Animator.Play(rp3Animations[roboAnimIndex]);
    }

    public void setChasis()
    {
        PlayerPrefs.SetInt("PlayerChassis", roboChasisIndex);
    }

    public void setPlayerName()
    {
        PlayerPrefs.SetString("PlayerName", nameInput.text);
    }

    public void updateName()
    {
        flavorText.text = "Congratulations <color=#C1852D>" + nameInput.text + "</color> unit  on your recent hire to the RoboRibosome team. " +
            "We are excited to see you thrive in your new position. " + "\n" +
            "\nYour task is to make sure things run smoothly in the Ribosome. Please take the time to read the manual before you begin!";
    }

    
}
