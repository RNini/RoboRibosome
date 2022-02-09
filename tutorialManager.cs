using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialManager : MonoBehaviour
{
    // Variables for the various animations
    public GameObject keyboardObject;
    private Animator tutorialAnimator;

    public GameObject tvChangeObject;
    private Animator tvAnimator;

    public string[] keysAnims;

    private int animationIndex;                 // Specifies which animations to play
    private bool transitionBool;            // Keeps track of transition animation progress   

    // Variables for the text box
    public GameObject instructionTextBox;
    [TextArea]                              // Apparently this is needed to allow for line breaks
    public string[] instructionText;

        public GameObject tutorialTasks;            // Object for task description
            [TextArea]
            public string[] tutorialTasksText;      // Text content

        public GameObject[] tutorialTracker;        // Text boxes for keeping goal info
            public string[] tutorialTrackerText;    // Text content

            public bool[] currTracker;                  // Helps keep track of currently used trackers for animating b/w tutorials
                public Color[] currColor;               // Variable for keeping colors consistent after changes

            public Color[] tutTextDoneColor;          // Completed tasks change to this color

    public Color[] textColors;

    // Variables for button functions
    public Button[] tutorialButtons;

    // Variables for instantiation
    /*public GameObject[,] moveTiles;
    public GameObject[,] switchTiles;
    public GameObject[,] matchingTiles;
    public GameObject[,] codonTiles;*/

    // Array of tiles to pull from
    public GameObject[] tileList;
    public GameObject[] matchTutList;
    public GameObject[] codonTutList;

    // Variables
    public Vector3 tileSlot1;
    public Vector3 tileSlot2;

    public float[] xCoor;
    public float[] yCoor;
    public float[] zCoor;

    private float tempX;
    private float tempY;
    private float tempZ;

    [SerializeField] private GameObject tilePreFab = null;
    public GameObject nuTile;

    // Array to keep track of instantiated tiles
    public GameObject[,] boardTiles;        // Board Tiles 2D array
        private int boardWidth;             // Initialize to 3   
        private int boardHeight;            // Initialize to 8

    // Variables to for animating tiles
    private GameObject oldTile;

    private Color origColor;
    private Color tempColor;

    public bool isAnimating;

    // Variables for initiating assets for each panel
    public int tutInt;
    public bool tutSwitchBool;

    // For accessing the cursor
    public GameObject playerCursor;

    // Sets the default state of the tutorial skip preference if it doesn't exist
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("TutorialSkip"))
        {
            PlayerPrefs.SetInt("TutorialSkip", 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables to default state
        transitionBool = false;
            tutSwitchBool = false;

        tutInt = 0;

        // Assigns the tutorial panel's animator
        tutorialAnimator = keyboardObject.GetComponent<Animator>();
        tvAnimator = tvChangeObject.GetComponent<Animator>();

        // Sets the default text
        instructionTextBox.GetComponent<Text>().text = instructionText[0];

        // Instantiate board arrays
        boardWidth = 8;
        boardHeight = 3;

        // Sets the dimensions of the 2D array and populates the board
        boardTiles = new GameObject[boardWidth, boardHeight];
        //PopulateBoard(boardWidth, boardHeight, boardTiles, tileList, true);

        // Start with the moving tutorial
        movingTutorial();
    }

    
    public void movingTutorial()
    {
        StartCoroutine(updateText(0));
            StartCoroutine(animSequence(0));

        for (int i = 0; i < tutorialButtons.Length; i++)
        {
            // Base interactability if all tutorial goals are met
            if (i == 0)
            {
                tutorialButtons[i].interactable = false;
            }

            else
            {
                tutorialButtons[i].interactable = true;
            }                     
                        
        }

        // Limits button interactability based on tutorial progress or player preference to skip tutorial
        if (playerCursor.GetComponent<tutorialMovementManager>().moveBool == false && PlayerPrefs.GetInt("TutorialSkip") == 0)
        {
            for (int i = 1; i < tutorialButtons.Length; i++)
            {
                tutorialButtons[i].interactable = false;
            }

            // Creates a temporary color variable
            Color disabledColor = playerCursor.GetComponent<tutorialMovementManager>().inactiveButtonColors[0];
                        
            for (int i = 1; i < tutorialButtons.Length; i++)
            {
                resetButtonColors(disabledColor, tutorialButtons[i]);                                
            }
        }            

        // Switches board content
        removeOld(boardWidth, boardHeight, tutInt);
        PopulateBoard(boardWidth, boardHeight, boardTiles, tileList, true);

        // Sets the tutorial integer
        tutInt = 0;
            playerCursor.GetComponent<tutorialMovementManager>().currentTutorial = tutInt;
    }

    public void switchingTutorial()
    {        
        
        StartCoroutine(updateText(1));
            StartCoroutine(animSequence(1));

        // Base button interactability
        for (int i = 0; i < tutorialButtons.Length; i++)
        {
            if (i == 1)
            {
                tutorialButtons[i].interactable = false;
            }

            else
            {
                tutorialButtons[i].interactable = true;
            }
        }

            // Button interactability gnostic to tutorial progress
            if (playerCursor.GetComponent<tutorialMovementManager>().switchBool == false && PlayerPrefs.GetInt("TutorialSkip") == 0)
            {
                // Creates a temporary color variable
                Color disabledColor = playerCursor.GetComponent<tutorialMovementManager>().inactiveButtonColors[1];
                    resetButtonColors(disabledColor, tutorialButtons[1]);

                for (int i = 2; i < tutorialButtons.Length; i++)
                {
                    tutorialButtons[i].interactable = false;
                }                
            }        

        // Replaces board tiles
        removeOld(boardWidth, boardHeight, tutInt);
            PopulateBoard(boardWidth, boardHeight, boardTiles, tileList, true);

        // Sets the tutorial integer
        tutInt = 1;
            playerCursor.GetComponent<tutorialMovementManager>().currentTutorial = tutInt;
    }

    public void matchingTutorial()
    {
        
        StartCoroutine(updateText(2));
            StartCoroutine(animSequence(2));

        // Base button interactability
        for (int i = 0; i < tutorialButtons.Length; i++)
        {
            if (i == 2)
            {
                tutorialButtons[i].interactable = false;
            }

            else
            {
                tutorialButtons[i].interactable = true;
            }
        }

            // Button interactability gnostic to tutorial progress
            if (playerCursor.GetComponent<tutorialMovementManager>().matchBool == false && PlayerPrefs.GetInt("TutorialSkip") == 0)
            {
                // Creates a temporary color variable
                Color disabledColor = playerCursor.GetComponent<tutorialMovementManager>().inactiveButtonColors[1];
                    resetButtonColors(disabledColor, tutorialButtons[2]);

            for (int i = 3; i < tutorialButtons.Length; i++)
                {
                    tutorialButtons[i].interactable = false;
                }
            }
                    

        // Replaces board tiles
        removeOld(boardWidth, boardHeight, tutInt);
            PopulateBoard(boardWidth, boardHeight, boardTiles, matchTutList, false);

        // Sets the tutorial integer
        tutInt = 2;
            playerCursor.GetComponent<tutorialMovementManager>().currentTutorial = tutInt;
    }

    public void codonsTutorial()
    {
        
        StartCoroutine(updateText(3));
            StartCoroutine(animSequence(3));

        // Base button interactability
        for (int i = 0; i < tutorialButtons.Length; i++)
        {
            if (i == 3)
            {
                tutorialButtons[i].interactable = false;
            }

            else
            {
                tutorialButtons[i].interactable = true;
            }
        }

            // Button interactability gnostic to tutorial progress
            if (playerCursor.GetComponent<tutorialMovementManager>().codonBool == false && PlayerPrefs.GetInt("TutorialSkip") == 0)
            {
                // Creates a temporary color variable
                Color disabledColor = playerCursor.GetComponent<tutorialMovementManager>().inactiveButtonColors[1];
                    resetButtonColors(disabledColor, tutorialButtons[3]);

                for (int i = 4; i < tutorialButtons.Length; i++)
                {
                    tutorialButtons[i].interactable = false;
                }
            }

        // Replaces the board tiles
        removeOld(boardWidth, boardHeight, tutInt);
            PopulateBoard(boardWidth, boardHeight, boardTiles, codonTutList, false);

        // Sets the tutorial integer
        tutInt = 3;
            playerCursor.GetComponent<tutorialMovementManager>().currentTutorial = tutInt;
    }

    public IEnumerator animSequence(int animIndex)
    {
        tvAnimator.Play("TVChange");

        while (transitionBool == false)
        {
            // Checks if the transition animation has finished
            if (tvAnimator.GetCurrentAnimatorStateInfo(0).IsName("TVChange_Idle"))
            {
                transitionBool = true;
            }
            yield return null;
        }

        //tutorialAnimator.Play(keysAnims[animIndex]);
        //tutorialAnimator.Play(tilesAnims[animIndex]);
    }

    public IEnumerator updateText(int textIndex)
    {        
        // Fade out text
        float elapsedTime = 0;

        while (elapsedTime < .25f)
        {            
            // Instructions box fade-out
            instructionTextBox.GetComponent<Text>().color = Color.Lerp(textColors[0], textColors[1], elapsedTime / .25f);
            
            // TutorialTracker fade-out
            for (int i = 0; i < currTracker.Length; i++)
            {
                if (currTracker[i] == true)
                {
                    tutorialTracker[i].GetComponent<Text>().color = Color.Lerp(currColor[i], tutTextDoneColor[1], elapsedTime / .25f);
                }
            }
            
            elapsedTime += Time.deltaTime;
            yield return null;

        }

            // Insures the boxes are faded out all the way
            instructionTextBox.GetComponent<Text>().color = textColors[1];

                // TutorialTracker fade-out
                for (int i = 0; i < currTracker.Length; i++)
                {
                    if (currTracker[i] == true)
                    {
                        tutorialTracker[i].GetComponent<Text>().color = tutTextDoneColor[1];
                    }
                }

        // Assign new tutorial trackers and corresponding text
        // Tutorial tracker info for the move tutorial
        if (textIndex == 0)
        {
            // Sets all tracker text boxes to false, clears their text
            for (int i = 0; i < tutorialTracker.Length; i++)
            {
                currTracker[i] = false;
                tutorialTracker[i].GetComponent<Text>().text = null;
            }

            // Activates the current trackers
            for (int i = 0; i < 4; i++)
            {
                currTracker[i] = true;
                    tutorialTracker[i].GetComponent<Text>().text = tutorialTrackerText[i];
            }

                for (int i = 4; i < 9; i++)
                {
                    currTracker[i] = false;
                        tutorialTracker[i].GetComponent<Text>().text = null;
                }
        }
        
            // Tutorial tracker info for the switch tutorial
            else if (textIndex == 1)
            {
                // Sets all tracker text boxes to false, clears their text
                for (int i = 0; i < tutorialTracker.Length; i++)
                {
                    currTracker[i] = false;
                    tutorialTracker[i].GetComponent<Text>().text = null;
                }

                    // Activates the current trackers
                    for (int i = 4; i < 8; i++)
                    {
                        currTracker[i] = true;
                    }

                tutorialTracker[4].GetComponent<Text>().text = tutorialTrackerText[4];
                        tutorialTracker[5].GetComponent<Text>().text = " " + (4 - playerCursor.GetComponent<tutorialMovementManager>().rotateGoals) + " x";
                
                    tutorialTracker[6].GetComponent<Text>().text = tutorialTrackerText[5];
                        tutorialTracker[7].GetComponent<Text>().text = " " + (5 - playerCursor.GetComponent<tutorialMovementManager>().switchGoals) + " x";         
            }

            // Tutorial tracker info for the match tutorial
            else if (textIndex == 2)
            {                                           
                // Sets all tracker text boxes to false, clears their text
                for (int i = 0; i < tutorialTracker.Length; i++)
                {
                    currTracker[i] = false;
                    tutorialTracker[i].GetComponent<Text>().text = null;
                }

                // Activates the current trackers
                currTracker[8] = true;

                // Sets number of matches needed
                if (playerCursor.GetComponent<tutorialMovementManager>().matchBool == false)
                {
                    tutorialTracker[8].GetComponent<Text>().text = 1 + " x";
                }

                else { tutorialTracker[8].GetComponent<Text>().text = 0 + " x"; }
            }

            // Tutorial tracker info for the codon tutorial
            else if (textIndex == 3)
            {
                // Sets other tracker text boxes to false, clears their text
                for (int i = 0; i < tutorialTracker.Length; i++)
                {
                    currTracker[i] = false;
                    tutorialTracker[i].GetComponent<Text>().text = null;
                }

                currTracker[9] = true;
                
                // Sets number of codon matches needed
                if (playerCursor.GetComponent<tutorialMovementManager>().codonBool == false)
                {
                    tutorialTracker[9].GetComponent<Text>().text = 1 + " x";
                }

                    else { tutorialTracker[9].GetComponent<Text>().text = 0 + " x"; }                
            }

        // Set new instruction text
        instructionTextBox.GetComponent<Text>().text = instructionText[textIndex];

            // Set new goal text
            tutorialTasks.GetComponent<Text>().text = tutorialTasksText[textIndex];
                tutorialTasks.GetComponent<Animator>().Play("Text_Blink");
                

        // Fade in new text
        elapsedTime = 0;

        while (elapsedTime < .25f)
        {
            // Instructions fade-in
            instructionTextBox.GetComponent<Text>().color = Color.Lerp(textColors[1], textColors[0], elapsedTime / .25f);

                // TutorialTracker fade-in
                for (int i = 0; i < currTracker.Length; i++)
                {
                    if (currTracker[i] == true)
                    {
                        tutorialTracker[i].GetComponent<Text>().color = Color.Lerp(tutTextDoneColor[1], currColor[i], elapsedTime / .25f);
                    }
                }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Insures the boxes are faded out all the way
        instructionTextBox.GetComponent<Text>().color = textColors[0];

        // TutorialTracker fade-in
        for (int i = 0; i < currTracker.Length; i++)
        {
            if (currTracker[i] == true)
            {
                tutorialTracker[i].GetComponent<Text>().color = currColor[i];
            }
        }

    }

    public void PopulateBoard(int Width, int Height, GameObject[,] board, GameObject[] tileArray, bool randomizeLoad)
    {

        // Loop through columns
        for (int i = 0; i < Height; i++)
        {
            // Loop through rows
            for (int j = 0; j < Width; j++)
            {
                // We randomize the tiles
                if (randomizeLoad == true)
                {
                    tileSlot1 = new Vector3(xCoor[j], yCoor[i], 5);
                    tilePreFab = tileList[Random.Range(0, tileList.Length)];

                    // Instanstiates a random tile at the calculated coordinates to the game board
                    nuTile = Instantiate(tilePreFab, tileSlot1, new Quaternion(0, 0, 0, 0));         
                }
                                
                // We pull from a pre-determined list
                else
                {
                    tileSlot1 = new Vector3(xCoor[j], yCoor[i], 5);
                    tilePreFab = tileArray[(8 * i) + j];

                    // Instanstiates a random tile at the calculated coordinates to the game board
                    nuTile = Instantiate(tilePreFab, tileSlot1, new Quaternion(0, 0, 0, 0));                     
                }

                // Stores the prefab in the 2D array for future use
                board[j, i] = nuTile;

            }

        }

    }

    public void removeOld(int Width, int Height, int tutInt)
    {

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Destroy(boardTiles[j, i]);
            }
        }
                      
    }

    public void RepopulateBoard(int colStart, int colEnd, int rowStart, int rowEnd)
    {

        // Loop through columns
        for (int i = colStart; i < colEnd; i++)
        {
            // Loop through rows
            for (int j = rowStart; j < rowEnd; j++)
            {

                tileSlot1 = new Vector3(xCoor[i], yCoor[j], 5);
                oldTile = boardTiles[i, j];

                tilePreFab = tileList[Random.Range(0, tileList.Length)];

                StartCoroutine(DestroyTiles(oldTile, tilePreFab, tileSlot1, i, j));

            }

        }

    }

    private IEnumerator DestroyTiles(GameObject old, GameObject nu, Vector3 Pos, int i, int j)
    {

        // Starts the animation and sets isAnimating to true
        old.GetComponent<Animator>().Play("Tile_FadeOut");
        isAnimating = true;

        // While loop to insure animation finishes before evaluating the rest of the function
        while (isAnimating == true)
        {
            // Ends the while loop when animation finishes
            if (old.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Tile_Hidden"))
            {
                isAnimating = false;
            }

            yield return null;
        }

        // At the end of the animation, destroy the tile
        Destroy(old);

        // Instanstiates a random tile at the calculated coordinates to the game board
        nuTile = Instantiate(nu, Pos, new Quaternion(0, 0, 0, 0));

        boardTiles[i, j] = nuTile;

        // Fades in the tiles
        nu.GetComponent<Animator>().Play("Tile_FadeIn");

    }

    public void resetButtonColors(Color inactiveColor, Button button)
    {
        ColorBlock cb = button.colors;
        cb.disabledColor = inactiveColor;
        button.colors = cb;
    }

    public IEnumerator updateTutTextProgress(bool colorSwitch, int textBoxInt, string updatedText)
    {
        tutorialTracker[textBoxInt].GetComponent<Text>().text = updatedText;
            
        if (colorSwitch == true)
        {
            float elapsedTime = 0f;                      

            while (elapsedTime < .25f)
            {
                elapsedTime += Time.deltaTime;

                Color nuColor = Color.Lerp(tutTextDoneColor[0], tutTextDoneColor[2], elapsedTime / 0.25f);
                    tutorialTracker[textBoxInt].GetComponent<Text>().color = nuColor;

                currColor[textBoxInt] = nuColor;
            }

            yield return null;
        }
    }
}
