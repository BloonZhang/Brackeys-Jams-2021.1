using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dessert", menuName = "ScriptableObjects/New Dessert")]
public class Dessert : ScriptableObject
{
    public string name;
    public Sprite sprite;
    public int healing;
    public int specialRecovery;

}
