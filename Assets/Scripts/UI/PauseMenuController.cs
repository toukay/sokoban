using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenuController: MonoBehaviour
    {
        [SerializeField] private GameObject pauseScreen;
        [SerializeField] private GameObject pauseMenuWindow;
        [SerializeField] private GameObject optionsMenuWindow;
        [SerializeField] private int mainMenuSceneIndex = 0;

        public static bool IsPaused = false;
        
        public event Action<bool> OnPause;
        
        private void Start()
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
            IsPaused = false;
        }

        private void Pause()
        {
            pauseScreen.SetActive(true);
            pauseMenuWindow.SetActive(true);
            optionsMenuWindow.SetActive(false);
            Time.timeScale = 0f;
            IsPaused = true;
            OnPause?.Invoke(IsPaused);
        }

        private void Resume()
        {
            pauseScreen.SetActive(false);
            pauseMenuWindow.SetActive(true);
            optionsMenuWindow.SetActive(false);
            Time.timeScale = 1f;
            IsPaused = false;
            OnPause?.Invoke(IsPaused);
        }

        public void OnInputPause(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            if (IsPaused) 
                Resume();
            else 
                Pause();
        }
        
        public void OnButtonResume()
        {
            Resume();
        }
        
        public void OnButtonRestart()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void OnButtonMainMenu()
        {
            SceneManager.LoadSceneAsync(mainMenuSceneIndex);
        }
    }
}