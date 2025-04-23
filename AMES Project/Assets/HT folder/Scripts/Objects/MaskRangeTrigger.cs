using UnityEngine;

public class MaskRangeTrigger : MonoBehaviour
{
    [SerializeField] QuestPopUp qpp;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Collider")
        {
            qpp.withinMaskRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player Collider")
        {
            qpp.withinMaskRange = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player Collider")
        {
            qpp.withinMaskRange = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player Collider")
        {
            qpp.withinMaskRange = false;
        }
    }
}
