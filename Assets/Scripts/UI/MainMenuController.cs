using System;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuController: MonoBehaviour
    {
        [SerializeField] private int gameSceneIndex = 1;
        [SerializeField] private CanvasGroup mainMenuCanvasGroup;
        [SerializeField] private Image background;
        [SerializeField] private float maxBackgroundAlphaLevel = 0.7f;
        private bool _fadeIn = false;
        
        public void Start()
        {
            Time.timeScale = 1f;
            
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayMainMenuMusic();
            
            background.color = new Color(0f, 0f, 0f, 0f);
            _fadeIn = true;
            mainMenuCanvasGroup.alpha = 0f;
        }
        
        public void Update()
        {
            if (_fadeIn)
            {
                mainMenuCanvasGroup.alpha += Time.deltaTime;
                background.color = new Color(0f, 0f, 0f, Mathf.Min(maxBackgroundAlphaLevel, background.color.a + Time.deltaTime));
                if (mainMenuCanvasGroup.alpha >= 1f)
                    _fadeIn = false;
            }
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