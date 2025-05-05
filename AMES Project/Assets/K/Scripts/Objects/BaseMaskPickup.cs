using Unity.VisualScripting;
using UnityEngine;

public class BaseMaskPickup : MonoBehaviour
{

    public Mask maskToGive; // create another script and change this to a mask in the start function, make sure the class has "BaseMaskPickup" where "Monobehaviour" should be.

    public void OnPickup()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MaskInteraction>().maskInventory.Add(maskToGive); // find the player and add the mask to his inventory
        maskToGive.AbilityOnPickup(); // calls the masks ability that you receive on pickup, if there is one. If there isn't one then nothing happens here.
        Destroy(this.gameObject); // destroy yourself
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            OnPickup();
    }
}
