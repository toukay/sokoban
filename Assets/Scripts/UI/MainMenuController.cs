using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuController: MonoBehaviour
    {
        [SerializeField] private int gameSceneIndex = 1;
        
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