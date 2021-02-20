using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    // GameObjects
    [SerializeField] private GameObject bulletPrefab;

    // definition variables
    [SerializeField] private int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (PlayerBulletController.BulletsAvailable())
            {
                GameObject bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
                PlayerBulletController bulletController = bullet.GetComponent<PlayerBulletController>();
                bulletController.DefineBulletDamage(damage);
                bulletController.Launch();
            }
        }
    }
}
