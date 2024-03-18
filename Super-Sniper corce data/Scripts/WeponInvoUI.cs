using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeponInvoUI : MonoBehaviour
{
    public GunSwhits gunSwhits;
    public List<Gun> guns = new List<Gun>();
    public List<GameObject> gameObjects = new List<GameObject>();
    public TextMeshProUGUI maxsBullets;
    public TextMeshProUGUI currentBullets;
    public TextMeshProUGUI allBullets;

    Gun curentGun;
    void Start()
    {
        Switch();
    }
    void Update()
    {
        if(gunSwhits.shwitch) Switch();
        maxsBullets.text = curentGun.ammunitionCapacity.ToString();
        currentBullets.text = curentGun.CurrentAmmoCapacity.ToString();
        allBullets.text = "999";
        
    }

    void Switch()
    {
        gunSwhits.shwitch = false;
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(false);
        }
        curentGun = guns[gunSwhits.curetnGun];
        gameObjects[gunSwhits.curetnGun].SetActive(true);
    }


    
}
