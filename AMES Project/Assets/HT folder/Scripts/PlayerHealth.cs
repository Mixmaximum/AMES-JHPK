using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float healAmount;
    [SerializeField] float timeToHeal;
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
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        healTimer = timeToHeal;
    }

    public void Heal(float healing) 
    {
        health += healing;
    }
}
