using UnityEngine;

public class SlowTimeMaskOBJ : BaseMaskPickup
{
    void Start()
    {
        maskToGive = ScriptableObject.CreateInstance<SlowTimeMask>();
    }
}
