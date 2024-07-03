using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class EndMenuController: MonoBehaviour
    {
        [SerializeField] private GameObject endMenuWindow;
        [SerializeField] private int mainMenuSceneIndex = 0;
        
        private void Start()
        {
            endMenuWindow.SetActive(false);
        }
        
        public void DisplayEndMenu()
        {
            GameManager.Instance.PauseGame();
            endMenuWindow.SetActive(true);
        }

        public void OnButtonNextLevel()
        {
            endMenuWindow.SetActive(false);
            if (GameManager.Instance.HasNextLevel)
                GameManager.Instance.LoadNextLevel();
            else
                SceneManager.LoadScene(mainMenuSceneIndex);
            
            GameManager.Instance.ResumeGame();
        }
        
        public void OnButtonRestartLevel()
        {
            endMenuWindow.SetActive(false);
            GameManager.Instance.RestartLevel();
            GameManager.Instance.ResumeGame();
        }
    }
}