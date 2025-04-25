using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public float switchTime = 5f;

    private bool switched = false;

    void Start()
    {
        if (camera1 != null && camera2 != null)
        {
            camera1.enabled = true;
            camera2.enabled = false;
            Invoke("SwitchCameras", switchTime);
        }
    }

    void SwitchCameras()
    {
        if (!switched && camera1 != null && camera2 != null)
        {
            camera1.enabled = false;
            camera2.enabled = true;
            switched = true;
        }
    }
}
