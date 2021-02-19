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
        hpText.text = "HP: " + currentHealth + "/" + maxHealth;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log(col.gameObject.name);
    }

    // public methods
    public void TakeDamage(int damage)
    {
        Debug.Log("ow");
        currentHealth -= damage;
    }

}

