using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    // gameobject
    private GameObject playerOnPlatform = null;

    // definition variables
    float speed = 0.75f; // how many units it moves a second

    // helper variables
    bool direction = true; // false = left, true = right
    private Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3((direction ? 1 : -1) * speed * Time.deltaTime, 0, 0);
    }

    void LateUpdate()
    {
        if (playerOnPlatform != null) {
            playerOnPlatform.transform.position = transform.position + offset;
        }
    }

    // Trigger2D methods
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "TrackTilemap") { direction = !direction; };
    }
    void OnTriggerStay2D(Collider2D col)
    {
        // For making the player stick on the platform
        if (col.gameObject.tag == "Player") 
        { 
            playerOnPlatform = col.gameObject; 
            offset = playerOnPlatform.transform.position - transform.position;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        // no more sticking once player is off platform
        if (col.gameObject.tag == "Player")
        {
            playerOnPlatform = null;
        }
    }

}

