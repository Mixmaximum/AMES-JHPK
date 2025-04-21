using UnityEngine;

public class SpeedMask : Mask
{
    public SpeedMask()
    {
        maskName = "Speed Boost Mask";
        maskDesc = "A mask that boosts your speed as long as you have it equipped.";
        currentUses = 1;
        maxUses = 1;
        currentCooldown = 0f;
        cooldown = 30f;
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
        if (isAble)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().walkSpeed = 12f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().sprintSpeed = 20f;
            speedCooldown += Time.deltaTime;
        }
        else if (!isAble)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().walkSpeed = 7f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().sprintSpeed = 10f;
        }
        if (speedCooldown >= 15)
        {
            isAble = false;
            speedCooldown = 0;
        }
    }
}

