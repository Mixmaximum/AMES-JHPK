using UnityEngine;

public class DebugLogMask : BaseMaskPickup
{
    private void Start()
    {
        maskToGive = ScriptableObject.CreateInstance<MaskTest>(); // you can't put this in the constructor, so it has to go in start.
    }
}
