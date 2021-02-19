using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    // GameObjects
    [SerializeField] private GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("shoot");
            if (PlayerBulletController.BulletsAvailable())
            {
                GameObject bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
                bullet.GetComponent<PlayerBulletController>().Launch();
            }
        }
    }
}
