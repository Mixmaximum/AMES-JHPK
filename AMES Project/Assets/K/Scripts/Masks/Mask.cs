using UnityEngine;

public class Mask : ScriptableObject
{

    public string maskName; // name of the mask
    public string maskDesc; // desc of the mask
	public float cooldown; // The cooldown of a given mask, ex: cooldown == 30 secs
    public float currentCooldown; // The cooldown timer of a given mask, so this would be affected by time.deltatime
    public int maxUses; // The uses a given mask has.
    public int currentUses; // The uses a mask currently has available, so if maxUses == 3 and you use the ability once, then currentUses == 2 because currentUses == maxUses.
                            // after a cooldown currentUses = maxUses to reset the available uses.


	public virtual void MaskAbility()
	{
        currentUses--; // Every mask loses a use upon.. using it. Instead of adding currentUses--; to every MaskAbility function, just call base.MaskAbility();
		// Ability code would go here for each individual mask.
		// Call this function when you want the player to use the ability of a mask, ex: press E to use mask ability
        // To put this in a new mask script, you would write "public override void MaskAbility()" and it should autofill it for you.
        // ^ If that does not work, make sure that the script has "Mask" where "Monobehaviour" would usually be on the script. 
	}

    public virtual void EquippedAbility()
    {
        // This function would be called when the player has the mask currently equipped.
        // Basically this function would be useful for when you want to have a mask with an ability that you dont want the player to constantly activate.
        // For example, if you want the player to have a double jump mask, you could write it so that when the player has the mask equipped they have a double jump.
        // If you want the player to increase their speed when they have it equipped, you would write it here.
    }

    public virtual void AbilityOnPickup()
    {
        // This function would be called when you pick up a mask.
        // This could potentially be used for static buffs to the player, like more health or speed.
        // You wouldn't use this for abilities you would want to activate with a key, or abilities you would want when the player has the mask equipped.
        // Generally if you're using this function then this mask wouldn't necessarily be equippable at all since it would just be a static buff.
    }

    public void ResetUses()
    {
        if (currentCooldown < cooldown && currentUses == 0) // if there are no uses left and the currentCooldown hasn't met the maximum cooldown
            currentCooldown += Time.deltaTime; // add to it incrementally using the time.

        if (currentCooldown >= cooldown) // if the cooldown has built up to the maximum cooldown
        {
            currentUses = maxUses; // Reset the uses
            currentCooldown = 0f; // Set the cooldown build up to zero.
        }
    }
}
