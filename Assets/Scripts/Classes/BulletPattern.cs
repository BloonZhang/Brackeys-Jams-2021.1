using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BulletPattern", menuName = "ScriptableObjects/New BulletPattern")]
public class BulletPattern : ScriptableObject
{
    // bullet related variables
    public int numberOfBullets;
    public List<float> listOfAngles; // 0 degrees being straight forward
    public List<float> delayBetweenBullets;
    public float bulletSpeed;


    // methods


}
