using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsPainter : MonoBehaviour
{

    public SpriteRenderer spriteRender;

    public RuntimeAnimatorController[] roboControllers;
    public int roboChassisIndex;

    // Start is called before the first frame update
    void Start()
    {
        // Grabs the int we set from the character selection screen
        roboChassisIndex = PlayerPrefs.GetInt("PlayerChassis");
        
        // Sets the Animator controller from the array based on the character chosen
        this.gameObject.GetComponent<Animator>().runtimeAnimatorController = roboControllers[roboChassisIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
