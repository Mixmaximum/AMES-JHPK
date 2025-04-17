using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public string enemyName; // Name of Enemy
    public int maxHealth; // Max Health of Enemy
    public int health; // Current Health of Enemy, On Start This Is maxHealth
    [SerializeField]
    public float speed; // Movement Speed of Enemy
    public int damage; // Damage of Enemy
    public bool isDead; // Death State of Enemy
    // Set these values in the constructor when you make a new enemy script by calling YourScriptName()


    public virtual void Movement()
    {
        // Put code here for their movements, so you would write any navmesh stuff here.
    }

    public void TakeDamage(int damageToTake) // The players attack script would call this script in any enemies they hit, damageToTake is obviously filled by the players damage val
    {
        health -= damageToTake; // self explanatory, but I'll comment anyway. Remove from the enemies health value the damage from player.
        if (health <= 0)
        {
            isDead = true;
            OnDeath(); // Call the OnDeath() function to run any code that we may want when an enemy dies
        }
    }

    public virtual void Attack()
    {
        // Put here code for enemies and their attacks, like melee attacking and whatnot.
        // Player detection within a certain range, etc etc.
        // Basically anything under the umbrella of "Enemy Attacking"
    }

    public virtual void EnemyUpdate()
    {
        // Put here any code that you want to be continuously running for any enemy.
        // This could be checks for things like their current state, their cooldown, etc.
        // Movement code, attack code, and whatever else would not go here. Please keep it organized, K.
    }

    public virtual void OnDeath()
    {
        // Put here any code that you want to execute when the enemy dies.
        // Like dropping coins or a mask or whatever
        
    }

    public virtual void Knockback()
    {

    }
}
