using System.Collections;
using System.Data;
using UnityEngine;

public class SlowTimeMask : Mask
{
    public SlowTimeMask()
    {
        maskName = "Slow Time Mask";
        maskDesc = "Mask that slows time for 10 seconds, and has a cooldown of 12 seconds.";
        cooldown = 12f;
        currentCooldown = 0f; // this is always just 0 at the beginning lol
        maxUses = 1;
        currentUses = maxUses; // at the beginning this should always equal the max uses.
    }

    public override void MaskAbility()
    {
        base.MaskAbility();
        SlowTime();
    }

    void SlowTime()
    {
        // so the way this is going to work is that enemies, objects, and whatever are going to have their speeds tied to a variable so that they can be manipulated individually
        // what does this mean? Well basically instead of manipulating Time.timeScale directly (which would potentially introduce a whole heap of issues, one would be messing with pausing)
        // We manipulate a variable that affects the time of enemies, objects, and anything else independently. So enemies might have their speeds, animations, look like this:
        // speed = 3 * (Multiplier), and we would just manipulate the multiplier variable rather than the timescale of the entire game.

        // Since scriptable objects can't start coroutines, there's probably going to be a GameObject with a monobehaviour script component on it that 1. holds the multiplier variable
        // and 2. holds the coroutine that actually slows the time. This function would just activate that coroutine by finding that object and going "Hey! Activate your thing."

        GameObject.Find("Data Handler").GetComponent<DataHandler>().StartCoroutine(GameObject.Find("Data Handler").GetComponent<DataHandler>().SlowTime());
    }

    public override void MaskUpdate() // checks if time is slowed, if it is then the cooldown doesn't start resetting
    {
        if (GameObject.Find("Data Handler").GetComponent<DataHandler>().timeMultiplier == 0.5f)
            currentCooldown = 0f;
        // this function is necessary because if you dont have it the mask will automatically start cooling down instead of 
        // waiting until after the effects are over.
    }
}

