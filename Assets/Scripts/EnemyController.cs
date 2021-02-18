using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // All the possible enemies
    enum EnemyType
    {
        Burger,
        None
    }

    // GameObjects
    public GameObject projectilePrefab;


    // Definition variables

    // helper variables
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3f)
        {
            timer = 0;
            GameObject bullet = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(-2, 0, 0);
        }
    }
}
