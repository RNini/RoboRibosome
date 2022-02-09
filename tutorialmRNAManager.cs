using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class tutorialmRNAManager : MonoBehaviour
{
    // Variables to store codon and anti-codon information
    public string[] codons;
        private string tempName;

        // Misc variables
        private string tempTileName;

    // Tiles to instantiate
    private GameObject tilePreFab;

        // Array of tile prefabs to pull from randomly
        public GameObject[] upTileList;

        // Variables
        private Vector3 tileLocation;
            public float[] xCoor;
            public float yCoor;

        // Stores the tiles for mRNA slots
        public List<GameObject> mRNATilesList = new List<GameObject>();
            public GameObject tempTile;

    // Variables for checking matches against codons and instantiating anticodon tiles
    public GameObject[] antiCodonTiles;
        public int antiCodonIndex;

    public GameObject[] downTileList;
        public float[] antiXCoor;
        public float antiYCoor;

    public GameObject playerCursor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializemRNA()
    {
        // Places tiles on board
        PopulatemRNA(0, 9, true);
    }

    public void PopulatemRNA(int startPos, int nBases, bool Initialize)
    {
        // Loop through mRNABoard
        for (int i = startPos; i < nBases; i++)
        {       
            
            tileLocation = new Vector3(xCoor[i], yCoor, 5);


            // Instanstiates a random tile at the calculated coordinates to the game board
            if (Initialize == true && i == 8)
            {
                tilePreFab = upTileList[2];         // First codon spot will gauranteed to be a guanine when initializing the fourth tutorial
            }

            else { tilePreFab = upTileList[Random.Range(0, upTileList.Length)]; }

            tempTile = (GameObject)Instantiate(tilePreFab, tileLocation, Quaternion.identity) as GameObject;

            // Stores the prefab in the tiles array
            mRNATilesList.Add(tempTile);

        }

        Translate();

    }

    public void frameshiftRNA()
    {
        // Keeps the list within bounds
        if (mRNATilesList.Count == 20)
        {
            for (int i = 0; i < 3; i++)
            {
                tempTile = mRNATilesList[19 - i];
                mRNATilesList.RemoveAt(19 - i);
                Destroy(tempTile);
            }
        }

            // Instantiates 3 new tiles
            for (int i = 0; i < 3; i++)
            {

                tileLocation = new Vector3(xCoor[i], yCoor, 5);
                tilePreFab = upTileList[Random.Range(0, upTileList.Length)];

                // Instanstiates a random tile at the calculated coordinates to the game board
                tempTile = (GameObject)Instantiate(tilePreFab, tileLocation, Quaternion.identity) as GameObject;

                // Stores the prefab in the tiles array
                mRNATilesList.Insert(i, tempTile);

            }

            // Shifts old tiles by 3 spots       
            for (int i = 0; i < mRNATilesList.Count - 3; i++)
            {
                tileLocation = new Vector3(xCoor[i + 3], yCoor, 5);
                tilePreFab = mRNATilesList[i + 3];

                StartCoroutine(SlideTile(tilePreFab, tileLocation));
            }

        Translate();

    }

    public void Translate()
    {

        // Loop through mRNABoard codon slots to read tile tags
        for (int i = 0; i < 3; i++)
        {
            codons[i] = mRNATilesList[8 - i].tag;
                        
        }

    }

        public void checkAntiCodon(string antiCodonLetter, int currentTutorial)
        {
            if (currentTutorial == 3)
            {
                // Matches for adenine
                if (antiCodonLetter == "Adenine")
                {
                    // When the tile is the complement, instantiate a tile in the appropriate slot
                    if (codons[antiCodonIndex] == "Uracil")
                    {
                        instantiateAntiCodon(0);
                    }

                    else { resetAntiCodons(); }
                }

                // Matches for cytosine
                else if (antiCodonLetter == "Cytosine")
                {
                    if (codons[antiCodonIndex] == "Guanine")
                    {
                        instantiateAntiCodon(1);
                    }

                    else { resetAntiCodons(); }
                }

                // Matches for guanine
                else if (antiCodonLetter == "Guanine")
                {
                    if (codons[antiCodonIndex] == "Cytosine")
                    {
                        instantiateAntiCodon(2);
                    }

                    else { resetAntiCodons(); }
                }

                // Matches for cytosine
                else if (antiCodonLetter == "Thymine")
                {
                    if (codons[antiCodonIndex] == "Adenine")
                    {
                        instantiateAntiCodon(3);
                    }

                    else { resetAntiCodons(); }
                }

            }   

        }

    public void instantiateAntiCodon(int letterIndex)
    {
        playerCursor.GetComponent<tutorialSFXManager>().matchSound(antiCodonIndex);

        // Parameter determination
        tileLocation = new Vector3(antiXCoor[antiCodonIndex], antiYCoor, 5);
        tilePreFab = downTileList[letterIndex];

        // Instanstiates a random tile at the calculated coordinates to the game board
        tempTile = (GameObject)Instantiate(tilePreFab, tileLocation, Quaternion.identity) as GameObject;

        // Stores the prefab in the tiles array
        antiCodonTiles[antiCodonIndex] = tempTile;

        // Sets the anticodon index after finishing
        if (antiCodonIndex == 2)
        {
            antiCodonIndex = 0;

            for (int i = 0; i < 3; i++)
            {
                Destroy(antiCodonTiles[i]);
            }

            frameshiftRNA();
        }

        else { antiCodonIndex++; }

        // Checks the tutorial boolean for completion

        if (playerCursor.GetComponent<tutorialMovementManager>().codonBool == false)
        {
            playerCursor.GetComponent<tutorialMovementManager>().codonBool = true;

            // Allows for progression to the next tutorial
            this.gameObject.GetComponent<tutorialManager>().tutorialButtons[4].interactable = true;

            // Updates tutorial goal text
            string text = 0 + " x";
            StartCoroutine(this.gameObject.GetComponent<tutorialManager>().updateTutTextProgress(true, 9, text));
        }
    }

    public void resetAntiCodons()
    {
        antiCodonIndex = 0;

        for (int i = 0; i < 3; i++)
        {
            Destroy(antiCodonTiles[i]);
        }

        playerCursor.GetComponent<tutorialSFXManager>().mismatchSound();
    }

    private IEnumerator SlideTile(GameObject tile, Vector3 targetPos)
    {
        // Initializes time and displacement variables
        float elapsedTime = 0;
        float timeToMove = playerCursor.GetComponent<tutorialMovementManager>().timeToMove;          // Borrows the time limit as defined by PlayerMovement in the inspector

        Vector3 origPos = tile.transform.position;

        while (elapsedTime < timeToMove)
        {
            // Moves the tile by linear interpolation (LERP)
            tile.transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));

            // Keeps track of time elapsed
            elapsedTime += Time.deltaTime;

            // Ends the while function
            yield return null;
        }

        // Insures that the tile makes it to the target position if there is an incidental offset
        tile.transform.position = targetPos;

    }

    public void clearmRNA()
    {
        int mRNACount = mRNATilesList.Count;

        Debug.Log("This is the count:" + mRNACount);
        
        if (mRNACount != 0)
        {
            for (int i = 0; i < mRNACount; i++)
            {
                int temp = mRNACount - i;
                
                Debug.Log("Deleting tile " + temp);

                tempTile = mRNATilesList[mRNACount - i - 1];
                mRNATilesList.RemoveAt(mRNACount - i - 1);
                Destroy(tempTile);
            }
        }
        
    }
}
