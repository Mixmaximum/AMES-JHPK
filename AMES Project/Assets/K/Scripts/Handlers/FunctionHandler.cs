using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class FunctionHandler : MonoBehaviour
{
    void Update() // this single update function handles all of the enemies in the scene
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<BaseEnemy>().Movement();
            enemy.GetComponent<BaseEnemy>().EnemyUpdate();
            enemy.GetComponent<BaseEnemy>().Attack();
        }
    }
}
