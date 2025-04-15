using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int maxHealth = 100;
    [SerializeField] public bool dead;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            dead = true;
        Debug.Log($"The player took ({damage}) out of {health}, and dead is {dead}");
    }
}
