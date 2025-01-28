using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    
    [Header("Game Over")]
    [SerializeField] private GameObject gameoverScreen;
    [SerializeField] private AudioClip gameoverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;
    private asynchronousLoadingManager asynchronousLoadingManager;

    private void Awake()
    {
        gameoverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void Start()
    {
        asynchronousLoadingManager = FindAnyObjectByType<asynchronousLoadingManager>();
    }

    private void Update()
    {
        

        if (Input.GetAxis("Escape") > 0.1)
        {
            PauseGame(true);
        }

    }

   

    #region Game Over
    public void GameOver()
    {
  
        gameoverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameoverSound);
    }

    public void Restart()
    {
        if (asynchronousLoadingManager == null)
        {
            Debug.LogError("asynchronousLoadingManager is null!");
            return;
        }

        asynchronousLoadingManager.LoadGame(1);
        Bait.score = 0;
    }

    public void MainMenu()
    {
        if (asynchronousLoadingManager == null)
        {
            Debug.LogError("asynchronousLoadingManager is null!");
            return;
        }

        Bait.score = 0;
        asynchronousLoadingManager.LoadGame(0);
    }

    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //executed only in editor
        #endif

    }
    #endregion

    #region Pause

    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);
        
        if (status)
            Time.timeScale = 0.01f;
        else
            Time.timeScale = 1;
     
    }

   

    #endregion

}
