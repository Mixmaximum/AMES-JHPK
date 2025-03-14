using UnityEngine;

public class DoubleJumpMaskOBJ : BaseMaskPickup
{
    private void Start()
    {
        maskToGive = ScriptableObject.CreateInstance<DoubleJumpMask>();
    }
}
