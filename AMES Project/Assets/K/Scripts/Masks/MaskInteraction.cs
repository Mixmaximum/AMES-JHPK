using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class MaskInteraction : MonoBehaviour
{

    // THIS IS A TEST SCRIPT TO TEST THE ABILITIES OF THE MASK.
    // THIS SCRIPT GOES ON THE PLAYER OBJECT

    public List<Mask> maskInventory = new List<Mask>(); // A list that holds mask objects, think of this like an inventory

    int cycleCount = 0; // the integer that controls the index of the maskInventory list, starts out at zero aka top of the list

    Mask equippedMask; // the currently equipped mask

    [SerializeField] bool GiveMePlease;
    [SerializeField] TextMeshProUGUI maskEquipText; // text that shows what mask you currently have equipped.
    [SerializeField] Image maskCooldown;
    [SerializeField] Image maskIconImage;
    [SerializeField] bool Level2Bool;

    void Update()
    {
        CycleMasks();
        MaskControl();
        if(equippedMask != null)
        maskCooldown.fillAmount = equippedMask.currentCooldown / equippedMask.cooldown;

        if(equippedMask != null && equippedMask.currentUses >= 1)
            maskCooldown.fillAmount = 100;
        if (equippedMask != null && equippedMask.currentCooldown == 0f && equippedMask.currentUses != 1)
            maskCooldown.fillAmount = 0;
        
    }

    private void Start()
    {
       GiveAllMasks();
        Level2Masks();
        foreach (Mask mask in maskInventory)
       mask.MaskOnStart();

       if(maskInventory.Count == 0)
            maskIconImage.enabled = false;
    }

    public void MaskControl()
    {
        if (Input.GetKeyDown(KeyCode.E) && maskInventory.Count != 0 && maskInventory[cycleCount] != null && equippedMask.currentUses != 0) // hitting f activates the mask ability, also equippedMask must not equal null (without this there would be an reference error)
            equippedMask.MaskAbility(); // runs the masks F ability, if it has one
        if (maskInventory.Count != 0 && maskInventory[cycleCount] != null) // error prevention
        {
            foreach (Mask mask in maskInventory)
            {
                mask.ResetUses(); // manages the cooldown for all of the masks in the list, so that even when you unequip a mask the cooldown still ticks down, or up I guess.
                mask.MaskUpdate(); // manages code for masks that is supposed to be always running, even if its not equipped
            }
            equippedMask.EquippedUpdate(); // Runs the method for an equipped masks ability that would constantly update, if it has none nothing happens.
        }
    }

    public void CycleMasks()
    {

        if(maskInventory.Count != 0 && equippedMask == null && maskInventory[0] != null) // These lines are basically placeholders for equipping a mask if you have none
        {
            maskIconImage.enabled = true;
            equippedMask = maskInventory[0]; 
            equippedMask.OnEquip();
            maskIconImage.sprite = equippedMask.GetIcon();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && cycleCount + 1 <= maskInventory.Count - 1 && maskInventory.Count > 1) // the second part is "If adding to the index wouldnt go over the number of items in the list"
        {
            equippedMask.OnUnequip(); // Runs the unequip method for the currently equipped mask, if it has one.
            cycleCount++; // cycles the count up, so that you can switch masks. It works the same way when you subtract
            equippedMask = maskInventory[cycleCount]; // set the equipped mask to what the index is set to
            equippedMask.OnEquip(); // runs the equip method for the new currently equipped mask, if it has one.
            maskIconImage.sprite = equippedMask.GetIcon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && cycleCount + 1 > maskInventory.Count - 1 && maskInventory.Count > 1) // if adding to the index WOULD go over the number of items in the list
        {
            if(cycleCount != 0) // if the mask isn't already at zero
            equippedMask.OnUnequip();
            cycleCount = 0; // reset the count to zero
            equippedMask = maskInventory[cycleCount];
            equippedMask.OnEquip();
            maskIconImage.sprite = equippedMask.GetIcon();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && cycleCount - 1 >= 0 && maskInventory.Count > 1) // if subtracting from the index wouldnt be less than zero
        {
            maskInventory[cycleCount].OnUnequip();
            cycleCount--;
            equippedMask = maskInventory[cycleCount];
            equippedMask.OnEquip();
            maskIconImage.sprite = equippedMask.GetIcon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && cycleCount - 1 < 0 && maskInventory.Count > 1) //if it would be reset it to zero
        {
            if(cycleCount != 0)
            maskInventory[cycleCount].OnUnequip();
            cycleCount = maskInventory.Count - 1;
            equippedMask = maskInventory[maskInventory.Count-1];  
            equippedMask.OnEquip();
            maskIconImage.sprite = equippedMask.GetIcon();
        }
    }

    private void GiveAllMasks()
    {
        if(GiveMePlease)
        {
            maskInventory.Add(ScriptableObject.CreateInstance<SlowTimeMask>());
            maskInventory.Add(ScriptableObject.CreateInstance<SpeedMask>());
            maskInventory.Add(ScriptableObject.CreateInstance<DoubleJumpMask>());
        }
    }
    private void Level2Masks()
    {
        if(Level2Bool)
        {
            maskInventory.Add(ScriptableObject.CreateInstance<SlowTimeMask>());
            maskInventory.Add(ScriptableObject.CreateInstance<DoubleJumpMask>());
        }
    }
}
