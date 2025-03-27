using UnityEngine;

public class DeathCanvasActive : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void ActivateCanvas()
    {
        canvas.enabled = true;
    }
}
