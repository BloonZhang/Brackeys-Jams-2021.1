using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{

    // counts the number of bullets on screen
    private static int numberOfBulletsOnScreen = 0;
    //public static int NumberOfBulletsOnScreen { get { return numberOfBulletsOnScreen; } }

    // abbreviation variables
    private int enemyHitboxLayer = 14;

    // definition properties
    private static int maxBulletsOnScreen = 3;
    //public static int MaxBulletsOnScreen { get { return maxBulletsOnScreen; } }
    private float bulletSpeed = 500f;
    private float timeOnScreen = 1.5f;
    private int damage;

    // helper variables
    private float timer = 0f;

    void Awake()
    {
        // Incrememnt number of bullets
        ++numberOfBulletsOnScreen;
        
        // if somehow there are too many bullets
        if (numberOfBulletsOnScreen > maxBulletsOnScreen) { Destroy(this.gameObject); }
    }

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

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == enemyHitboxLayer) 
        { 
            EnemyController enemyHit = col.transform.parent.GetComponent<EnemyController>();
            if (enemyHit == null) { Debug.Log("No EnemyController found in PlayerBulletController.OnTriggerEnter2D"); }
            else { enemyHit.TakeDamage(damage); }
        }
        RemoveBullet();
    }

    void OnDestroy()
    {
        --numberOfBulletsOnScreen;
    }

    // "constructor" methods
    public void DefineBulletDamage(int damageToDeal)
    {
        this.damage = damageToDeal;
    }

    // public methods
    // Sends the bullet flying in the correct direction
    public void Launch()
    {
        // Get direction of player
        bool launchDirection = PlayerController.Instance.GetComponent<CharacterController2D>().FacingRight;
        // Send bullet in that direction with specified speed
        this.GetComponent<Rigidbody2D>().AddForce(new Vector3((launchDirection ? 1 : -1), 0, 0) * bulletSpeed);
    }

    // helper methods
    public static bool BulletsAvailable() { return numberOfBulletsOnScreen < maxBulletsOnScreen; }
    private void RemoveBullet() { Destroy(this.gameObject); }
}
