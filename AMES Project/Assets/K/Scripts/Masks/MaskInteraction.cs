
using UnityEngine;
using System.Collections.Generic;

public class MaskInteraction : MonoBehaviour
{

    // THIS IS A TEST SCRIPT TO TEST THE ABILITIES OF THE MASK.
    // THIS SCRIPT GOES ON THE PLAYER OBJECT

    List<Mask> maskInventory = new List<Mask>(); // A list that holds mask objects, think of this like an inventory

    int cycleCount; // the integer that controls the index of the maskInventory list

    Mask equippedMask; // The current mask the player has equipped.

    void Start()
    {
        maskInventory.Add(ScriptableObject.CreateInstance<MaskTest>()); // Add a mask to the mask inventory
        maskInventory.Add(ScriptableObject.CreateInstance<MaskTest1>());
        cycleCount = 0; // the index of the inventory equals 0 (so the index isn't immediately out of range)
    }

    // Update is called once per frame
    void Update()
    {
        equippedMask = maskInventory[cycleCount]; // the equipped mask is whatever the index of the list is, controlled by the integer
        if (Input.GetKeyDown(KeyCode.F)) // hitting f activates the mask ability
            equippedMask.MaskAbility();
        equippedMask.ResetUses(); // Uses check for the mask, i don't know if I want to put this here in update but this is for testing soooooo
        CycleMasks();
    }

    public void CycleMasks()
    {
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
