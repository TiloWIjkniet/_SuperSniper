using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Distacen : MonoBehaviour
{
    TextMeshProUGUI text;
    public Transform player;
    void Start()
    {   
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = ((int)Vector3.Distance(transform.position, player.position)).ToString() +"m";
    }
}
