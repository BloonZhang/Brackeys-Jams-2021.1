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
    // Gameobjects
    public GameObject dessertPrefab;

    // Definition variables
    // ScriptableObjects
    public Enemy enemyDefinitions; // scriptable object containing all definitions
    // Simple
    private string m_name;
    private int m_maxHealth;
    private int m_damage;
    private bool m_invincibleWhileIdle;
    private float m_aggroDistance;
    [HideInInspector] public float m_walkSpeed;
    [HideInInspector] public float m_runSpeed;
    // Complex
    private GameObject m_projectilePrefab;
    private BulletPattern m_BulletPattern;
    private AIPattern m_idleAI;
    private AIPattern m_aggroAI;
    private List<Dessert> m_possibleDrops;
    private List<float> m_dropPercentages;

    // settings variables
    private float aggroYRange = 7f;      // TODO: maybe in Enemy SO? hardcode for now
    private float aggroFeathering = 2f;   // hysteresis for aggro distance
    private Vector3 dropOffset = new Vector3(0, 0.5f, 0); // Offset for dessert drops

    // helper variables
    private float timer;
    public bool facingRight = true;
    private bool isIdle = true;
    private int currentHealth;

    void Awake()
    {
        // Set up the enemy variables
        InitializeEnemy();
        m_idleAI.Initialize(this.gameObject);
        m_aggroAI.Initialize(this.gameObject);
        currentHealth = m_maxHealth;
    }

    void FixedUpdate()
    {
        // check if aggro state needs to be changed
        //float distanceToPlayer = Vector3.Distance(this.gameObject.transform, PlayerController.Instance.gameObject.transform);
        float distanceXToPlayer = Mathf.Abs(this.gameObject.transform.position.x - PlayerController.Instance.gameObject.transform.position.x);
        float distanceYToPlayer = Mathf.Abs(this.gameObject.transform.position.y - PlayerController.Instance.gameObject.transform.position.y);
        if (isIdle && distanceYToPlayer < aggroYRange && distanceXToPlayer < m_aggroDistance)
        {
            isIdle = false;
            m_idleAI.StopAIPattern();
            m_aggroAI.StartAIPattern();
        }
        if (!isIdle && (distanceYToPlayer > aggroYRange + aggroFeathering || distanceXToPlayer > m_aggroDistance + aggroFeathering))
        {
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
    public void TakeDamage(int damage) 
    { 
        currentHealth -= damage; 
        if (currentHealth <= 0) { Death(); } 
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
            bullet.GetComponent<EnemyBulletController>().DefineBulletDamage(m_damage);
            // wait for x seconds if not 0 seconds (and also if not last bullets)
            if (i < m_BulletPattern.numberOfBullets - 1) 
            {
                if (m_BulletPattern.delayBetweenBullets[i] == 0) { continue; }
                else { yield return new WaitForSeconds(m_BulletPattern.delayBetweenBullets[i]); }
            }
        }
        yield return null;
    }

    // helper methods
    private void InitializeEnemy()
    {
        // Simple
        m_name = enemyDefinitions.name;
        m_maxHealth = enemyDefinitions.maxHealth;
        m_invincibleWhileIdle = enemyDefinitions.invincibleWhileIdle;
        m_aggroDistance = enemyDefinitions.aggroDistance;
        m_walkSpeed = enemyDefinitions.walkSpeed;
        m_runSpeed = enemyDefinitions.runSpeed;
        m_damage = enemyDefinitions.damagePerBullet;
        // Complex
        this.GetComponent<SpriteRenderer>().sprite = enemyDefinitions.sprite;
        m_projectilePrefab = enemyDefinitions.bulletPrefab;
        m_BulletPattern = enemyDefinitions.bulletPattern;
        m_idleAI = enemyDefinitions.idleAI;
        m_aggroAI = enemyDefinitions.aggroAI;
        m_possibleDrops = enemyDefinitions.possibleDrops;
        m_dropPercentages = enemyDefinitions.dropPercentages;

        // Check if bullet pattern SO is valid.
        // TODO: automatic way to do this?
        // TODO: move to bulletcontroller?
        if (m_BulletPattern.numberOfBullets != m_BulletPattern.listOfAngles.Count || 
            m_BulletPattern.listOfAngles.Count != 1 + m_BulletPattern.delayBetweenBullets.Count)
        {
            Debug.Log("ERROR with Bullet Pattern counts for enemy: " + m_name);
        }
        // Check if drop table is valid
        if (m_possibleDrops.Count != m_dropPercentages.Count)
        {
            Debug.Log("ERROR with counts for drop table for enemy: " + m_name);
        }

    }
    private Dessert GetRandomDrop()
    {
        float rng = Random.Range(0.0f, 1.0f);
        float rngHelper = 0f;
        for (int i = 0; i < m_possibleDrops.Count; ++i)
        {
            rngHelper += m_dropPercentages[i];
            if (rng < rngHelper) { return m_possibleDrops[i]; }
        }
        // Didn't get any drops
        return null;
    }
    private void SpawnDrop(Dessert drop)
    {
        GameObject dessert = Instantiate(dessertPrefab, this.transform.position + dropOffset, Quaternion.identity);
        dessert.GetComponent<DessertController>().dessertDefinitions = drop;
    }
    private void Death()
    {
        Debug.Log("enemy: " + this.name + " died");
        // TODO: explosion effect
        // Potential drops
        Dessert enemyDrop = GetRandomDrop();
        if (enemyDrop != null) { SpawnDrop(enemyDrop); }
        Destroy(this.gameObject);
    }

    /*
    // testing item drop rate works as intended
    private void Test()
    {
        int[] dropList = new int[5];
        Dessert testDessert = null;
        for (int i = 0; i < 10000; ++i)
        {
            testDessert = GetRandomDrop();
            if (testDessert == null) { dropList[4]++; continue; }
            switch (testDessert.name)
            {
                case "LargeHealth":
                    dropList[0]++;
                    break;
                case "LargeSpecial":
                    dropList[1]++;
                    break;
                case "SmallHealth":
                    dropList[2]++;
                    break;
                case "SmallSpecial":
                    dropList[3]++;
                    break;
                default:
                    dropList[4]++;
                    break;
            }
        }
        Debug.Log("LargeHealth: " + dropList[0]);
        Debug.Log("LargeSpecial: " + dropList[1]);
        Debug.Log("SmallHealth: " + dropList[2]);
        Debug.Log("SmallSpecial: " + dropList[3]);
        Debug.Log("None: " + dropList[4]);

    }
    */
}
