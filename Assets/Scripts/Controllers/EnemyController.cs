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
    public float m_walkSpeed;
    public float m_runSpeed;
    // Complex
    private GameObject m_projectilePrefab;
    private BulletPattern m_BulletPattern;
    private AIPattern m_idleAI;
    private AIPattern m_aggroAI;

    // settings variables
    private float aggroYRange = 7f;      // TODO: maybe in Enemy SO? hardcode for now
    private float aggroFeathering = 2f;   // hysteresis for aggro distance

    // helper variables
    private float timer;
    public bool facingRight = true;    // TODO: add facingRight logic
    private bool isIdle = true;         // TODO: add isIdle logic. remember to reset aipattern when switching

    void Awake()
    {
        // Set up the enemy variables
        InitializeEnemy();
        m_idleAI.Initialize(this.gameObject);
        m_aggroAI.Initialize(this.gameObject);
    }

    void FixedUpdate()
    {
        // check if aggro state needs to be changed
        //float distanceToPlayer = Vector3.Distance(this.gameObject.transform, PlayerController.Instance.gameObject.transform);
        float distanceXToPlayer = Mathf.Abs(this.gameObject.transform.position.x - PlayerController.Instance.gameObject.transform.position.x);
        float distanceYToPlayer = Mathf.Abs(this.gameObject.transform.position.y - PlayerController.Instance.gameObject.transform.position.y);
        if (isIdle && distanceYToPlayer < aggroYRange && distanceXToPlayer < m_aggroDistance)
        {
            Debug.Log("aggro!");
            // TODO: face player
            isIdle = false;
            m_idleAI.StopAIPattern();
            m_aggroAI.StartAIPattern();
        }
        if (!isIdle && (distanceYToPlayer > aggroYRange + aggroFeathering || distanceXToPlayer > m_aggroDistance + aggroFeathering))
        {
            Debug.Log("lost aggro");
            isIdle = true;
            m_aggroAI.StopAIPattern();
            m_idleAI.StartAIPattern();
        }

        // AIPattern behavior
        if (isIdle) { m_idleAI.UpdateAIPattern(Time.fixedDeltaTime); }
        else { m_aggroAI.UpdateAIPattern(Time.fixedDeltaTime); }
    }

    // public methods
    public void Fire() { StartCoroutine(FireCoroutine()); }
    public void TurnAround()
    {
        facingRight = !facingRight;
        // Multiply the x local scale by -1.
        Vector3 theScale = this.transform.localScale;
        theScale.x *= -1;
        this.transform.localScale = theScale;
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
        m_walkSpeed = enemyDefinitions.walkSpeed;
        m_runSpeed = enemyDefinitions.runSpeed;
        // Complex
        this.GetComponent<SpriteRenderer>().sprite = enemyDefinitions.sprite;
        m_projectilePrefab = enemyDefinitions.bulletPrefab;
        m_BulletPattern = enemyDefinitions.bulletPattern;
        m_idleAI = enemyDefinitions.idleAI;
        m_aggroAI = enemyDefinitions.aggroAI;

        // Check if bullet pattern SO is valid.
        // TODO: automatic way to do this?
        // TODO: move to bulletcontroller?
        if (m_BulletPattern.numberOfBullets != m_BulletPattern.listOfAngles.Count || 
            m_BulletPattern.listOfAngles.Count != 1 + m_BulletPattern.delayBetweenBullets.Count)
        {
            Debug.Log("ERROR with Bullet Pattern for enemy: " + m_name);
        }
    }


    // Coroutines
    private IEnumerator FireCoroutine()
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
