using System;
using System.Linq;
using Commands;
using Level;
using UI;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public bool IsGamePaused => PauseMenuController.IsPaused;
    public bool HasNextLevel => levelLoader.HasNextLevel;
    
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private EndMenuController endMenuController;
    
    private Target[] _targets;
    
    private PauseMenuController _pauseMenuController;
    
    private void Awake()
    {
        EnsureSingleton();
    }

    private void OnEnable()
    {
        levelLoader.OnLevelLoaded += OnLevelLoaded;
        _pauseMenuController = FindObjectOfType<PauseMenuController>();
    }
    
    private void OnDisable()
    {
        levelLoader.OnLevelLoaded -= OnLevelLoaded;
        if (_targets != null)
        {
            foreach (var target in _targets)
            {
                target.OnOccupied -= OnTargetOccupied;
            }
        }
    }

    private void Start()
    {
        _pauseMenuController = FindObjectOfType<PauseMenuController>();
    }

    private bool AreAllTargetsOccupied()
    {
        return _targets.Length == 0 || _targets.All(target => target.IsOccupied);
    }
    
    private void OnTargetOccupied()
    {
        if (AreAllTargetsOccupied())
        {
            if (endMenuController != null && _pauseMenuController)
            {
                _pauseMenuController.Pause(false);
                endMenuController.DisplayEndMenu();
            }
            else LoadNextLevel();
        }
    }
    
    private void OnLevelLoaded()
    {
        _targets = levelLoader.GetObjectsOfType<Target>();
        foreach (var target in _targets)
        {
            target.OnOccupied += OnTargetOccupied;
        }
        Movable player = levelLoader.GetObjectOfTypeWithTag<Movable>(playerTag);
        playerMovementController.SetPlayer(player);
    }

    private void EnsureSingleton(bool destroyOnLoad = true)
    {
        if (Instance == null)
        {
            Instance = this;
            if(!destroyOnLoad) DontDestroyOnLoad(gameObject);
        } 
        else Destroy(gameObject);
    }
    
    public void RestartLevel()
    {
        CommandHistoryHandler.Instance.Clear();
        levelLoader.RestartLevel();
    }
    
    public void LoadNextLevel()
    {
        CommandHistoryHandler.Instance.Clear();
        levelLoader.LoadNextLevel();
    }

    public void PauseGame()
    {
        if (_pauseMenuController == null || IsGamePaused) return;
        
        _pauseMenuController.Pause(false);
    }
    
    public void ResumeGame()
    {
        if (_pauseMenuController == null || !IsGamePaused) return;
        
        _pauseMenuController.Resume();
    }
}