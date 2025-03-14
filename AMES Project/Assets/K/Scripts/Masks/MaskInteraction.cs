using UnityEngine;
using System.Collections.Generic;

public class MaskInteraction : MonoBehaviour
{

    // THIS IS A TEST SCRIPT TO TEST THE ABILITIES OF THE MASK.
    // THIS SCRIPT GOES ON THE PLAYER OBJECT

    public List<Mask> maskInventory = new List<Mask>(); // A list that holds mask objects, think of this like an inventory

    int cycleCount = 0; // the integer that controls the index of the maskInventory list, starts out at zero aka top of the list

    Mask equippedMask; // the currently equipped mask

    void Update()
    {
        CycleMasks();
        MaskControl();
    }

    public void MaskControl()
    {
        if (Input.GetKeyDown(KeyCode.F) && maskInventory.Count != 0 && maskInventory[cycleCount] != null) // hitting f activates the mask ability, also equippedMask must not equal null (without this there would be an reference error)
            equippedMask.MaskAbility(); // the masks ability if it has one
        if (maskInventory.Count != 0 && maskInventory[cycleCount] != null) // error prevention
        {
            equippedMask.EquippedAbility(); // Runs the function for the masks equipped ability, if the mask has no function for it then it does nothing (theres no error). Maybe wouldnt want this in update
            equippedMask.ResetUses(); // Uses check for the mask, i don't know if I want to put this here in update but this is for testing soooooo
        }
    }

    public void CycleMasks()
    {

        if(maskInventory.Count != 0 && maskInventory.Count < 2)
        equippedMask = maskInventory[cycleCount];

        if (Input.GetKeyDown(KeyCode.Z) && cycleCount + 1 <= maskInventory.Count - 1 && maskInventory.Count > 1) // the second part is "If adding to the index wouldnt go over the number of items in the list"
        {
            equippedMask.OnUnequip(); // Runs the unequip method for the mask
            cycleCount++;
            equippedMask = maskInventory[cycleCount]; // set the equipped mask to what the index is set to
            Debug.Log($"the cycleCount is currently {cycleCount}"); // sanity check
        }
        else if (Input.GetKeyDown(KeyCode.Z) && cycleCount + 1 > maskInventory.Count - 1 && maskInventory.Count > 1) // if adding to the index WOULD go over the number of items in the list
        {
            if(cycleCount != 0) // if the mask isn't already at zero
            equippedMask.OnUnequip();
            cycleCount = 0; // reset the count to zero
            equippedMask = maskInventory[cycleCount];
            Debug.Log($"the cycleCount is currently {cycleCount}, and it has been reset.");
        }

        if (Input.GetKeyDown(KeyCode.X) && cycleCount - 1 >= 0 && maskInventory.Count > 1) // if subtracting from the index wouldnt be less than zero
        {
            maskInventory[cycleCount].OnUnequip();
            cycleCount--;
            equippedMask = maskInventory[cycleCount];
            Debug.Log($"the cycleCount is currently {cycleCount}");
        }
        else if (Input.GetKeyDown(KeyCode.X) && cycleCount - 1 < 0 && maskInventory.Count > 1) //if it would be reset it to zero
        {
            if(cycleCount != 0)
            maskInventory[cycleCount].OnUnequip();
            cycleCount = 0;
            equippedMask = maskInventory[cycleCount];
            Debug.Log($"the cycleCount is currently {cycleCount}, and it has been reset.");
        }
    }
}
