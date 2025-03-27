using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] GameObject cam;

    [Header("Health Settings")]
    [SerializeField] float health;
    [SerializeField] float healAmount;
    [SerializeField] float timeToHeal;
    [Space(5)]

    [Header("I-Frame Settings")]
    [SerializeField] float invincibilityDuration = 2f;
    [Space(5)]

    float invincibilityTimeRemaining = 0f;
    bool isInvincible = false;
    float healTimer;
    float maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        HandleIFrames();
        HealOverTime();


    }
    public void TakeDamage(float damage)
    {
        if (!isInvincible) 
        {
            health -= damage;
            isInvincible = true;
            invincibilityTimeRemaining = invincibilityDuration;
            if (health <= 0)
            {
                Die();
            }
            healTimer = timeToHeal;
        }
    }

    public void Heal(float healing) 
    {
        health += healing;
    }

    private void HealOverTime()
    {
        if (healTimer > 0)
        {
            healTimer -= Time.deltaTime;

            if (healTimer == 0)
            {
                Heal(healAmount);

                if (health != maxHealth)
                {
                    healTimer = timeToHeal;
                }
            }
        }
    }

    public void Die()
    {
        cam.GetComponent<Animator>().SetBool("Dead", true);
        GetComponent<Rigidbody>().
    }
    private void HandleIFrames()
    {
        if (isInvincible)
        {
            invincibilityTimeRemaining -= Time.deltaTime;

            if (invincibilityTimeRemaining <= 0)
            {
                isInvincible = false;
                //Debug.Log("No longer invincible");
            }
        }
   
    }
}
