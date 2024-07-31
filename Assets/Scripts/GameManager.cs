using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Idle,
    Ready,
    Play,
    Pause,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state;

    [Header("UI")] 
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject menuPanel;
    
    private void Awake()
    {
        if (this != null)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            // this = instance;
        }
    }
}
