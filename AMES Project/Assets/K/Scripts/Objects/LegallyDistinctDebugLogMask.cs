using UnityEngine;

public class LegallyDistinctDebugLogMask : BaseMaskPickup
{
    private void Start()
    {
        maskToGive = ScriptableObject.CreateInstance<MaskTest1>();
    }
}
