
using UnityEngine;
using System.Collections.Generic;

public class MaskInteraction : MonoBehaviour
{

    // THIS IS A TEST SCRIPT TO TEST THE ABILITIES OF THE MASK.
    // THIS SCRIPT GOES ON THE PLAYER OBJECT

    public List<Mask> maskInventory = new List<Mask>(); // A list that holds mask objects, think of this like an inventory

    int cycleCount = 0; // the integer that controls the index of the maskInventory list, starts out at zero aka top of the list

    Mask equippedMask; // The current mask the player has equipped.


    void Update()
    {
        MaskControl();
        CycleMasks();
    }

    public void MaskControl()
    {
        if (Input.GetKeyDown(KeyCode.F) && equippedMask != null) // hitting f activates the mask ability, also equippedMask must not equal null (without this there would be an reference error)
            equippedMask.MaskAbility();
        if (equippedMask != null) // error prevention
        {
            equippedMask.ResetUses(); // Uses check for the mask, i don't know if I want to put this here in update but this is for testing soooooo
            equippedMask.EquippedAbility(); // Runs the function for the masks equipped ability, if the mask has no function for it then it does nothing (theres no error)
        }
    }

    public void CycleMasks()
    {
        if(maskInventory.Count != 0) // this prevents an index out of range error by only allowing the equipped mask to equal the index of the list only when there's something inside of the list.
        equippedMask = maskInventory[cycleCount]; // the equipped mask is whatever the index of the list is, controlled by the integer

        if (Input.GetKeyDown(KeyCode.Z) && cycleCount + 1 <= maskInventory.Count - 1) // the second part is "If adding to the index wouldnt go over the number of items in the list"
        {
            cycleCount++;
            Debug.Log($"the cycleCount is currently {cycleCount}"); // sanity check
        }
        else if (Input.GetKeyDown(KeyCode.Z) && cycleCount + 1 > maskInventory.Count - 1) // if adding to the index WOULD go over the number of items in the list
        {
            cycleCount = 0; // reset the count to zero
            Debug.Log($"the cycleCount is currently {cycleCount}, and it has been reset.");
        }

        if (Input.GetKeyDown(KeyCode.X) && cycleCount - 1 >= 0) // if subtracting from the index wouldnt be less than zero
        {
            cycleCount--;
            Debug.Log($"the cycleCount is currently {cycleCount}");
        }
        else if (Input.GetKeyDown(KeyCode.X) && cycleCount - 1 < 0) //if it would be reset it to zero
        {
            cycleCount = 0;
            Debug.Log($"the cycleCount is currently {cycleCount}, and it has been reset.");
        }
    }
}
