using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NUnit.Framework;

public class QuestPopUp : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MaskInteraction mi;
    [SerializeField] private TextMeshProUGUI missionText;

    [Header("GameObjects")]
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject maskBarrier;
    

    [Header("Quests")]
    [SerializeField] private string firstPrompt; //for when you start the game and need to collect mask
    [SerializeField] private string pickUpMaskPrompt;
    [SerializeField] private string secondPrompt; // when you need to kill enemies
    [SerializeField] private string thirdPrompt; // when you need to fight boss

    public bool withinMaskRange;
    public bool maskPickedUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mi.maskInventory.Count > 0)
        {
            maskPickedUp = true;
            barrier.SetActive(false);
        }

        if (mi.maskInventory.Count == 0 && withinMaskRange == false)
        {
            missionText.text = ("Mission: ") + firstPrompt;
        }
        else if (withinMaskRange)
        {
            missionText.text = ("Mission: ") + pickUpMaskPrompt;
        }

        if (maskPickedUp && GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            missionText.text = ("Mission: ") + secondPrompt + GameObject.FindGameObjectsWithTag("Enemy").Length + ("left");
        }
        else if (maskPickedUp && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            maskBarrier.SetActive(false);
            missionText.text = ("Mission: ") + thirdPrompt;
        }

        
    }

}
