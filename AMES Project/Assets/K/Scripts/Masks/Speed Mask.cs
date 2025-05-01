using UnityEngine;

public class SpeedMask : Mask
{
    public SpeedMask()
    {
        maskName = "Speed Boost Mask";
        maskDesc = "A mask that boosts your speed for 15 seconds when you hit F---has a cooldown of 30 seconds";
        currentUses = 1;
        maxUses = 1;
        currentCooldown = 0f;
        cooldown = 15f;
    }

    bool isAble;
    float speedCooldown;

    public override void MaskAbility()
    {
        base.MaskAbility();
        isAble = true;
    }


    public override void MaskUpdate()
    {
        base.MaskUpdate();
        if (isAble && GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().playerTimeSlowMultiplier != 0.5f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().walkSpeed = 12f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().sprintSpeed = 20f;
            speedCooldown += Time.deltaTime;
        }
        else if (!isAble || GameObject.FindGameObjectWithTag("Handler").GetComponent<DataHandler>().playerTimeSlowMultiplier == 0.5f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().walkSpeed = 7f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().sprintSpeed = 10f;
        }
        if (speedCooldown >= 15)
        {
            isAble = false;
            speedCooldown = 0;
        }

        if (isAble)
            currentCooldown = 0f;
    }

    public override void MaskOnStart()
    {
        base.MaskOnStart();
        maskIcon = Resources.Load<Sprite>("2");
    }

    public override void AbilityOnPickup()
    {
        base.AbilityOnPickup();
        maskIcon = Resources.Load<Sprite>("2");
    }
}

