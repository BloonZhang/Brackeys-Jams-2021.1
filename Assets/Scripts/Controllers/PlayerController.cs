using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //////// Singleton shenanigans ////////
    private static PlayerController _instance;
    public static PlayerController Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // GameObjects
    [SerializeField] private TextMeshProUGUI hpText;

    // game properties
    [SerializeField] private int maxHealth;
    private int currentHealth;
    public int CurrentHealth {get { return currentHealth; } }

    // definition variables
    [SerializeField] private float timeInvincibleAfterHit;
    //[SerializeField] private float timeStunnedAfterHit;

    // helper variables
    private bool invincible = false;
    private float invincibilityTimer = 0f;
    //private bool stunned;
    //private float stunTimer = 0f;


    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}

        // Set up variables
        currentHealth = maxHealth;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update HP text
        hpText.text = "HP: " + currentHealth + "/" + maxHealth;

        // if invincible, count the timer up
        // TODO: flash sprite
        if (invincible)
        {
            invincibilityTimer += Time.deltaTime;
            if (invincibilityTimer > timeInvincibleAfterHit) { invincible = false; }
        }
        /*
        if (stunned)
        {
            stunTimer += Time.deltaTime;
            if (stunTimer > timeStunnedAfterHit) { stunned = false; }
        }
        */
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log(col.gameObject.name);
    }

    // public methods
    public void TakeDamage(int damage)
    {
        // Don't take any damage if in Iframes
        if (invincible) { return; }
        Debug.Log("ow. damage taken: " + damage);
        currentHealth -= damage;
        if (currentHealth <= 0) { Death(); }
        BecomeInvincible();
        //BecomeStunned();
    }

    // helper methods
    private void BecomeInvincible()
    {
        invincible = true; invincibilityTimer = 0f;
    }
    private void BecomeStunned()
    {
        // stunned = true; stunTimer = 0f;
    }
    private void Death()
    {
        Debug.Log("Game Over!");
    }

}

