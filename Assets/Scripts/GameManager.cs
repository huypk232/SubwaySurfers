using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum GameState
{
    Idle,
    Ready,
    Play,
    Pause,
}

public enum CameraAngle
{
    Idle = 0,
    Play = 1,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state;

    [Header("Camera")] 
    [SerializeField] private Camera[] cameras;
    
    [Header("UI")] 
    [SerializeField] private GameObject idlePanel;
    [SerializeField] private GameObject inGamePanel;
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

    private void Start()
    {
        idlePanel.SetActive(true);
    }

    public void TestButton()
    {
        Debug.Log("Press");
    }
    
    private void ChangeCamera(CameraAngle index)
    {
        foreach (var angle in cameras)
        {
            angle.gameObject.SetActive(false);
        }
        cameras[(int)index].gameObject.SetActive(true);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        inGamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void StartGame()
    {
        idlePanel.SetActive(false);
        inGamePanel.SetActive(true);
        state = GameState.Play;
        ChangeCamera(CameraAngle.Play);
    }

    public void PauseGame()
    {
        
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }
    
    // Debug only
    public void Resume()
    {
        Time.timeScale = 1;
        inGamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }
}
