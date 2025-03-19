using System.Collections;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public float timeMultiplier = 1f;

    public IEnumerator SlowTime()
    {
        Debug.Log("Time Slowed");
        timeMultiplier = 0.5f;
        yield return new WaitForSeconds(10f);
        timeMultiplier = 1f;
        Debug.Log("Time Unslowed");
    }
}
