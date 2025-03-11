using UnityEngine;

public class MaskTest1 : Mask
{

    public MaskTest1() // Constructor for the variables of the mask.
    {
        maxUses = 1; // The maximum amount of uses this mask has
        currentUses = 1; // The current amount of uses this mask has, generally in the constructor this always wants to be set to the maxUses.
        cooldown = 5f; // The cooldown of this mask.
        currentCooldown = 0f; // The current build up of the cooldown this mask has, set this to zero in the constructor because this is the variable that's added to with Time.deltaTime.
    }

    public override void MaskAbility()
    {
        if (currentUses != 0) // If you have mask uses
        {
            Debug.Log("Legally Distinct Ability"); // Do something
            currentUses--;
        }
    }
       
}

