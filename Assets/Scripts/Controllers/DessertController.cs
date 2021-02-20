using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DessertController : MonoBehaviour
{
    // ScriptableObject
    public Dessert dessertDefinitions;

    // abbreviation variables
    private string playerCollisionboxName = "PlayerCollisionbox";

    // definition variables
    private string m_name;
    private int m_healing;
    private int m_specialRecovery;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // if it's the player
        if (col.gameObject.name == playerCollisionboxName)
        {
            PlayerController.Instance.Heal(m_healing, m_specialRecovery);
            Destroy(this.gameObject);
        }
    }

    // helper methods
    public void Initialize()
    {
        // simple variables
        m_name = dessertDefinitions.name;
        m_healing = dessertDefinitions.healing;
        m_specialRecovery = dessertDefinitions.specialRecovery;
        // components
        this.GetComponent<SpriteRenderer>().sprite = dessertDefinitions.sprite;
    }
}
