using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    float interactRange;
    [SerializeField]
    int damage;
    [SerializeField]
    float cooldownMax;
    float currentCooldown;

    private void Update()
    {
        Cooldown();
        Attack();
    }

    public void Attack()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && hit.collider.gameObject.tag == "Enemy" && currentCooldown >= cooldownMax)
            {
                hit.collider.gameObject.GetComponent<BaseEnemy>().TakeDamage(damage);
                currentCooldown = 0f;
                Debug.Log("Player Attack!");
            }
        }
    }

    private void Cooldown()
    {
        if (currentCooldown < cooldownMax)
            currentCooldown += Time.deltaTime;
    }
}
