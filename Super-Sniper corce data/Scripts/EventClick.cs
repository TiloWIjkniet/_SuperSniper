using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class EventClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    Renderer Renderer;
    AudioSource audioSource;
    public Color selecht;
    public Color klick;
    public Color normal;
    public UnityEvent Event;

    void Start() 
    {   
        audioSource= GetComponent<AudioSource>();
        Renderer = GetComponent<Renderer>();
        Renderer.material.color = normal;
    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Renderer.material.color = selecht;
        Event.Invoke();
        audioSource.Play();
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Renderer.material.color = klick;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Renderer.material.color = selecht;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Renderer.material.color = normal;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

}
