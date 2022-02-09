using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialMovementManager : MonoBehaviour
{
    // Player variables
    public GameObject playerCursor;
        public float moveFactor;        // Used to correct displacement to fit along grid

    // Variables for clamping movement
    private bool isMoving;
        private Vector3 origPos;
        private Vector3 targetPos;

    // Bools for limiting movement out of bounds
    public bool limitUp;
        public bool limitDown;
        public bool limitLeft;
        public bool limitRight;

    // We declare a second position variable to prevent conflicts with our transform functions
    private Vector3 currPos;

    // Grid Indices
    public int xIndex;          // For keeping track of x movement
        public int yIndex;      // For keeping track of y movement

    public int xIndex2;         // This will be calculated based on current rotation
        public int yIndex2;     // This will be calculated based on current rotation

    // Variables for clamping rotation
    private bool isRotating;
        private Quaternion origQuat;
        private Quaternion targetQuat;

    public int rotationIndex;           // Keeps track of our orientation

    public bool limitRotation;          // Bool for limiting rotation so player cursor doesn't leave the board

    // Variables for determining movement speeds
    public float timeToMove;
        public float timeToRotate;

    // Variables for managing tile switching
    public GameObject boardManager;
        public GameObject tile1;
        public GameObject tile2;

    // Variables for checking tile matches
    public string[] slotCheck1;
        public bool slot1Match;
    
    public string[] slotCheck2;
        public bool slot2Match;

    // Animation variables
    public string[] keystrokes;
        public string currKeystroke;

    public GameObject keyboardSprite;

    // Tutorial limiters
    public int currentTutorial;
        
        // Tutorial bools will limit player progression until tutorial
        public bool moveBool;
            public bool[] moveGoals;            // Condition before setting move bool to true

        public bool switchBool;
            public int switchGoals;             // Conditions before setting switch bool to true
            public int rotateGoals;

        public bool matchBool;
        
        public bool codonBool;

        public Color[] inactiveButtonColors;

    // Tutorial checkpoint variables
    public bool[] tutorialComplete;

    void Start()
    {
        rotationIndex = 1;
        xIndex = 0;
        yIndex = 2;

        getCursorPoss(xIndex, yIndex2);

        currentTutorial = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // Checks that the player is within specified bounds before evaluating motion/rotation
        // When rotation is at 0º
        if (rotationIndex == 1)
        {
            // Lateral movement boundaries
            if (xIndex <= 0)                //Lower-bound
            {
                limitLeft = true;
            }

            else limitLeft = false;

            if (xIndex >= 6)               //Upper-bound
            {
                limitRight = true;
            }

            else limitRight = false;

            // Vertical movement boundaries
            if (yIndex <= 0)                //Lower-bound
            {
                limitUp = true;
            }

            else limitUp = false;

            if (yIndex >= 2)                //Upper-bound
            {
                limitDown = true;
            }

            else limitDown = false;

            // Rotation boundaries
            if (yIndex <= 0)
            {
                limitRotation = true;
            }

            else limitRotation = false;
        }

        // When rotation is at 90º
        if (rotationIndex == 2)
        {
            // Lateral movement boundaries
            if (xIndex <= 0)                //Lower-bound
            {
                limitLeft = true;
            }

            else limitLeft = false;

            if (xIndex >= 7)               //Upper-bound
            {
                limitRight = true;
            }

            else limitRight = false;

            // Vertical movement boundaries
            if (yIndex <= 1)                //Lower-bound
            {
                limitUp = true;
            }

            else limitUp = false;

            if (yIndex >= 2)                //Upper-bound
            {
                limitDown = true;
            }

            else limitDown = false;

            // Rotation boundaries
            if (xIndex <= 0)
            {
                limitRotation = true;
            }

            else limitRotation = false;
        }

        // When rotation is at 180º
        if (rotationIndex == 3)
        {
            // Lateral movement boundaries
            if (xIndex <= 1)                //Lower-bound
            {
                limitLeft = true;
            }

            else limitLeft = false;

            if (xIndex >= 7)               //Upper-bound
            {
                limitRight = true;
            }

            else limitRight = false;

            // Vertical movement boundaries
            if (yIndex <= 0)                //Lower-bound
            {
                limitUp = true;
            }

            else limitUp = false;

            if (yIndex >= 2)                //Upper-bound
            {
                limitDown = true;
            }

            else limitDown = false;

            // Rotation boundaries
            if (yIndex >= 2)
            {
                limitRotation = true;
            }

            else limitRotation = false;
        }

        // When rotation is at 270º
        if (rotationIndex == 0)
        {
            // Lateral movement boundaries
            if (xIndex <= 0)                //Lower-bound
            {
                limitLeft = true;
            }

            else limitLeft = false;

            if (xIndex >= 7)               //Upper-bound
            {
                limitRight = true;
            }

            else limitRight = false;

            // Vertical movement boundaries
            if (yIndex <= 0)                //Lower-bound
            {
                limitUp = true;
            }

            else limitUp = false;

            if (yIndex >= 1)                //Upper-bound
            {
                limitDown = true;
            }

            else limitDown = false;

            // Rotation boundaries
            if (xIndex >= 7)
            {
                limitRotation = true;
            }

            else limitRotation = false;
        }


        // Up movement
        if (Input.GetKey(KeyCode.UpArrow) & !isMoving & !limitUp)
        {
            // Updates tutorial goal text            
            if (!moveGoals[3])
            {
                string text = "Up";
                    StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(true, 2, text));
            }
            
            moveGoals[3] = true;

            StartCoroutine(MovePlayer(Vector3.up, "up"));
            
            currKeystroke = keystrokes[3];
                animateKeyboard(currKeystroke);
                        
        }

        // Down movement
        if (Input.GetKey(KeyCode.DownArrow) & !isMoving & !limitDown)
        {
            // Updates tutorial goal text            
            if (!moveGoals[2])
            {
                string text = "Down";
                    StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(true, 3, text));
            }

            moveGoals[2] = true;

            StartCoroutine(MovePlayer(Vector3.down, "down"));
            
            currKeystroke = keystrokes[2];
                animateKeyboard(currKeystroke);            
        }

        // Right movement
        if (Input.GetKey(KeyCode.RightArrow) & !isMoving & !limitRight)
        {
            // Updates tutorial goal text            
            if (!moveGoals[1])
            {
                string text = "Right";
                    StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(true, 1, text));
            }

            moveGoals[1] = true;

            StartCoroutine(MovePlayer(Vector3.right, "right"));
                
            currKeystroke = keystrokes[1];
                animateKeyboard(currKeystroke);            
        }

        // Left movement
        if (Input.GetKey(KeyCode.LeftArrow) & !isMoving & !limitLeft)
        {
            // Updates tutorial goal text            
            if (!moveGoals[0])
            {
                string text = "Left";
                    StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(true, 0, text));
            }

            moveGoals[0] = true;
            
            StartCoroutine(MovePlayer(Vector3.left, "left"));
            
            currKeystroke = keystrokes[0];
                animateKeyboard(currKeystroke);
        }

        // Rotates cursor
        if (Input.GetKeyDown(KeyCode.LeftShift) & !isMoving & !limitRotation & currentTutorial != 0)
        {
            StartCoroutine(RotatePlayer());
            
            currKeystroke = keystrokes[4];
                animateKeyboard(currKeystroke);
        }

        // Switches selected tiles
        if (Input.GetKeyDown(KeyCode.Space) & !isMoving & currentTutorial != 0)
        {
            tile1 = boardManager.GetComponent<tutorialManager>().boardTiles[xIndex, yIndex];
            tile2 = boardManager.GetComponent<tutorialManager>().boardTiles[xIndex2, yIndex2];

            //StartCoroutine(MoveTile1(tile1, tile2));
            StartCoroutine(MoveTiles(tile1, tile2, rotationIndex));
            
            currKeystroke = keystrokes[5];
                animateKeyboard(currKeystroke);
        }

    }

    // Function for moving the character cursor
    private IEnumerator MovePlayer(Vector3 direction, string dir)
    {
        isMoving = true;

        // Checks for satisfying move tutorial requirements
        if (moveGoals[0] && moveGoals[1] && moveGoals[2] && moveGoals[3] && !moveBool)
        {
            moveBool = true;
            
            // Allows for progression to the next tutorial
            boardManager.GetComponent<tutorialManager>().tutorialButtons[1].interactable = true;
        }

        // Adjusts the coordinate indices based on button input
        if (dir == "up")
        {
            yIndex--;
        }

        else if (dir == "down")
        {
            yIndex++;
        }

        else if (dir == "left")
        {
            xIndex--;
        }

        else if (dir == "right")
        {
            xIndex++;
        }

        // Initializes time and displacement variables
        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + (direction * moveFactor);

        while (elapsedTime < timeToMove)
        {
            // Moves the object in the direction by linear interpolation (LERP)
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));

            // Keeps track of time elapsed
            elapsedTime += Time.deltaTime;

            // Ends the while function
            yield return null;
        }

        this.gameObject.GetComponent<tutorialSFXManager>().moveSound();

        // Insures that the player's cursor makes it to the target position if there is an incidental offset
        transform.position = targetPos;

        isMoving = false;

        resetLimits();
        getCursorPoss(xIndex, yIndex);
    }

    // Function for moving the character cursor
    private IEnumerator RotatePlayer()
    {
        isMoving = true;

        float elapsedTime = 0;

        origQuat = transform.rotation;
        targetQuat = Quaternion.Euler(0, 0, (rotationIndex * 90));

        if (switchBool == false)
        {
            if (rotateGoals < 4)
            {
                rotateGoals++;

                // Updates tutorial goal text
                string text = " " + (4 - rotateGoals) + " x";
                    StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(false, 5, text));
            }

                if (rotateGoals >= 4)
                {
                    // Updates tutorial goal text
                    string text = " " + 0 + " x";
                    StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(true, 5, text));
                }

            if (switchGoals >= 5 && rotateGoals >= 4)
            {
                switchBool = true;                

                // Allows for progression to the next tutorial
                boardManager.GetComponent<tutorialManager>().tutorialButtons[2].interactable = true;
            }
        }
        

        // Updates rotation index after evaluating rotation transform
        if (rotationIndex < 3)
        {
            rotationIndex++;
        }

        else
        {
            rotationIndex = 0;
        }

        while (elapsedTime < timeToRotate)
        {
            // Moves the object in the direction by linear interpolation (LERP)
            transform.rotation = Quaternion.Slerp(origQuat, targetQuat, (elapsedTime / timeToRotate));

            // Keeps track of time elapsed
            elapsedTime += Time.deltaTime;

            // Ends the while function
            yield return null;
        }

        // Insures that the player's cursor makes it to the target position if there is an incidental offset
        transform.rotation = targetQuat;

        isMoving = false;

        resetLimits();
        getCursorPoss(xIndex, yIndex);

        this.gameObject.GetComponent<tutorialSFXManager>().moveSound();
    }

    public void resetLimits()
    {
        limitUp = false;
        limitDown = false;
        limitLeft = false;
        limitRight = false;

        limitRotation = false;
    }

    // Sets the values for cursor points to be used with tile switching
    public void getCursorPoss(int X1, int Y1)
    {
        // Slot 2 is below slot 1
        if (rotationIndex == 0)
        {
            xIndex2 = X1;
            yIndex2 = Y1 + 1;

        }

        // Slot 2 is to the right of slot 1
        else if (rotationIndex == 1)
        {
            xIndex2 = X1 + 1;
            yIndex2 = Y1;

        }

        // Slot 2 is above slot 1
        else if (rotationIndex == 2)
        {
            xIndex2 = X1;
            yIndex2 = Y1 - 1;

        }

        // Slot 2 to the left of slot 1
        else if (rotationIndex == 3)
        {
            xIndex2 = X1 - 1;
            yIndex2 = Y1;
        }

    }

    // Switches tiles that are currently selected by the player's cursor
    public IEnumerator MoveTiles(GameObject Tile1, GameObject Tile2, int rotIndex)
    {
        isMoving = true;

        float elapsedTime = 0;

        Vector3 tempTile1 = tile1.transform.position;       // Temporarily stores the transform of tile1
        Vector3 tempTile2 = tile2.transform.position;       // Temporarily stores the transform of tile1

        while (elapsedTime < timeToMove)
        {
            // Moves the object in the direction by linear interpolation (LERP)
            Tile2.transform.position = Vector3.Lerp(tempTile2, tempTile1, (elapsedTime / timeToMove));
            Tile1.transform.position = Vector3.Lerp(tempTile1, tempTile2, (elapsedTime / timeToMove));

            // Keeps track of time elapsed
            elapsedTime += Time.deltaTime;

            // Ends the while function
            yield return null;
        }

        // Insures that the player's cursor makes it to the target position if there is an incidental offset
        Tile2.transform.position = tempTile1;
        Tile1.transform.position = tempTile2;

        boardManager.GetComponent<tutorialManager>().boardTiles[xIndex, yIndex] = tile2;
        boardManager.GetComponent<tutorialManager>().boardTiles[xIndex2, yIndex2] = tile1;

        // Sets current values to temporary variables
        int X1 = xIndex;
        int Y1 = yIndex;
        int X2 = xIndex2;
        int Y2 = yIndex2;

        // Pushes current values to check for matches
        if ( currentTutorial == 2 || currentTutorial == 3)
        {
            checkMatches(rotIndex, X1, Y1, X2, Y2);
        }
        
        isMoving = false;

        // Sets tutorial bool to true after successful tile switching
        if (switchBool == false)
        {
            if (switchGoals < 5)
            {
                switchGoals++;

                // Updates tutorial goal text
                string text = " " + (5 - switchGoals) + " x";
                    StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(false, 7, text));
            }

                if (switchGoals >= 5)
                {
                    // Updates tutorial goal text
                    string text = " " + 0 + " x";
                    StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(true, 7, text));
                }

            if (switchGoals >= 5 && rotateGoals >=4)
            {
                switchBool = true;                

                // Allows for progression to the next tutorial
                boardManager.GetComponent<tutorialManager>().tutorialButtons[2].interactable = true;
            }
            
        }
    }

    public void checkMatches(int rotIndex, int X1, int Y1, int X2, int Y2)
    {
        if (rotIndex == 0) // Check rows
        {
            // Checks the first slot's row for matches
            slotCheck1 = new string[8];                    // Adjusts the array's size
            slot1Match = true;                         // Resets the slot1match bool to default

            // We load all the tags into our array
            for (int i = 0; i < slotCheck1.Length; i++)
            {
                slotCheck1[i] = boardManager.GetComponent<tutorialManager>().boardTiles[i, Y1].gameObject.tag;
            }

            // And check for mismatches against the first tile
            for (int i = 1; i < slotCheck1.Length; i++)
            {
                if (slotCheck1[0] != slotCheck1[i])
                {
                    slot1Match = false;
                }
            }

            // This is repeated for slot2's row            
            slotCheck2 = new string[8];                    // Resets the array
            slot2Match = true;                         // Resets the slot1match bool to default

            // We load all the tags into our array
            for (int i = 0; i < slotCheck2.Length; i++)
            {
                slotCheck2[i] = boardManager.GetComponent<tutorialManager>().boardTiles[i, Y2].gameObject.tag;
            }

            // And check for mismatches against the first tile
            for (int i = 1; i < slotCheck2.Length; i++)
            {
                if (slotCheck2[0] != slotCheck2[i])
                {
                    slot2Match = false;
                }
            }

                    // Sets tutorial bool to true after successful tile matching
                    if (slot1Match == true || slot2Match == true)
                    {                
                        if (matchBool == false)
                        {                                       
                            matchBool = true;
                                                 
                            // Allows for progression to the next tutorial
                            boardManager.GetComponent<tutorialManager>().tutorialButtons[3].interactable = true;

                            // Updates tutorial goal text
                            string text = 0 + " x";
                                StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(true, 8, text));
                        }
                    }

            // Tile management in the various match cases
            if (slot1Match && slot2Match == true)
            {
                int colStart = 0;
                int colEnd = slotCheck1.Length;
                int rowStart = Y1;
                int rowEnd = Y2 + 1;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck1[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }

            else if (slot1Match == true && slot2Match == false)
            {
                int colStart = 0;
                int colEnd = slotCheck1.Length;
                int rowStart = Y1;
                int rowEnd = Y2;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck1[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }

            else if (slot1Match == false && slot2Match == true)
            {
                int colStart = 0;
                int colEnd = slotCheck1.Length;
                int rowStart = Y2;
                int rowEnd = Y2 + 1;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck2[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }
        }

        else if (rotIndex == 1) // Check columns
        {
            // Checks the first slot's row for matches
            slotCheck1 = new string[3];                      // Adjusts the array's size
            slot1Match = true;                          // Resets the slot1match bool to default

            // We load all the tags into our array
            for (int i = 0; i < slotCheck1.Length; i++)
            {
                slotCheck1[i] = boardManager.GetComponent<tutorialManager>().boardTiles[X1, i].gameObject.tag;
            }

            // And check for mismatches against the first tile
            for (int i = 1; i < slotCheck1.Length; i++)
            {
                if (slotCheck1[0] != slotCheck1[i])
                {
                    slot1Match = false;
                }
            }

            // This is repeated for slot2's row            
            slotCheck2 = new string[3];                      // Resets the array
            slot2Match = true;                              // Resets the slot1match bool to default

            // We load all the tags into our array
            for (int i = 0; i < slotCheck2.Length; i++)
            {
                slotCheck2[i] = boardManager.GetComponent<tutorialManager>().boardTiles[X2, i].gameObject.tag;
            }

            // And check for mismatches against the first tile
            for (int i = 1; i < slotCheck2.Length; i++)
            {
                if (slotCheck2[0] != slotCheck2[i])
                {
                    slot2Match = false;
                }
            }

                    // Sets tutorial bool to true after successful tile matching
                    if (slot1Match == true || slot2Match == true)
                    {
                        if (matchBool == false)
                        {
                            matchBool = true;

                            // Allows for progression to the next tutorial
                            boardManager.GetComponent<tutorialManager>().tutorialButtons[3].interactable = true;

                            // Updates tutorial goal text
                            string text = 0 + " x";
                                StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(true, 8, text));
                        }
                    }

            // Tile management in the various match cases
            if (slot1Match && slot2Match == true)
            {
                int colStart = X1;
                int colEnd = X2 + 1;
                int rowStart = 0;
                int rowEnd = slotCheck1.Length;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck1[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }

            else if (slot1Match == true && slot2Match == false)
            {
                int colStart = X1;
                int colEnd = X2;
                int rowStart = 0;
                int rowEnd = slotCheck1.Length;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck1[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }

            else if (slot1Match == false && slot2Match == true)
            {
                int colStart = X2;
                int colEnd = X2 + 1;
                int rowStart = 0;
                int rowEnd = slotCheck1.Length;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck2[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }
        }

        if (rotIndex == 2) // Check rows
        {
            // Checks the first slot's row for matches
            slotCheck1 = new string[8];                     // Adjusts the array's size
            slot1Match = true;                              // Resets the slot1match bool to default

            // We load all the tags into our array
            for (int i = 0; i < slotCheck1.Length; i++)
            {
                slotCheck1[i] = boardManager.GetComponent<tutorialManager>().boardTiles[i, Y1].gameObject.tag;
            }

            // And check for mismatches against the first tile
            for (int i = 1; i < slotCheck1.Length; i++)
            {
                if (slotCheck1[0] != slotCheck1[i])
                {
                    slot1Match = false;
                }
            }

            // This is repeated for slot2's row            
            slotCheck2 = new string[8];                     // Resets the array
            slot2Match = true;                              // Resets the slot1match bool to default

            // We load all the tags into our array
            for (int i = 0; i < slotCheck2.Length; i++)
            {
                slotCheck2[i] = boardManager.GetComponent<tutorialManager>().boardTiles[i, Y2].gameObject.tag;
            }

            // And check for mismatches against the first tile
            for (int i = 1; i < slotCheck2.Length; i++)
            {
                if (slotCheck2[0] != slotCheck2[i])
                {
                    slot2Match = false;
                }
            }

                    // Sets tutorial bool to true after successful tile matching
                    if (slot1Match == true || slot2Match == true)
                    {
                        if (matchBool == false)
                        {
                            matchBool = true;

                            // Allows for progression to the next tutorial
                            boardManager.GetComponent<tutorialManager>().tutorialButtons[3].interactable = true;

                            // Updates tutorial goal text
                            string text = 0 + " x";
                                StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(true, 8, text));
                        }
                    }

            // Tile management in the various match cases
            if (slot1Match && slot2Match == true)
            {
                int colStart = 0;
                int colEnd = slotCheck1.Length;
                int rowStart = Y2;
                int rowEnd = Y1 + 1;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck1[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }

            else if (slot1Match == true && slot2Match == false)
            {
                int colStart = 0;
                int colEnd = slotCheck1.Length;
                int rowStart = Y1;
                int rowEnd = Y1 + 1;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck1[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }

            else if (slot1Match == false && slot2Match == true)
            {
                int colStart = 0;
                int colEnd = slotCheck1.Length;
                int rowStart = Y2;
                int rowEnd = Y2 + 1;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck2[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }
        }

        if (rotIndex == 3) // Check columns
        {
            // Checks the first slot's row for matches
            slotCheck1 = new string[3];                      // Adjusts the array's size
            slot1Match = true;                              // Resets the slot1match bool to default

            // We load all the tags into our array
            for (int i = 0; i < slotCheck1.Length; i++)
            {
                slotCheck1[i] = boardManager.GetComponent<tutorialManager>().boardTiles[X1, i].gameObject.tag;
            }

            // And check for mismatches against the first tile
            for (int i = 1; i < slotCheck1.Length; i++)
            {
                if (slotCheck1[0] != slotCheck1[i])
                {
                    slot1Match = false;
                }
            }

            // This is repeated for slot2's row            
            slotCheck2 = new string[3];                      // Resets the array
            slot2Match = true;                              // Resets the slot1match bool to default

            // We load all the tags into our array
            for (int i = 0; i < slotCheck2.Length; i++)
            {
                slotCheck2[i] = boardManager.GetComponent<tutorialManager>().boardTiles[X2, i].gameObject.tag;
            }

            // And check for mismatches against the first tile
            for (int i = 1; i < slotCheck2.Length; i++)
            {
                if (slotCheck2[0] != slotCheck2[i])
                {
                    slot2Match = false;
                }
            }

                    // Sets tutorial bool to true after successful tile matching
                    if (slot1Match == true || slot2Match == true)
                    {
                        if (matchBool == false)
                        {
                            matchBool = true;

                            // Allows for progression to the next tutorial
                            boardManager.GetComponent<tutorialManager>().tutorialButtons[3].interactable = true;

                            // Updates tutorial goal text
                            string text = 0 + " x";
                                StartCoroutine(boardManager.GetComponent<tutorialManager>().updateTutTextProgress(true, 8, text));
                        }
                    }

            // Tile management in the various match cases
            if (slot1Match && slot2Match == true)
            {
                int colStart = X1;
                int colEnd = X2 + 1;
                int rowStart = 0;
                int rowEnd = slotCheck1.Length;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck1[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }

            else if (slot1Match == true && slot2Match == false)
            {
                int colStart = X1;
                int colEnd = X1 + 1;
                int rowStart = 0;
                int rowEnd = slotCheck1.Length;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck1[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }

            else if (slot1Match == false && slot2Match == true)
            {
                int colStart = X2;
                int colEnd = X2 + 1;
                int rowStart = 0;
                int rowEnd = slotCheck1.Length;

                boardManager.GetComponent<tutorialmRNAManager>().checkAntiCodon(slotCheck2[0], currentTutorial);

                boardManager.GetComponent<tutorialManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);
            }
        }

    }

    public void animateKeyboard(string keyboardAnim)
    {
        keyboardSprite.GetComponent<Animator>().Play(keyboardAnim);
    }

}
