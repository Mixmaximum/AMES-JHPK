using UnityEngine;

public class MaskTest : Mask // Inherits from the mask script so that it can have access to the same variables and methods. That does not mean that it affects the original mask script, they are individual.
{

    public MaskTest() // Constructor for the variables of the mask.
    {
        maskName = "DebugLog Mask";
        maskDesc = "A mask used for testing the mask system, sends things to the Unity Console.";
        maxUses = 1; // The maximum amount of uses this mask has
        currentUses = 1; // The current amount of uses this mask has, generally in the constructor this always wants to be set to the maxUses.
        cooldown = 5f; // The cooldown of this mask.
        currentCooldown = 0f; // The current build up of the cooldown this mask has, set this to zero in the constructor because this is the variable that's added to with Time.deltaTime.
    }

    public override void MaskAbility()
    {
        if (currentUses != 0) // If you have mask uses
        {
            base.MaskAbility();
            Debug.Log("Ability"); // Do something
        }
    }

    public override void EquippedUpdate()
    {
        Debug.Log("Ability When Equipped Always Updating"); // This is always running when you have the mask equipped, when you take it off it ceases.
    }

}

