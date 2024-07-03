using System;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuController: MonoBehaviour
    {
        [SerializeField] private int gameSceneIndex = 1;
        
        public void Start()
        {
            Time.timeScale = 1f;
            
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayMainMenuMusic();
        }
        
        public void OnButtonStartGame()
        {
            SceneManager.LoadSceneAsync(gameSceneIndex);
        }
        
        public void OnButtonQuitGame()
        {
            Application.Quit();
        }
    }
}