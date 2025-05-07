using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] Image healthBar;

    [Header("Health Settings")]
    [SerializeField] public float health;
    [SerializeField] float healAmount;
    [SerializeField] float timeToHeal;
    [Space(5)]

    [Header("I-Frame Settings")]
    [SerializeField] float invincibilityDuration = 2f;
    [Space(5)]

    public GameObject respawnPoint;

    float invincibilityTimeRemaining = 0f;
    bool isInvincible = false;
    public float healTimer;
    float maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;
        healthBar.fillAmount = health / maxHealth;
        respawnPoint = GameObject.FindGameObjectWithTag("Starting Respawn Point");
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
            healthBar.fillAmount = health / maxHealth;
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
        healthBar.fillAmount = health / maxHealth;
    }

    private void HealOverTime()
    {
        if (healTimer > 0)
        {
            healTimer -= Time.deltaTime;

            if (healTimer <= 0)
            {
                if (health + healAmount > maxHealth)
                {
                    health = maxHealth;
                    healthBar.fillAmount = health / maxHealth;
                }
                else
                {
                    Heal(healAmount);
                }

                if (health < maxHealth)
                {
                    healTimer = timeToHeal;
                }
            }
        }
    }

    public void Die()
    {
        health = maxHealth;
        healthBar.fillAmount = health / maxHealth;
        transform.position = respawnPoint.transform.position;
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
