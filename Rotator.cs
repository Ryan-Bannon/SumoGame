using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool collected;
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }

    void OnTriggerEnter(Collider player)
    {
        if(player.tag == "Player")
        {
            collected = true;
            gameObject.SetActive(false);          
        }           
    }
}