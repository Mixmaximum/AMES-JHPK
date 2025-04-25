using UnityEngine;

public class Greg : BaseEnemy
{
    public Greg()
    {
        enemyName = "Greg: Genetically Reprogrammed Enforcement Golem";
        maxHealth = 1000;
        health = maxHealth;
        speed = 7f;
        damage = 50;
        isDead = false;
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
