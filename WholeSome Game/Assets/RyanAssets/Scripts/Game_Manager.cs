using UnityEngine;
using System;
using System.Collections.Generic;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        SetupScene();
    }

    void SetupScene()
    {
        
    }
    public void Exit()
    {
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("Exiting Game.");
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
