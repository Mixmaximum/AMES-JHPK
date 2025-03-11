using UnityEngine;

public class Mask : ScriptableObject
{

	public float cooldown; // The cooldown of a given mask, ex: cooldown == 30 secs
    public float currentCooldown; // The cooldown timer of a given mask, so this would be affected by time.deltatime
    public int maxUses; // The uses a given mask has.
    public int currentUses; // The uses a mask currently has available, so if maxUses == 3 and you use the ability once, then currentUses == 2 because currentUses == maxUses.
					 // after a cooldown currentUses = maxUses to reset the available uses.


	public virtual void MaskAbility()
	{
		// Ability code would go here for each individual mask.
		// Call this function when you want the player to use the ability of a mask, ex: press E to use mask ability
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
