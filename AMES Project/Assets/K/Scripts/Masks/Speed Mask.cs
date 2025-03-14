using UnityEngine;

public class SpeedMask : Mask
{
    public SpeedMask()
    {
        maskName = "Speed Boost Mask";
        maskDesc = "A mask that boosts your speed as long as you have it equipped.";
    }

    public override void EquippedAbility()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().walkSpeed = 7;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().sprintSpeed = 9;
    }

    public override void OnUnequip()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().walkSpeed = 4;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().sprintSpeed = 6;
    }
}
