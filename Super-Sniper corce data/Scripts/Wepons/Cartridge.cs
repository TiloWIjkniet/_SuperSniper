using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartridge : MonoBehaviour
{
    public float destoyTime = 10;
    public LayerMask layerMask;
    public AudioClip[] audioClip;
    public AudioSource audioSource;
    void Start()
    {
        Destroy(this.gameObject, destoyTime);
    }

    void Update()
    {
        if(Physics.CheckSphere(transform.position, 0.1f, layerMask))
        {

            transform.parent = null;
            transform.localScale = Vector3.one;
            audioSource.clip = audioClip[Random.Range(0,audioClip.Length)];
            audioSource.pitch *= Random.Range(0.9f,1.1f);
            audioSource.volume*= Random.Range(0.8f,1.2f);
            audioSource.Play();
            Destroy(this);
        }
    }

}
