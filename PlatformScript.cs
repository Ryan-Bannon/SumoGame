using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlatformScript : MonoBehaviour
{
    [HideInInspector]
    public Transform transform;
    //The amount of time before the platform begins to shrink
    public float timeLeft;
    //Controls the rate at which the platform shrinks
    public float shrinkageRate;
    //Stores what is set as the time in Unity
    [HideInInspector]
    public float timeAtStart;
    //Stores the platform's scale at the start so it can be set back when a player falls off
    [HideInInspector]
    public Vector3 initialScale;
    //Accesses the first player's gameobject
	public GameObject player1;
    //Accesses the first player's script
	[HideInInspector]
	public PlayerScript1 p1Script;
   
    void Start()
    {
        transform = GetComponent<Transform>();
        //Since the scale is so small, we have to define a constant by 1000, so 2 becomes .0002
        shrinkageRate /= 1000;
        initialScale = transform.localScale;
        timeAtStart = timeLeft;
        p1Script = player1.GetComponent<PlayerScript1>();
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        if(!p1Script.gameOver)
        {
        //Checks if the timer is finished
        if(timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
        }      
        //If the timer ends, the platform begins to shrink, and the conditional stops the scale from inverting
        else if(transform.localScale.x >= 0)
            transform.localScale -= new Vector3(shrinkageRate, 0, shrinkageRate);
        }        
    }

    public void resetPlatform()
    {
        timeLeft = timeAtStart;
        transform.localScale = initialScale;
    }
}
