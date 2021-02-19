using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    // abbreviation variables
    private int playerHitboxLayer = 13;

    // definition variables (default values)
    private float timeOnScreen = 5f;
    private int damage = 1;

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
        if (timer > timeOnScreen) { RemoveBullet(); }
    }

    // "constructors"
    public void DefineBulletLifetime(float time)
    {
        timeOnScreen = time;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // if it hits the player hitbox
        if(col.gameObject.layer == playerHitboxLayer) { PlayerController.Instance.TakeDamage(damage); }
        RemoveBullet();
    }

    // helper methods
    private void RemoveBullet()
    {
        Destroy(this.gameObject);
    }
}
