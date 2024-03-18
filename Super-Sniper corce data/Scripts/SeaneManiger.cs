using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeaneManiger : MonoBehaviour
{
    public float timeScale = 1;
    void Update()
    {
        Time.timeScale = timeScale;
        if(Input.GetKeyDown(KeyCode.Keypad1)) { SceneManager.LoadScene(0);Debug.Log("hee");}
        if(Input.GetKeyDown(KeyCode.Keypad2))  SceneManager.LoadScene(1);
        if(Input.GetKeyDown(KeyCode.Keypad3))  SceneManager.LoadScene(2);
        if(Input.GetKeyDown(KeyCode.Keypad4))  SceneManager.LoadScene(3);
    }
}
