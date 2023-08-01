using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]

public class EnemyStats : ScriptableObject
{

    public string className;
    public string description;

    public float health;
    public float attackAmount;
    public float speed;
  

    public GameObject hitEffect;
    
}
