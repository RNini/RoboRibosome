using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BoardManager : MonoBehaviour
{

    // Tiles to instantiate
    [SerializeField] private GameObject tilePreFab = null;
        public GameObject nuTile;

    // Array to keep track of instantiated tiles
    //public GameObject[] boardTiles;       // Board Tiles 1D array
    public GameObject[,] boardTiles;        // Board Tiles 2D array
    private int boardWidth;                 // Initialize to 6   
        private int boardHeight;            // Initialize to 12

    public int colStart;                    // Parameters for limiting tile instantiation bounds
        public int colEnd;

    public int rowStart;
        public int rowEnd;

    // Array of tiles to pull from randomly
    public GameObject[] tileList;

    // Variables
    public Vector3 tileSlot1;
        public Vector3 tileSlot2;
        
    public float[] xCoor;
        public float[] yCoor;
        public float[] zCoor;

    private float tempX;
        private float tempY;
        private float tempZ;

    // Variables to for animating tiles
    private GameObject oldTile;
    
    private Color origColor;
        private Color tempColor;

        public bool isAnimating;

    // Start is called before the first frame update
    void Start()
    {
        // Place tiles on board
        boardWidth = 12;
            boardHeight = 6;
        
        PopulateBoard();

        Time.timeScale = 1;
    }

    public void PopulateBoard()
    {
        // Sets the dimensions of the 2D array
        boardTiles = new GameObject[boardWidth, boardHeight];

        // Loop through columns
        for (int i = 0; i < boardHeight; i++) 
        {
            // Loop through rows
            for (int j = 0; j < boardWidth; j++)
            {

                tileSlot1 = new Vector3(xCoor[j], yCoor[i], 5);
                tilePreFab = tileList[Random.Range(0, tileList.Length)];

                // Instanstiates a random tile at the calculated coordinates to the game board
                nuTile = Instantiate(tilePreFab, tileSlot1, new Quaternion(0, 0, 0, 0));

                // Stores the prefab in the 2D array for future use
                boardTiles[j, i] = nuTile;
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
    
}

    
