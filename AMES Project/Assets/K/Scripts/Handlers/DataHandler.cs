using System.Collections;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public float timeMultiplier = 1f; // this is the variable that controls the time that things like enemies go by
    public float timeSlowDuration = 7f;
    public float playerTimeSlowDuration = 15f;
    [SerializeField] public float playerTimeSlowMultiplier = 1f;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public IEnumerator SlowTime()
    {
        Debug.Log("Time Slowed");
        timeMultiplier = 0.5f; // since enemy speed and whatnot is multiplied by this variable, it slows down independently of the actual time.timescale
        yield return new WaitForSeconds(timeSlowDuration);
        timeMultiplier = 1f;
        Debug.Log("Time Unslowed");
        StopCoroutine(SlowTime());
    }

    public IEnumerator GregSlowTime()
    {

        Debug.Log("Greg slowed time!");
        player.GetComponent<PlayerMovement>().movementMultiplier = 3;
        playerTimeSlowMultiplier = 0.5f;
        yield return new WaitForSeconds(playerTimeSlowDuration);
        player.GetComponent<PlayerMovement>().movementMultiplier = 9;
        playerTimeSlowMultiplier = 1f;
        Debug.Log("Greg unslowed time!");
        StopCoroutine(GregSlowTime());
    }

    // having an independent multiplier that handles the time individually of the time.timescale variable also has the added
    // bonus of having more compatability with things like a main menu
    // as you don't have to worry about the time unslowing down when you pause and unpause
}
