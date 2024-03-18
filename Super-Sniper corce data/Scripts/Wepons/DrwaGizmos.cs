using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrwaGizmos : MonoBehaviour
{
void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.01f);
    }
}
