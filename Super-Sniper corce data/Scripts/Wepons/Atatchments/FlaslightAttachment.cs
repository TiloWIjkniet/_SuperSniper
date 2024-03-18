using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaslightAttachment : Attachments1
{
    public GameObject flaslight;
    public AudioSource audioSource;
    bool flaslightOn = true;

    void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.F))
        {
            flaslightOn = !flaslightOn;
            audioSource.Play();
            StartCoroutine(SetActiveTime(flaslight,flaslightOn, 0.2f));
        }
        

    }

    IEnumerator SetActiveTime(GameObject gameObject, bool value, float Time)
    {
        yield return new WaitForSeconds(Time);
        gameObject.SetActive(value);
    }
}
