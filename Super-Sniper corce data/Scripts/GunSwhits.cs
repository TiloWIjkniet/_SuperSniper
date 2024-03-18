using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwhits : MonoBehaviour
{
    public List<GameObject> guns = new List<GameObject>();
    public List<KeyCode> keyCodes = new List<KeyCode>();
    public int curetnGun {get;set;}
    public AudioSource audioSource;
    public bool shwitch {get;set;}


    void Update()
    {

        if(!UnityEngine.Input.GetKey(KeyCode.Mouse1))
        {
            if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") > 0f  && curetnGun+ 1 < guns.Count) // forward
            {
                guns[curetnGun].GetComponent<Animator>().SetBool("ChangeWepon", true);
                
                curetnGun += 1;
            }
            else if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") < 0f && curetnGun - 1 >= 0) // backwards
            {
                guns[curetnGun].GetComponent<Animator>().SetBool("ChangeWepon", true);
                curetnGun -= 1;
            }
        }

        for (int i = 0; i < keyCodes.Count; i++)
        {
            if(Input.GetKeyDown(keyCodes[i]))
            {
                guns[curetnGun].GetComponent<Animator>().SetBool("ChangeWepon", true);
                curetnGun = i;
                
            }
        }
    }

    public void SetPlus()
    {
        shwitch = true;
        guns[curetnGun].GetComponent<Animator>().SetBool("ChangeWepon", false);
        audioSource.Play();
        
        for (int i = 0; i < guns.Count; i++)
        {

            guns[i].SetActive(false);   
        }
        guns[curetnGun].SetActive(true);
        
    }


}
