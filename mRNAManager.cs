using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class mRNAManager : MonoBehaviour
{
    // Variables to store codon and anti-codon information
    public string[] codons;
        private string tempName;

        // Arrays for AA names and 3-letter codes
        public int aaIndex;
            public int codonIndex;

        public string[] aminoAcid;
            public string[] aaCode;
            public string[] aaLetter;

        public string[] aaCodon;

        // Variables for setting text
        public GameObject aminoAcidText;
            public GameObject aaCodeText;
            public GameObject[] aaLetterText;
            public List<string> aaLettersList = new List<string>();     // We'll keep track of aa letter codes here, to facilitate frameshifts

        public GameObject codonText;

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

    public int[] matchCounter;

    // Start is called before the first frame update
    void Start()
    {
        // Place tiles on board
        PopulatemRNA(0,14);

        // Initializes variables
        aaIndex = -1;
            codonIndex = -1;

    }

    public void PopulatemRNA(int startPos, int nBases)
    {
        // Loop through mRNABoard
        for (int i = startPos; i < nBases; i++)
        {

            tileLocation = new Vector3(xCoor[i], yCoor, 5);
                tilePreFab = upTileList[Random.Range(0, upTileList.Length)];

            // Instanstiates a random tile at the calculated coordinates to the game board
            tempTile = (GameObject)Instantiate(tilePreFab, tileLocation, Quaternion.identity) as GameObject;

            // Stores the prefab in the tiles array
            mRNATilesList.Add(tempTile);
                       
        }

        Translate();
       
    }

    // Congrats, you coded this beast!
    public void frameshiftRNA()
    {
        // Keeps the list within bounds
        if (mRNATilesList.Count == 29)
        {
            for (int i = 0; i < 3; i++)
            {
                tempTile = mRNATilesList[28 - i];
                    mRNATilesList.RemoveAt(28 - i);
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
                    tileLocation = new Vector3(xCoor[i+3], yCoor, 5);
                        tilePreFab = mRNATilesList[i+3];                    

                // Instanstiates a random tile at the calculated coordinates to the game board
                //tilePreFab.transform.position = tileLocation;

                StartCoroutine(SlideTile(tilePreFab, tileLocation));

            }
        
        Translate();

        // Adds to aaCounts
        this.gameObject.GetComponent<PanelManager>().aaCounter();

    }

    public void Translate()
    {

        // Loop through mRNABoard codon slots to read tile tags
        for (int i = 0; i < 3; i++)
        {
            codons[i] = mRNATilesList[13 - i].tag;
                       
        }

        // Nested IFs to grab indices for other amino-acid info
        // First Letter U
        if (codons[0] == "Uracil")
        {

            // Second Letter U
            if (codons[1] == "Uracil")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 0;
                    codonIndex = 0;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 0;
                    codonIndex = 1;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 1;
                    codonIndex = 2;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 1;
                    codonIndex = 3;
                }

            }

            // Second Letter C
            else if (codons[1] == "Cytosine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 5;
                    codonIndex = 4;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 5;
                    codonIndex = 5;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 5;
                    codonIndex = 6;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 5;
                    codonIndex = 7;
                }

            }

            // Second Letter A
            else if (codons[1] == "Adenine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 9;
                    codonIndex = 8;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 9;
                    codonIndex = 9;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 10;
                    codonIndex = 10;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 10;
                    codonIndex = 11;
                }

            }

            // Second Letter G
            else if (codons[1] == "Guanine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 17;
                    codonIndex = 12;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 17;
                    codonIndex = 13;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 10;
                    codonIndex = 14;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 18;
                    codonIndex = 15;
                }

            }

        }

        // First Letter C
        else if (codons[0] == "Cytosine")
        {

            // Second Letter U
            if (codons[1] == "Uracil")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 1;
                    codonIndex = 16;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 1;
                    codonIndex = 17;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 1;
                    codonIndex = 18;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 1;
                    codonIndex = 19;
                }

            }

            // Second Letter C
            else if (codons[1] == "Cytosine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 6;
                    codonIndex = 20;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 6;
                    codonIndex = 21;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 6;
                    codonIndex = 22;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 6;
                    codonIndex = 23;
                }

            }

            // Second Letter A
            else if (codons[1] == "Adenine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 11;
                    codonIndex = 24;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 11;
                    codonIndex = 25;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 12;
                    codonIndex = 26;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 12;
                    codonIndex = 27;
                }

            }

            // Second Letter G
            else if (codons[1] == "Guanine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 19;
                    codonIndex = 28;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 19;
                    codonIndex = 29;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 19;
                    codonIndex = 30;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 19;
                    codonIndex = 31;
                }

            }

        }

        // First Letter A
        else if (codons[0] == "Adenine")
        {

            // Second Letter U
            if (codons[1] == "Uracil")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 2;
                    codonIndex = 32;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 2;
                    codonIndex = 33;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 2;
                    codonIndex = 34;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 3;
                    codonIndex = 35;
                }

            }

            // Second Letter C
            else if (codons[1] == "Cytosine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 7;
                    codonIndex = 36;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 7;
                    codonIndex = 37;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 7;
                    codonIndex = 38;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 7;
                    codonIndex = 39;
                }

            }

            // Second Letter A
            else if (codons[1] == "Adenine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 13;
                    codonIndex = 40;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 13;
                    codonIndex = 41;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 14;
                    codonIndex = 42;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 14;
                    codonIndex = 43;
                }

            }

            // Second Letter G
            else if (codons[1] == "Guanine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 5;
                    codonIndex = 44;
                }

                // Third Letter C
                else if (codons[2] == "Cytosine")
                {
                    aaIndex = 5;
                    codonIndex = 45;
                }

                // Third Letter A
                else if (codons[2] == "Adenine")
                {
                    aaIndex = 19;
                    codonIndex = 46;
                }

                // Third Letter G
                else if (codons[2] == "Guanine")
                {
                    aaIndex = 19;
                    codonIndex = 47;
                }

            }

        }

        // First Letter G
        if (codons[0] == "Guanine")
        {

            // Second Letter U
            if (codons[1] == "Uracil")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 4;
                    codonIndex = 48;
                }

                // Third Letter C
                if (codons[2] == "Cytosine")
                {
                    aaIndex = 4;
                    codonIndex = 49;
                }

                // Third Letter A
                if (codons[2] == "Adenine")
                {
                    aaIndex = 4;
                    codonIndex = 50;
                }

                // Third Letter G
                if (codons[2] == "Guanine")
                {
                    aaIndex = 4;
                    codonIndex = 51;
                }

            }

            // Second Letter C
            if (codons[1] == "Cytosine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 8;
                    codonIndex = 52;
                }

                // Third Letter C
                if (codons[2] == "Cytosine")
                {
                    aaIndex = 8;
                    codonIndex = 53;
                }

                // Third Letter A
                if (codons[2] == "Adenine")
                {
                    aaIndex = 8;
                    codonIndex = 54;
                }

                // Third Letter G
                if (codons[2] == "Guanine")
                {
                    aaIndex = 8;
                    codonIndex = 55;
                }

            }

            // Second Letter A
            if (codons[1] == "Adenine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 15;
                    codonIndex = 56;
                }

                // Third Letter C
                if (codons[2] == "Cytosine")
                {
                    aaIndex = 15;
                    codonIndex = 57;
                }

                // Third Letter A
                if (codons[2] == "Adenine")
                {
                    aaIndex = 16;
                    codonIndex = 58;
                }

                // Third Letter G
                if (codons[2] == "Guanine")
                {
                    aaIndex = 16;
                    codonIndex = 59;
                }

            }

            // Second Letter G
            if (codons[1] == "Guanine")
            {

                // Third Letter U
                if (codons[2] == "Uracil")
                {
                    aaIndex = 20;
                    codonIndex = 60;
                }

                // Third Letter C
                if (codons[2] == "Cytosine")
                {
                    aaIndex = 20;
                    codonIndex = 61;
                }

                // Third Letter A
                if (codons[2] == "Adenine")
                {
                    aaIndex = 20;
                    codonIndex = 62;
                }

                // Third Letter G
                if (codons[2] == "Guanine")
                {
                    aaIndex = 20;
                    codonIndex = 63;
                }

            }

        }

        // Set text based on codon index
        aminoAcidText.GetComponent<Text>().text = aminoAcid[aaIndex];
            aaCodeText.GetComponent<Text>().text = aaCode[aaIndex];
            codonText.GetComponent<Text>().text = aaCodon[codonIndex];

        // Adds the next letter to the list
        aaLettersList.Insert(0, aaLetter[aaIndex]);
        
        if (aaLettersList.Count == 7)
        {            
            aaLettersList.RemoveAt(6);
        }
        
        // Assigns the letters to corresponding 
        for (int i = 0; i < aaLettersList.Count; i++)
        {
            aaLetterText[i].GetComponent<Text>().text = aaLettersList[i];
        }

        // Sets the first letter
        
    }

    public void checkAntiCodon(string antiCodonLetter, int tileCount1, int tileCount2)
    {
        // Matches for adenine
        if (antiCodonLetter == "Adenine")
        {
            // When the tile is the complement, instantiate a tile in the appropriate slot
            if (codons[antiCodonIndex] == "Uracil")
            {
                instantiateAntiCodon(0, tileCount1, tileCount2);
                matchCounter[0]++;
            }

            else { resetAntiCodons(tileCount1, tileCount2); matchCounter[1]++; }
        }

        // Matches for cytosine
        else if (antiCodonLetter == "Cytosine")
        {
            if (codons[antiCodonIndex] == "Guanine")
            {
                instantiateAntiCodon(1, tileCount1, tileCount2);
                matchCounter[0]++;
            }

            else { resetAntiCodons(tileCount1, tileCount2); matchCounter[1]++; }
        }

        // Matches for guanine
        else if (antiCodonLetter == "Guanine")
        {
            if (codons[antiCodonIndex] == "Cytosine")
            {
                instantiateAntiCodon(2, tileCount1, tileCount2);
                matchCounter[0]++;
            }

            else { resetAntiCodons(tileCount1, tileCount2); matchCounter[1]++; }
        }

        // Matches for thymine
        else if (antiCodonLetter == "Thymine")
        {
            if (codons[antiCodonIndex] == "Adenine")
            {
                instantiateAntiCodon(3, tileCount1, tileCount2);
                matchCounter[0]++;
            }

            else { resetAntiCodons(tileCount1, tileCount2); matchCounter[1]++; }
        }

    }

    public void instantiateAntiCodon(int letterIndex, int tileCount1, int tileCount2)
    {
        playerCursor.GetComponent<sfxManager>().matchSound(antiCodonIndex);

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
            // Adds to score
            this.gameObject.GetComponent<PanelManager>().scoreManager(tileCount1, tileCount2, 3);

            antiCodonIndex = 0;

            for (int i = 0; i < 3; i++)
            {
                Destroy(antiCodonTiles[i]);
            }

            frameshiftRNA();
        }

        else
        {            
            antiCodonIndex++;

            // Adds to score
            this.gameObject.GetComponent<PanelManager>().scoreManager(tileCount1, tileCount2, antiCodonIndex);
        }
                
    }

    public void resetAntiCodons(int tileCount1, int tileCount2)
    {
        antiCodonIndex = 0;

        for (int i = 0; i < 3; i++)
        {
            Destroy(antiCodonTiles[i]);
        }

        playerCursor.GetComponent<sfxManager>().mismatchSound();

        // Adds to the player score
        this.gameObject.GetComponent<PanelManager>().scoreManager(tileCount1, tileCount2, antiCodonIndex);
    }

    private IEnumerator SlideTile(GameObject tile, Vector3 targetPos)
    {
        // Initializes time and displacement variables
        float elapsedTime = 0;
            float timeToMove = playerCursor.GetComponent<PlayerMovement>().timeToMove;          // Borrows the time limit as defined by PlayerMovement in the inspector

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
}
