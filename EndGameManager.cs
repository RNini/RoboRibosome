using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    // Variables vital for transition animations
    public Animator transPanelAnimator;
        public Animator transParticlesAnimator;

    public Text flavorText;

    public Animator endGameHeader;
    
    // Variables for fun facts
    [TextArea]
    public string[] funFactText;
        public Text funFactTextbox;

    // Sprite and image variables
    public GameObject playerCharacter;
        public Image tileImage;
            public Sprite[] tileSprites;

    public string[] tileTypes;

    // Text variables for progress report
    public Text finalScoreText;
        public Text matchAccuracyText;
        public Text tileClearText;

    public int[] tempClearArray;

    private int matchCorrect;
        private int matchIncorrect;
        private float matchAccuracy;
    
    // Start is called before the first frame update
    void Start()
    {
        highScoreAnims();

        // Provides a random factoid
        funFactTextbox.text = funFactText[Random.Range(0, funFactText.Length)];

        // Plays flashy flash
        endGameHeader.Play("Text_Blink");

        updateEndText();

        Time.timeScale = 1;
                        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void highScoreAnims()
    {
        transPanelAnimator.Play("TransitionPanel_Hide");
        transParticlesAnimator.Play("TransitionParticles_Hide");
    }

    public void updateEndText()
    {     
        
        finalScoreText.text = "" + PlayerPrefs.GetInt("PlayerScore");

        // Calculations for match accuracy
        matchCorrect = PlayerPrefs.GetInt("MatchCorrect");
            matchIncorrect = PlayerPrefs.GetInt("MatchIncorrect");

            matchAccuracy = Mathf.FloorToInt(((float)matchCorrect / ((float)matchCorrect + (float)matchIncorrect)) * 100);

            matchAccuracyText.text = matchAccuracy + " %";

            Debug.Log("The number correct: " + matchCorrect);
                Debug.Log("The number incorrect:" + matchIncorrect);
            Debug.Log("The accuracy rate:" + (((float)matchCorrect / ((float)matchCorrect + (float)matchIncorrect)) * 100));

        // Logic for figuring out which tile was most cleared        
        for (int i = 0; i < 4; i++)
            {
                tempClearArray[i] = PlayerPrefs.GetInt("TilesCleared" + i);
            }

            // Out temp variables for storing the max and the corresponding int
            int maxClear = -1;
                int maxClearInt = 0;

            // Cycle through comparisons, reassigning the max and int when a new value is greater
            for (int i = 0; i < tempClearArray.Length; i++)
            {
                if (tempClearArray[i] > maxClear)
                {
                    maxClear = tempClearArray[i];
                        maxClearInt = i;
                }
            } 

            // set the text accordingly
            tileClearText.text = PlayerPrefs.GetInt("TilesCleared" + maxClearInt) + " " + tileTypes[maxClearInt] + " cleared";

                // Sets the image accordingly
                tileImage.sprite = tileSprites[maxClearInt];

        // Sets flavor text and calculates total number of cleared tiles
        int totalTiles = 0;

        for (int i = 0; i < tempClearArray.Length; i++)
        {
            totalTiles += tempClearArray[i];
        }

        flavorText.text = "Good job <color=#C1852D>" + PlayerPrefs.GetString("PlayerName") + "</color> unit." + "\n" +
            "\nThanks to your hard work and dedication, you were able to clear <color=#C1852D>" + totalTiles +
            "</color> nucleotides and translate mRNA to produce a peptide <color=#C1852D>" + PlayerPrefs.GetInt("AACounts") + "</color> amino acid(s) long!" +
            "\n" + "\nHere's a breakdown of your performance:";
    }
}
