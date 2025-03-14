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
        if (Input.GetKeyDown(KeyCode.F) && maskInventory.Count != 0 && maskInventory[cycleCount] != null && equippedMask.currentUses != 0) // hitting f activates the mask ability, also equippedMask must not equal null (without this there would be an reference error)
            equippedMask.MaskAbility(); // the masks ability if it has one
        if (maskInventory.Count != 0 && maskInventory[cycleCount] != null) // error prevention
        {
            foreach (Mask mask in maskInventory)
                mask.ResetUses(); // manages the cooldown for all of the masks in the list, so that even when you unequip a mask the cooldown still ticks down, or up I guess.
            equippedMask.EquippedAbility(); // Runs the function for a masks ability that would constantly update, if it has none nothing happens.
        }
    }

    public void CycleMasks()
    {

        if(maskInventory.Count != 0 && maskInventory.Count < 2) // These lines are basically placeholders for equipping a mask if you have none
        {
            equippedMask = maskInventory[cycleCount]; 
            equippedMask.OnEquip();
        }

        if (Input.GetKeyDown(KeyCode.Z) && cycleCount + 1 <= maskInventory.Count - 1 && maskInventory.Count > 1) // the second part is "If adding to the index wouldnt go over the number of items in the list"
        {
            equippedMask.OnUnequip(); // Runs the unequip method for the currently equipped mask, if it has one.
            cycleCount++;
            equippedMask = maskInventory[cycleCount]; // set the equipped mask to what the index is set to
            equippedMask.OnEquip(); // runs the equip method for the new currently equipped mask, if it has one.
            Debug.Log($"the cycleCount is currently {cycleCount}"); // sanity check
        }
        else if (Input.GetKeyDown(KeyCode.Z) && cycleCount + 1 > maskInventory.Count - 1 && maskInventory.Count > 1) // if adding to the index WOULD go over the number of items in the list
        {
            if(cycleCount != 0) // if the mask isn't already at zero
            equippedMask.OnUnequip();
            cycleCount = 0; // reset the count to zero
            equippedMask = maskInventory[cycleCount];
            equippedMask.OnEquip();
            Debug.Log($"the cycleCount is currently {cycleCount}, and it has been reset.");
        }

        if (Input.GetKeyDown(KeyCode.X) && cycleCount - 1 >= 0 && maskInventory.Count > 1) // if subtracting from the index wouldnt be less than zero
        {
            maskInventory[cycleCount].OnUnequip();
            cycleCount--;
            equippedMask = maskInventory[cycleCount];
            equippedMask.OnEquip();
            Debug.Log($"the cycleCount is currently {cycleCount}");
        }
        else if (Input.GetKeyDown(KeyCode.X) && cycleCount - 1 < 0 && maskInventory.Count > 1) //if it would be reset it to zero
        {
            if(cycleCount != 0)
            maskInventory[cycleCount].OnUnequip();
            cycleCount = 0;
            equippedMask = maskInventory[cycleCount];
            equippedMask.OnEquip();
            Debug.Log($"the cycleCount is currently {cycleCount}, and it has been reset.");
        }
    }
}
