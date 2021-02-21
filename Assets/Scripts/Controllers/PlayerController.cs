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
    [SerializeField] private int maxSpecial;
    private int currentSpecial;
    public int CurrentSpecial { get { return currentSpecial; } }

    // definition variables
    [SerializeField] private float timeInvincibleAfterHit;
    //[SerializeField] private float timeStunnedAfterHit;
    private float healRate = 10f;   // How fast the player heals over time from a pickup
    private float spriteFlashRate = 10f;// How many times a second the player's sprite flashes when hit

    // helper variables
    private bool invincible = false;
    private float invincibilityTimer = 0f;
    //private bool stunned;
    //private float stunTimer = 0f;
    private float healthToHeal;     // determines how much health must be healed
    private float specialToRecover;
    private bool currentlyHealing = false;
    private Coroutine spriteFlashCoroutine;
    private float spriteFlashTimer = 0f;



    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}

        // Set up variables
        currentHealth = maxHealth;
        currentSpecial = maxSpecial;
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
            if (invincibilityTimer > timeInvincibleAfterHit) { StopInvincible(); }
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
    public void Heal(int healing, int recovery)
    {
        healthToHeal += healing;
        specialToRecover += recovery;
        if (!currentlyHealing) { StartCoroutine(HealOverTime()); }
    }

    // Coroutines
    private IEnumerator HealOverTime()
    {
        currentlyHealing = true;
        // while there's still healing left to do
        while (healthToHeal > 0 || specialToRecover > 0)
        {
            // Health section
            if (currentHealth == maxHealth) { healthToHeal = 0; }
            else 
            {
                currentHealth += 1; healthToHeal -= 1;
            }
            // Special section
            if (currentSpecial == maxSpecial) { specialToRecover = 0; }
            else
            {
                currentSpecial += 1; specialToRecover -= 1;
            }
            // continue if not done healing
            if (healthToHeal > 0 || specialToRecover > 0) { yield return new WaitForSeconds(1f / healRate); }
            else { break; }
        }
        // done healing
        currentlyHealing = false;
        yield return null; 
    }
    private IEnumerator FlashSprite()
    {
        while(true)
        {
            SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(1f / spriteFlashRate);
        }
        yield return null;
    }

    // helper methods
    private void BecomeInvincible()
    {
        invincible = true; invincibilityTimer = 0f;
        spriteFlashCoroutine = StartCoroutine(FlashSprite());
    }
    private void StopInvincible()
    {
        invincible = false;
        StopCoroutine(spriteFlashCoroutine);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
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

