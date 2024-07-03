using System.Linq;
using Commands;
using Level;
using UI;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public bool IsGamePaused { get; private set; }
    
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private string playerTag = "Player";
    
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
        if (_pauseMenuController != null)
        {
            _pauseMenuController.OnPause += SetPause;
        }
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
        if (_pauseMenuController != null)
        {
            _pauseMenuController.OnPause -= SetPause;
        }
    }
    
    private bool AreAllTargetsOccupied()
    {
        return _targets.Length == 0 || _targets.All(target => target.IsOccupied);
    }
    
    private void OnTargetOccupied()
    {
        if (AreAllTargetsOccupied())
        {
            CommandHistoryHandler.Instance.Clear();
            levelLoader.LoadNextLevel();
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
    
    public void SetPause(bool isPaused)
    {
        IsGamePaused = isPaused;
    }
}