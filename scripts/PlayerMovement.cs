using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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

    // Variables for checking tile mathes
    public string[] slotCheck1;
        public bool slot1Match;
    public string[] slotCheck2;
        public bool slot2Match;

    void Start()
    {
        rotationIndex = 1;
            xIndex = 0;
            yIndex = 5;

        getCursorPoss(xIndex, yIndex2);
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

            if (xIndex >= 10)               //Upper-bound
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

            if (yIndex >= 5)                //Upper-bound
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

            if (xIndex >= 11)               //Upper-bound
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

            if (yIndex >= 5)                //Upper-bound
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

            if (xIndex >= 11)               //Upper-bound
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

            if (yIndex >= 5)                //Upper-bound
            {
                limitDown = true;
            }

                else limitDown = false;

            // Rotation boundaries
            if (yIndex >= 5)
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

            if (xIndex >= 11)               //Upper-bound
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

            if (yIndex >= 4)                //Upper-bound
            {
                limitDown = true;
            }

                else limitDown = false;

            // Rotation boundaries
            if (xIndex >= 11)
            {
                limitRotation = true;
            }

            else limitRotation = false;
        }

        // Limit movement when game is paused or ended, but allow animation to continue
        if (Time.timeScale > 0)
        {
            // Up movement
            if (Input.GetKey(KeyCode.UpArrow) & !isMoving & !limitUp)
            {
                StartCoroutine(MovePlayer(Vector3.up, "up"));
            }

            // Down movement
            if (Input.GetKey(KeyCode.DownArrow) & !isMoving & !limitDown)
            {
                StartCoroutine(MovePlayer(Vector3.down, "down"));
            }

            // Right movement
            if (Input.GetKey(KeyCode.RightArrow) & !isMoving & !limitRight)
            {
                StartCoroutine(MovePlayer(Vector3.right, "right"));
            }

            // Left movement
            if (Input.GetKey(KeyCode.LeftArrow) & !isMoving & !limitLeft)
            {
                StartCoroutine(MovePlayer(Vector3.left, "left"));
            }

            // Rotates cursor
            if (Input.GetKeyDown(KeyCode.LeftShift) & !isMoving & !limitRotation)
            {
                StartCoroutine(RotatePlayer());
            }

            // Switches selected tiles
            if (Input.GetKeyDown(KeyCode.Space) & !isMoving)
            {
                tile1 = boardManager.GetComponent<BoardManager>().boardTiles[xIndex, yIndex];
                tile2 = boardManager.GetComponent<BoardManager>().boardTiles[xIndex2, yIndex2];

                //StartCoroutine(MoveTile1(tile1, tile2));
                StartCoroutine(MoveTiles(tile1, tile2, rotationIndex));
            }

        }
        

    }

    // Function for moving the character cursor
    private IEnumerator MovePlayer(Vector3 direction, string dir)
    {
        isMoving = true;

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
            elapsedTime += Time.unscaledDeltaTime;

            // Ends the while function
            yield return null;
        }

        this.gameObject.GetComponent<sfxManager>().moveSound();

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
            elapsedTime += Time.unscaledDeltaTime;

            // Ends the while function
            yield return null;
        }

        // Insures that the player's cursor makes it to the target position if there is an incidental offset
        transform.rotation = targetQuat;

        isMoving = false;

        resetLimits();
        getCursorPoss(xIndex, yIndex);

        this.gameObject.GetComponent<sfxManager>().moveSound();
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

        boardManager.GetComponent<BoardManager>().boardTiles[xIndex, yIndex] = tile2;
        boardManager.GetComponent<BoardManager>().boardTiles[xIndex2, yIndex2] = tile1;

        // Sets current values to temporary variables
        int X1 = xIndex;                               
            int Y1 = yIndex;
            int X2 = xIndex2;
            int Y2 = yIndex2;

        // Pushes current values to check for matches
        checkMatches(rotIndex, X1, Y1, X2, Y2);

        isMoving = false;
    }

    public void checkMatches(int rotIndex, int X1, int Y1, int X2, int Y2)
    {
        // temp variables for checking tile counts
        int tileCount1 = 0;
            int tileType1;
        int tileCount2 = 0;
            int tileType2;

        // Variables for checking anticodon matching
        string codonCheck = null;
            int colStart = -1;
            int colEnd = -1;
            int rowStart = -1;
            int rowEnd = -1;

        if (rotIndex == 0) // Check rows
        {
            // Checks the first slot's row for matches
            slotCheck1 = new string[12];                    // Adjusts the array's size
                slot1Match = true;                         // Resets the slot1match bool to default

                // We load all the tags into our array
                for (int i = 0; i < slotCheck1.Length; i++)
                {
                    slotCheck1[i] = boardManager.GetComponent<BoardManager>().boardTiles[i, Y1].gameObject.tag;
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
            slotCheck2 = new string[12];                    // Resets the array
                slot2Match = true;                         // Resets the slot1match bool to default

                // We load all the tags into our array
                for (int i = 0; i < slotCheck2.Length; i++)
                {
                    slotCheck2[i] = boardManager.GetComponent<BoardManager>().boardTiles[i, Y2].gameObject.tag;
                }

                // And check for mismatches against the first tile
                for (int i = 1; i < slotCheck2.Length; i++)
                {
                    if (slotCheck2[0] != slotCheck2[i])
                    {
                        slot2Match = false;
                    }
                }



            // Tile management in the various match cases
            if (slot1Match && slot2Match == true)
            {
                colStart = 0;
                    colEnd = slotCheck1.Length;
                    rowStart = Y1;
                    rowEnd = Y2 + 1;

                codonCheck = slotCheck1[0];
            }

                    else if (slot1Match == true && slot2Match == false)
                    {
                        colStart = 0;
                            colEnd = slotCheck1.Length;
                            rowStart = Y1;
                            rowEnd = Y2;


                        codonCheck = slotCheck1[0];
                    }

                    else if (slot1Match == false && slot2Match == true)
                    {
                        colStart = 0;
                            colEnd = slotCheck1.Length;
                            rowStart = Y2;
                            rowEnd = Y2+1;

                            codonCheck = slotCheck2[0];
                    }
        }

        else if (rotIndex == 1) // Check columns
        {
            // Checks the first slot's row for matches
            slotCheck1 = new string[6];                      // Adjusts the array's size
                slot1Match = true;                          // Resets the slot1match bool to default

                // We load all the tags into our array
                for (int i = 0; i < slotCheck1.Length; i++)
                {
                    slotCheck1[i] = boardManager.GetComponent<BoardManager>().boardTiles[X1, i].gameObject.tag;
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
            slotCheck2 = new string[6];                      // Resets the array
            slot2Match = true;                              // Resets the slot1match bool to default

                // We load all the tags into our array
                for (int i = 0; i < slotCheck2.Length; i++)
                {
                    slotCheck2[i] = boardManager.GetComponent<BoardManager>().boardTiles[X2, i].gameObject.tag;
                }

                // And check for mismatches against the first tile
                for (int i = 1; i < slotCheck2.Length; i++)
                {
                    if (slotCheck2[0] != slotCheck2[i])
                    {
                        slot2Match = false;
                    }
                }

            // Tile management in the various match cases
            if (slot1Match && slot2Match == true)
            {
                colStart = X1;
                colEnd = X2 + 1;
                rowStart = 0;
                rowEnd = slotCheck1.Length;

                codonCheck = slotCheck1[0];
            }

                    else if (slot1Match == true && slot2Match == false)
                    {
                        colStart = X1;
                        colEnd = X2;
                        rowStart = 0;
                        rowEnd = slotCheck1.Length;

                        codonCheck = slotCheck1[0];
                    }

                    else if (slot1Match == false && slot2Match == true)
                    {
                        colStart = X2;
                        colEnd = X2 + 1;
                        rowStart = 0;
                        rowEnd = slotCheck1.Length;


                        codonCheck = slotCheck2[0];
                    }
        }

        if (rotIndex == 2) // Check rows
        {
            // Checks the first slot's row for matches
            slotCheck1 = new string[12];                     // Adjusts the array's size
            slot1Match = true;                              // Resets the slot1match bool to default

                // We load all the tags into our array
                for (int i = 0; i < slotCheck1.Length; i++)
                {
                    slotCheck1[i] = boardManager.GetComponent<BoardManager>().boardTiles[i, Y1].gameObject.tag;
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
            slotCheck2 = new string[12];                     // Resets the array
            slot2Match = true;                              // Resets the slot1match bool to default

                // We load all the tags into our array
                for (int i = 0; i < slotCheck2.Length; i++)
                {
                    slotCheck2[i] = boardManager.GetComponent<BoardManager>().boardTiles[i, Y2].gameObject.tag;
                }

                // And check for mismatches against the first tile
                for (int i = 1; i < slotCheck2.Length; i++)
                {
                    if (slotCheck2[0] != slotCheck2[i])
                    {
                        slot2Match = false;
                    }
                }

            // Tile management in the various match cases
            if (slot1Match && slot2Match == true)
            {
                colStart = 0;
                colEnd = slotCheck1.Length;
                rowStart = Y2;
                rowEnd = Y1 + 1;

                codonCheck = slotCheck1[0];                
            }

                    else if (slot1Match == true && slot2Match == false)
                    {
                        colStart = 0;
                        colEnd = slotCheck1.Length;
                        rowStart = Y1;
                        rowEnd = Y1 + 1;

                        codonCheck = slotCheck1[0];
                    }

                    else if (slot1Match == false && slot2Match == true)
                    {
                        colStart = 0;
                        colEnd = slotCheck1.Length;
                        rowStart = Y2;
                        rowEnd = Y2 + 1;

                        codonCheck = slotCheck2[0];
                    }
        }

        if (rotIndex == 3) // Check columns
        {
            // Checks the first slot's row for matches
            slotCheck1 = new string[6];                      // Adjusts the array's size
            slot1Match = true;                              // Resets the slot1match bool to default

                // We load all the tags into our array
                for (int i = 0; i < slotCheck1.Length; i++)
                {
                    slotCheck1[i] = boardManager.GetComponent<BoardManager>().boardTiles[X1, i].gameObject.tag;
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
            slotCheck2 = new string[6];                      // Resets the array
            slot2Match = true;                              // Resets the slot1match bool to default

                // We load all the tags into our array
                for (int i = 0; i < slotCheck2.Length; i++)
                {
                    slotCheck2[i] = boardManager.GetComponent<BoardManager>().boardTiles[X2, i].gameObject.tag;
                }

                // And check for mismatches against the first tile
                for (int i = 1; i < slotCheck2.Length; i++)
                {
                    if (slotCheck2[0] != slotCheck2[i])
                    {
                        slot2Match = false;
                    }
                }

            // Tile management in the various match cases
            if (slot1Match && slot2Match == true)
            {
                colStart = X1;
                colEnd = X2 + 1;
                rowStart = 0;
                rowEnd = slotCheck1.Length;

                codonCheck = slotCheck1[0];
            }

                    else if (slot1Match == true && slot2Match == false)
                    {
                        colStart = X1;
                        colEnd = X1 + 1;
                        rowStart = 0;
                        rowEnd = slotCheck1.Length;

                        codonCheck = slotCheck1[0];
                    }

                    else if (slot1Match == false && slot2Match == true)
                    {
                        colStart = X2;
                        colEnd = X2 + 1;
                        rowStart = 0;
                        rowEnd = slotCheck1.Length;

                        codonCheck = slotCheck2[0];
                    }
        }

        // These are only triggered if there are matches
        if (slot1Match == true || slot2Match == true)
        {
            // Functions for repopulating the board
            boardManager.GetComponent<BoardManager>().RepopulateBoard(colStart, colEnd, rowStart, rowEnd);

            // Sets appropriate tile match counts based on match results above
            if (slot1Match)
            {
                tileCount1 = slotCheck1.Length;
            }

                if (slot2Match)
                {
                    tileCount2 = slotCheck2.Length;
                }

            // Returns the appropriate indices for each slot
            if (slotCheck1[0] == "Adenine")
            {
                tileType1 = 0;
            }

                else if (slotCheck1[0] == "Cytosine")
                {
                    tileType1 = 1;
                }

                else if (slotCheck1[0] == "Guanine")
                {
                    tileType1 = 2;
                }

                else
                {
                    tileType1 = 3;
                }

            if (slotCheck2[0] == "Adenine")
            {
                tileType2 = 0;
            }

                else if (slotCheck2[0] == "Cytosine")
                {
                    tileType2 = 1;
                }

                else if (slotCheck2[0] == "Guanine")
                {
                    tileType2 = 2;
                }

                else
                {
                    tileType2 = 3;
                }

            // Adds to the tiles cleared counts
            boardManager.GetComponent<PanelManager>().tileCounter(tileType1, tileCount1, tileType2, tileCount2);

            // We push the current tile info to check anticodons, and carry over extra variables for scoring
            boardManager.GetComponent<mRNAManager>().checkAntiCodon(codonCheck, tileCount1, tileCount2);
        }
                       

    }

}
