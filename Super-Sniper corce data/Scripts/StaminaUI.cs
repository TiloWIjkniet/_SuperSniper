using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaUI : MonoBehaviour
{
    public Brains_Player brains_Player;
    public GameObject staminaUI;
    public RectTransform staminaSlider;

    void Update()
    {
        if(brains_Player.currentStamina >= brains_Player.maxsStamin)
        {
            staminaUI.SetActive(false);
        }
        else
        {

            staminaSlider.localScale = new Vector3(brains_Player.currentStamina/ brains_Player.maxsStamin,1,1);
            staminaUI.SetActive(true);
        }


    }
}
