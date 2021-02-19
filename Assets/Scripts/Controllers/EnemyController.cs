using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // All the possible enemies
    /*
    enum EnemyType
    {
        Burger,
        None
    }
    */

    // Definition variables
    // ScriptableObjects
    public Enemy enemyDefinitions; // scriptable object containing all definitions
    // Simple
    private string m_name;
    private int m_maxHealth;
    private int m_damage;
    private bool m_invincibleWhileIdle;
    private float m_delayBetweenShots;
    private float m_aggroDistance;
    // Complex
    private GameObject m_projectilePrefab;
    private BulletPattern m_BulletPattern;

    // helper variables
    private float timer;
    private bool facingRight = true;    // TODO: correct facingRight logic

    void Awake()
    {
        // Set up the enemy variables
        InitializeEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > m_delayBetweenShots)
        {
            timer = 0;
            StartCoroutine(Fire());
        }
    }

    // helper methods
    private void InitializeEnemy()
    {
        // Simple
        m_name = enemyDefinitions.name;
        m_maxHealth = enemyDefinitions.maxHealth;
        m_invincibleWhileIdle = enemyDefinitions.invincibleWhileIdle;
        m_delayBetweenShots = enemyDefinitions.shotCooldown;
        m_aggroDistance = enemyDefinitions.aggroDistance;
        // Complex
        this.GetComponent<SpriteRenderer>().sprite = enemyDefinitions.sprite;
        m_projectilePrefab = enemyDefinitions.bulletPrefab;
        m_BulletPattern = enemyDefinitions.bulletPattern;

        // Check if bullet pattern SO is valid.
        // TODO: automatic way to do this?
        if (m_BulletPattern.numberOfBullets != m_BulletPattern.listOfAngles.Count || 
            m_BulletPattern.listOfAngles.Count != 1 + m_BulletPattern.delayBetweenBullets.Count)
        {
            Debug.Log("ERROR with Bullet Pattern for enemy: " + m_name);
        }
    }
    private IEnumerator Fire()
    {
        // For each bullet in the list, fire at the specified angle with the specified speed
        // then wait for the specified time
        for (int i = 0; i < m_BulletPattern.numberOfBullets; ++i)
        {
            GameObject bullet = Instantiate(m_projectilePrefab,
                                            new Vector3(this.transform.position.x + enemyDefinitions.shootPointOffset.x,
                                                        this.transform.position.y + enemyDefinitions.shootPointOffset.y,
                                                        this.transform.position.z),
                                            Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(  m_BulletPattern.bulletSpeed * Mathf.Cos(Mathf.Deg2Rad * m_BulletPattern.listOfAngles[i]) * (facingRight ? 1 : -1),
                                                                        m_BulletPattern.bulletSpeed * Mathf.Sin(Mathf.Deg2Rad * m_BulletPattern.listOfAngles[i]),
                                                                        0);
            // wait for x seconds if not 0 seconds (and also if not last bullets)
            if (i < m_BulletPattern.numberOfBullets - 1) 
            {
                if (m_BulletPattern.delayBetweenBullets[i] == 0) { continue; }
                else { yield return new WaitForSeconds(m_BulletPattern.delayBetweenBullets[i]); }
            }
        }
        yield return null;
    }



}
