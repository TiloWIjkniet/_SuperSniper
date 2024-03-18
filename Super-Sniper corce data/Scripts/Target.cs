using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    public GameObject target;
    public GameObject hitInamge;
    public GameObject setHit;
    public static GameObject setTarget;
    public static float time;

    
    void Start()
    {
        if(GetComponent<HitSettings>() != null)
        {
            GetComponent<HitSettings>().hitEvent.RemoveAllListeners();
            GetComponent<HitSettings>().hitEvent.AddListener(this.gameObject.GetComponent<Target>().Rotate);
        }
    }

    public void Rotate(RaycastHit hit, float damigea)
    {
        Vector3 midde = transform.position  + transform.up * -0.4f;

        float richt =  Vector3.Dot(midde - hit.point, transform.up);
        float douwn =  Vector3.Dot(midde - hit.point, transform.forward);

  
        Debug.DrawLine(midde, hit.point,Color.blue,10);
        //int persenitge = (int)(math.round(100 / 0.6 * (0.6- dis)));

        if(setTarget == null)
        {
            setTarget = Instantiate(target,this.transform);
            setTarget.transform.position = transform.position + new Vector3(3 ,0, 0 );
            StartCoroutine(SiseBige(setTarget));
        }
        setHit = Instantiate(hitInamge, setTarget.transform.GetChild(0));
        setHit.transform .localPosition= new Vector3(135 / 0.6f * richt, 135 / 0.6f * -douwn, 0 );
        StartCoroutine(SiseBigee(setHit));
        if(time >= 1) time = 1;
        
    }

    IEnumerator SiseBigee(GameObject gam)
    {   
       float TimeGam = 0;
        while(TimeGam <= 3)
        {
            TimeGam += Time.deltaTime;
            if(gam == null) break;
            if(TimeGam <=1)
            {
                gam.transform.localScale = Vector3.one * Mathf.Clamp(TimeGam,0,0.5f) * 2;
            }
            else if(TimeGam > 2)
            {   
                gam.transform.localScale = Vector3.one * Mathf.Clamp(3 - TimeGam,0,0.5f) * 2;
            }
            
            yield return new WaitForEndOfFrame();
        }
   
        
    }

    IEnumerator SiseBige(GameObject SetTarget)
    {   
       
        while(time <= 4)
        {
            time += Time.deltaTime;
            if(time <1)
            {
                SetTarget.transform.GetChild(0).localScale = Vector3.one * Mathf.Clamp(time,0,0.5f) * 2;
            }
            else if(time > 3)
            {   
                SetTarget.transform.GetChild(0).localScale = Vector3.one * Mathf.Clamp(4- time,0,0.5f) * 2;
            }
            
            yield return new WaitForEndOfFrame();
        }
        Destroy(SetTarget);
        if(setTarget != null) Destroy(setTarget);
        time = 0;  
    }



}
