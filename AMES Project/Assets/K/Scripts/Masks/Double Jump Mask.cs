using UnityEngine;

public class DoubleJumpMask : Mask
{
    public DoubleJumpMask()
    {
        maskName = "Double Jump Mask";
        maskDesc = "A mask that makes you jump regardless of where you are when you hit F---has a cooldown of 3 seconds.";
        cooldown = 3f;
        currentCooldown = 0f;
        maxUses = 1;
        currentUses = 1;
    }

    public override void MaskAbility()
    {
        if(!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().isSliding || !GameObject.FindGameObjectWithTag("Player").GetComponent<WallRun>().wallRunning)
        base.MaskAbility();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(player.transform.up * 10f, ForceMode.Impulse); // we can always change how high the player can go.
    }
}
