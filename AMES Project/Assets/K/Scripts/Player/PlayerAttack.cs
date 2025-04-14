using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    int health;
    int maxHealth;
    bool dead;

    public void TakeDamage(int damage)
    {
        health = -damage;
        if (health <= 0)
            dead = true;
    }
}
