using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeponInvo : MonoBehaviour
{
 
    TextMeshProUGUI text;
    Transform slider1;
    Transform slider2;
    Image image;
    void Awake()
    {
        text= transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        slider1 = transform.GetChild(2);
        slider2 = slider1.GetChild(1);
        image = slider2.GetChild(0).GetComponent<Image>();
    }

    public void SetInvo(string textWeponInvot, float slider1Sclae, float slider2Sclae)
    {
        text.text = textWeponInvot;
        slider1.localScale = new Vector3(slider1Sclae,1,1);

        image.color = (slider2Sclae < 0? new Color(255, 0, 0, 0.2f): new Color(0,255,0,0.2f));
        slider2.localScale = new Vector3(slider2Sclae,1,1);
    }
}
