using UnityEngine;
using UnityEngine.Rendering;

public class SpeedMaskOBJ : BaseMaskPickup
{
    void Start()
    {
        maskToGive = ScriptableObject.CreateInstance<SpeedMask>();   
    }
}
