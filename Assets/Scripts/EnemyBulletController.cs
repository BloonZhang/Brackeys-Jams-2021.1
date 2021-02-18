using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{

    // definition variables (default values)
    private float timeOnScreen = 5f;

    // helper variables
    private float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeOnScreen) { Destroy(this.gameObject); }
    }

    // "constructors"
    public void DefineBulletLifetime(float time)
    {
        timeOnScreen = time;
    }
}
