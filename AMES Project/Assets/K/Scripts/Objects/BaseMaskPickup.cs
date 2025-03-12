using UnityEngine;

public class BaseMaskPickup : MonoBehaviour
{

    public Mask maskToGive; // create another script and change this to a mask in the start function, make sure the class has "BaseMaskPickup" where "Monobehaviour" should be.

    public void OnPickup()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<MaskInteraction>().maskInventory.Add(maskToGive); // find the player and add the mask to his inventory
        Destroy(this.gameObject); // destroy yourself
    }
}
