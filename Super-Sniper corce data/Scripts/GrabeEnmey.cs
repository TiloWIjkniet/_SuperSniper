using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabeEnmey : MonoBehaviour
{
    public LayerMask layerMask;
    public Brains_Player brains_Player;
    Transform hitColTransform;
    bool grabe;

    void Update()
    {
        Grabe();
        if(hitColTransform != null && grabe)
        {
            Vector3 pos = brains_Player.Camera.transform.position + brains_Player.Camera.transform.forward * 3;
            RaycastHit hit;
            Physics.Raycast(pos + Vector3.up * 4, -Vector3.up, out hit, 4, layerMask);
           pos.y = hit.point.y + 0.2f;
            hitColTransform.transform.position = pos;
        }
    }

    void Grabe()
    {
        RaycastHit hit;

        if(grabe == false && Input.GetKeyDown(KeyCode.E) && Physics.Raycast(brains_Player.Camera.transform.position ,brains_Player.Camera.transform.forward,  out hit, 3))
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.C))
            {
                return;
            }
      
            if(hit.collider.tag == "Enemy")
            {
                if (hit.collider.GetComponentInParent<Enemy>() != null) return;

                hitColTransform = hit.collider.transform;
                grabe = true;

                Rigidbody[] rigidbody = hitColTransform.GetComponentsInChildren<Rigidbody>();
                for (int i = 0; i < rigidbody.Length; i++)
                {
                    
                    rigidbody[i].isKinematic = true;
                }
                return;
            }  
        }
        else if(grabe && (Input.GetKeyUp(KeyCode.E) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.C)))
        {
            grabe = false;
            Rigidbody[] rigidbody = hitColTransform.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rigidbody.Length; i++)
            {
                    rigidbody[i].isKinematic = false;
            }
        }
    }   
}
