using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTransform : MonoBehaviour
{
    public Transform playerTransform;

    void Update()
    {
        transform.localScale = new Vector3(1/playerTransform.localScale.x,1/playerTransform.localScale.y,1/playerTransform.localScale.z);
    }
}
