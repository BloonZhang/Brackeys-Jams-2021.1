using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    // definition variables
    float speed = 0.75f; // how many units it moves a second

    // helper variables
    bool direction = true; // false = left, true = right


    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3((direction ? 1 : -1) * speed * Time.deltaTime, 0, 0);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "TrackTilemap") { direction = !direction; };
    }

}

