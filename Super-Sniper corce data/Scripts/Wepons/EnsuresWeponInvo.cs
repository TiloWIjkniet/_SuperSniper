using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnsuresWeponInvo : MonoBehaviour
{
    public Gun weapon;
    public TextMeshProUGUI text;
    public List<WeponInvo> WeponInvos = new List<WeponInvo>();

    public GameObject FrameImag;
    public GameObject[] berrelImmag;
    public GameObject[] scopeImmag;
    public GameObject[] underGunImmag;

    void Start()
    {
        UpdatUI();
    }

    public void UpdatUI()
    {


        text.text = weapon.gameObject.name;
        WeponInvos[0].SetInvo("Damage "             ,(weapon.damage -  weapon.damageMin)/ 200, (weapon.damageMin)/ 200);
        WeponInvos[1].SetInvo("Range "              ,weapon.range - weapon.rangeMin,  weapon.rangeMin);
        WeponInvos[2].SetInvo("Acuration: "         ,weapon.acuration - weapon.acurationMin, weapon.acurationMin);
        WeponInvos[3].SetInvo("Manageable "         ,1 - weapon.manageable -weapon.manageableMin, weapon.manageableMin);
        WeponInvos[4].SetInvo("Fire Rate "          ,weapon.fireRate - weapon.fireRateMin, weapon.fireRateMin);
        WeponInvos[5].SetInvo("Capacity " ,((float)weapon.ammunitionCapacity -  (float)weapon.ammunitionCapacityMin)/ 40f, (float)weapon.ammunitionCapacityMin/ 40);
        WeponInvos[6].SetInvo("Noce " ,     weapon.noice -  weapon.noiceMin,weapon.noiceMin);

        if(berrelImmag.Length <= 0) return;
        setTrue(berrelImmag, weapon.curentBerrelAttachment);
        setTrue(scopeImmag, weapon.curentScopeAttachment);
        setTrue(underGunImmag, weapon.curentUnderGunAttachment);
        FrameImag.SetActive((weapon.curentScopeAttachment+ weapon.curentUnderGunAttachment == 0) ? false : true);

    }
    void setTrue(GameObject[] list, int x)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i].SetActive(x == i);
        }
    }
}
