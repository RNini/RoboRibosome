using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationManager : MonoBehaviour
{
    // Variables for robots and their animators
    public GameObject robo1;
        public Animation[] robo1Animations;
        private Animator robo1Animator;
    
    public GameObject robo2;
        public Animation[] robo2Animations;
        private Animator robo2Animator;
    

    public GameObject robo3;
        public Animation[] robo3Animations;
        private Animator robo3Animator;

    // Start is called before the first frame update
    void Start()
    {
        // Assign animator components
        robo1Animator = robo1.GetComponent<Animator>();
            robo2Animator = robo2.GetComponent<Animator>();
            robo3Animator = robo3.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
