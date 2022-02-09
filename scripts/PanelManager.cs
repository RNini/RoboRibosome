using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{

    // Keeps track of current and previous amino acids
    public GameObject[] aminoAcidText;

    // Time-keeping variables
    public GameObject timeText;
        public Color[] timeColor;

    public float minDigit;
        public float tenDigit;
        public float oneDigit;

    public float timeLimit;
        private float currTime;

    public bool gameOver;
    
    // Gameover Assets
    public GameObject gameOverPanel;
        public Animator gameOverAnimator;
    
    public GameObject[] gameOverText;
        public Animator[] gameOverTextAnimator;

    public GameObject transPanel;
        public Animator transPanelAnimator;

    public GameObject transParticles;
        public Animator transParticlesAnimator;

    // Used for keeping track of score
    public int score;                       // This will be changed with any match, depending on types of matches
        public int[] tileClear;             // Array of values for each tile type
        public int aaCount;                 // Increased on frameshift
        public int matchCount;              // This increases with each match

    // UI elements for displaying score
        public GameObject scoreText;

    // Start is called before the first frame update
    void Start()
    {       

        // Hides game over text
        gameOverTextAnimator[0].Play("Text_Hidden");
            gameOverTextAnimator[1].Play("Text_Hidden");

        // Plays panel hide animation
        gameOverAnimator.Play("GameOver_Hide");

        // Hides transition effects
        transPanelAnimator.Play("TransitionPanel_Hidden");
        transParticlesAnimator.Play("TransitionParticles_Hidden");

        gameOver = false;

        currTime = timeLimit;

    }

    private void Update()
    {        

        if (currTime <= 0)
        {
            timeText.GetComponent<Text>().text = "0 : 0 0";
            timeText.GetComponent<Text>().color = Color.Lerp(timeColor[0], timeColor[1], 0);

            
            Time.timeScale = 0;

            if (gameOver == false)
            {
                StartCoroutine(EndGame());

                gameOver = true;
            }

            if (transPanelAnimator.GetCurrentAnimatorStateInfo(0).IsName("TransitionPanel_Hide"))
            {
                this.gameObject.GetComponent<SceneLoader>().sceneChange();
            }

        }

        else
        {
            minDigit = Mathf.FloorToInt(currTime / 60);
                oneDigit = currTime % 60 % 10;
                tenDigit = (currTime % 60 - oneDigit) / 10;

            timeText.GetComponent<Text>().text = (int)minDigit + " : " + (int)tenDigit + " " + (int)oneDigit;

            currTime -= Time.deltaTime;

            // Adjusts color gradually as time passes
            timeText.GetComponent<Text>().color = Color.Lerp(timeColor[0], timeColor[1], currTime / timeLimit);
        }

    }

    public void scoreManager(int tileNumber1, int tileNumber2, int codonSlot)
    {
        // Gauranteed score based on number of tile matches
        float baseNumber = ((tileNumber1 + tileNumber2) / 2);
        
        // Bonus score awarded based on codon slot cleared
        float bonusScore = Mathf.Pow(((tileNumber1 + tileNumber2) / 2), codonSlot);

        // Final score added to rolling score
        score += Mathf.FloorToInt(baseNumber + bonusScore) * 350;

        // Updates the score text
        scoreText.GetComponent<Text>().text = "" + score;
        
    }

    public void tileCounter(int tileType1, int tileCount1, int tileType2, int tileCount2)
    {           
        // Adds the number of tiles cleared by type to the tile clear array
        tileClear[tileType1] += tileCount1;
            tileClear[tileType2] += tileCount2;

        // Increases the match count accordingly
        if (tileCount1 > 0)
        {
            matchCount++;
        }

            if (tileCount2 > 0)
            {
                matchCount++;
            }
    }

    public void aaCounter()
    {
        aaCount++;
    }

    IEnumerator EndGame()
    {
        // Writes player progress to Playerprefs
        PlayerPrefs.SetInt("PlayerScore", score);

            for (int i = 0; i < 4; i++)
            { 
                PlayerPrefs.SetInt("TilesCleared" + i, tileClear[i]);
            }

            PlayerPrefs.SetInt("AACounts", aaCount);

            PlayerPrefs.SetInt("MatchCount", matchCount);

        int matchCorrect = this.gameObject.GetComponent<mRNAManager>().matchCounter[0];
            int matchIncorrect = this.gameObject.GetComponent<mRNAManager>().matchCounter[1];

        PlayerPrefs.SetInt("MatchCorrect", matchCorrect);
            PlayerPrefs.SetInt("MatchIncorrect", matchIncorrect);

        // Transition animations
            // Unhides game over text
            gameOverTextAnimator[0].Play("Text_Shown");
                gameOverTextAnimator[1].Play("Text_Shown");

                // Shows the game over panel
                gameOverAnimator.Play("GameOver_Show");

                // Plays text blink animation
                gameOverTextAnimator[0].Play("Text_Blink");

        // Scene change
            // Wait before executing animations
            yield return new WaitForSecondsRealtime(2.5f);
                
            transPanelAnimator.Play("TransitionPanel_Show");
                transParticlesAnimator.Play("TransitionParticles_Show");
               
    }


}
