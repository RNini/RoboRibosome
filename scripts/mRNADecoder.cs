using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class mRNADecoder : MonoBehaviour
{
    // Variable for accessing scripts
    public List<GameObject> mRNATilesList;

    // Variables to store codon and anti-codon information
    public string[] codons;

    // Location variables
    private Vector3Int tileLocation;
        private int xCoor;
        private int yCoor;
        private int zCoor;

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
        public GameObject aaLetterText;

    public GameObject codonText;

    // Misc variables
    private string tempTileName;

    public void Start()
    {
        aaIndex = -1;
        codonIndex = -1;

        mRNATilesList = this.GetComponent<mRNAManager>().mRNATilesList;  // Grabs the instantiated tiles from the mRNA manager
    }

    public void Translate()
    {
        
        // Loop through mRNABoard codon slots to read tile tags
        for (int i = 12; i < 15; i++)
        {
            codons[i] = mRNATilesList[i].tag;
        }

        // Nested IFs to grab indices for other amino-acid info
            // First Letter U
            if(codons[0] == "Uracil")
            {
                
                // Second Letter U
                if(codons[1] == "Uracil")
                {

                    // Third Letter U
                    if(codons[2] == "Uracil")
                        {
                        aaIndex = 0;
                        codonIndex = 0;
                        }

                    // Third Letter C
                    if(codons[2] == "Cytosine")
                        {
                        aaIndex = 0;
                        codonIndex = 1;
                        }

                    // Third Letter A
                    if (codons[2] == "Adenine")
                        {
                        aaIndex = 1;
                        codonIndex = 2;
                        }

                    // Third Letter G
                    if (codons[2] == "Guanine")
                        {
                        aaIndex = 1;
                        codonIndex = 3;
                        }

                }

                // Second Letter C
                if (codons[1] == "Cytosine")
                {

                    // Third Letter U
                    if (codons[2] == "Uracil")
                    {
                        aaIndex = 5;
                        codonIndex = 4;
                    }

                    // Third Letter C
                    if (codons[2] == "Cytosine")
                    {
                        aaIndex = 5;
                        codonIndex = 5;
                    }

                    // Third Letter A
                    if (codons[2] == "Adenine")
                    {
                        aaIndex = 5;
                        codonIndex = 6;
                    }

                    // Third Letter G
                    if (codons[2] == "Guanine")
                    {
                        aaIndex = 5;
                        codonIndex = 7;
                    }

                }

                // Second Letter A
                if (codons[1] == "Adenine")
                {

                    // Third Letter U
                    if (codons[2] == "Uracil")
                    {
                        aaIndex = 9;
                        codonIndex = 8;
                    }

                    // Third Letter C
                    if (codons[2] == "Cytosine")
                    {
                        aaIndex = 9;
                        codonIndex = 9;
                    }

                    // Third Letter A
                    if (codons[2] == "Adenine")
                    {
                        aaIndex = 10;
                    codonIndex = 10;
                    }

                    // Third Letter G
                    if (codons[2] == "Guanine")
                    {
                        aaIndex = 10;
                        codonIndex = 11;
                    }

                }

                // Second Letter G
                if (codons[1] == "Guanine")
                {

                    // Third Letter U
                    if (codons[2] == "Uracil")
                    {
                        aaIndex = 17;
                        codonIndex = 12;
                    }

                    // Third Letter C
                    if (codons[2] == "Cytosine")
                    {
                        aaIndex = 17;
                        codonIndex = 13;
                    }

                    // Third Letter A
                    if (codons[2] == "Adenine")
                    {
                        aaIndex = 10;
                        codonIndex = 14;
                    }

                    // Third Letter G
                    if (codons[2] == "Guanine")
                    {
                        aaIndex = 18;
                        codonIndex = 15;
                    }

                }

            }

            // First Letter C
            if (codons[0] == "Cytosine")
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
                    if (codons[2] == "Cytosine")
                    {
                        aaIndex = 1;
                        codonIndex = 17;
                    }

                    // Third Letter A
                    if (codons[2] == "Adenine")
                    {
                        aaIndex = 1;
                        codonIndex = 18;
                    }

                    // Third Letter G
                    if (codons[2] == "Guanine")
                    {
                        aaIndex = 1;
                        codonIndex = 19;
                    }

                }

                // Second Letter C
                if (codons[1] == "Cytosine")
                {

                    // Third Letter U
                    if (codons[2] == "Uracil")
                    {
                        aaIndex = 6;
                        codonIndex = 20;
                    }

                    // Third Letter C
                    if (codons[2] == "Cytosine")
                    {
                        aaIndex = 6;
                        codonIndex = 21;
                    }

                    // Third Letter A
                    if (codons[2] == "Adenine")
                    {
                        aaIndex = 6;
                        codonIndex = 22;
                    }

                    // Third Letter G
                    if (codons[2] == "Guanine")
                    {
                        aaIndex = 6;
                        codonIndex = 23;
                    }

                }

                // Second Letter A
                if (codons[1] == "Adenine")
                {

                    // Third Letter U
                    if (codons[2] == "Uracil")
                    {
                        aaIndex = 11;
                        codonIndex = 24;
                    }

                    // Third Letter C
                    if (codons[2] == "Cytosine")
                    {
                        aaIndex = 11;
                        codonIndex = 25;
                    }

                    // Third Letter A
                    if (codons[2] == "Adenine")
                    {
                        aaIndex = 12;
                        codonIndex = 26;
                    }

                    // Third Letter G
                    if (codons[2] == "Guanine")
                    {
                        aaIndex = 12;
                        codonIndex = 27;
                    }

                }

                // Second Letter G
                if (codons[1] == "Guanine")
                {

                    // Third Letter U
                    if (codons[2] == "Uracil")
                    {
                        aaIndex = 19;
                        codonIndex = 28;
                    }

                    // Third Letter C
                    if (codons[2] == "Cytosine")
                    {
                        aaIndex = 19;
                        codonIndex = 29;
                    }

                    // Third Letter A
                    if (codons[2] == "Adenine")
                    {
                        aaIndex = 19;
                        codonIndex = 30;
                        }

                    // Third Letter G
                    if (codons[2] == "Guanine")
                    {
                        aaIndex = 19;
                        codonIndex = 31;
                        }

                }

            }

            // First Letter A
            if (codons[0] == "Adenine")
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
                    if (codons[2] == "Cytosine")
                    {
                        aaIndex = 2;
                        codonIndex = 33;
                    }

                    // Third Letter A
                    if (codons[2] == "Adenine")
                    {
                        aaIndex = 2;
                        codonIndex = 34;
                    }

                    // Third Letter G
                    if (codons[2] == "Guanine")
                    {
                        aaIndex = 3;
                        codonIndex = 35;
                    }

                }

                // Second Letter C
                if (codons[1] == "Cytosine")
                {

                    // Third Letter U
                    if (codons[2] == "Uracil")
                    {
                        aaIndex = 7;
                        codonIndex = 36;
                    }

                    // Third Letter C
                    if (codons[2] == "Cytosine")
                    {
                        aaIndex = 7;
                        codonIndex = 37;
                    }

                    // Third Letter A
                    if (codons[2] == "Adenine")
                    {
                        aaIndex = 7;
                        codonIndex = 38;
                    }

                    // Third Letter G
                    if (codons[2] == "Guanine")
                    {
                        aaIndex = 7;
                        codonIndex = 39;
                    }

                }

                // Second Letter A
                if (codons[1] == "Adenine")
                {

                    // Third Letter U
                    if (codons[2] == "Uracil")
                    {
                        aaIndex = 13;
                        codonIndex = 40;
                    }

                    // Third Letter C
                    if (codons[2] == "Cytosine")
                    {
                        aaIndex = 13;
                        codonIndex = 41;
                    }

                    // Third Letter A
                    if (codons[2] == "Adenine")
                    {
                        aaIndex = 14;
                        codonIndex = 42;
                    }

                    // Third Letter G
                    if (codons[2] == "Guanine")
                    {
                        aaIndex = 14;
                        codonIndex = 43;
                    }

                }

                // Second Letter G
                if (codons[1] == "Guanine")
                {

                    // Third Letter U
                    if (codons[2] == "Uracil")
                    {
                        aaIndex = 5;
                        codonIndex = 44;
                    }

                    // Third Letter C
                    if (codons[2] == "Cytosine")
                    {
                        aaIndex = 5;
                        codonIndex = 45;
                    }

                // Third Letter A
                if (codons[2] == "Adenine")
                    {
                        aaIndex = 19;
                        codonIndex = 46;
                    }

                // Third Letter G
                if (codons[2] == "Guanine")
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
            aaLetterText.GetComponent<Text>().text = aaLetter[aaIndex];
            codonText.GetComponent<Text>().text = aaCodon[codonIndex];
    }

}
