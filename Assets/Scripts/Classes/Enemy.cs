using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/New Enemy")]
public class Enemy : ScriptableObject
{
    // Basic variables
    public string name;
    public int maxHealth;
    public int attackStrength;
    public bool invincibleWhileIdle;
    public float shotCooldown;
    public float aggroDistance;

    // Complex variables
    public Sprite sprite;
    public Vector2 shootPointOffset;
    // TODO: animation
    public GameObject bulletPrefab;

    // Other scriptable objects
    public BulletPattern bulletPattern;
    // public AIPattern idleAI;
    // public AIPattern aggroAI;

}
