using System.Linq;
using Level;
using Player;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private PlayerMovementController playerMovementController;
    [SerializeField] private string playerTag = "Player";
    
    private Target[] _targets;
    
    private void Awake()
    {
        EnsureSingleton();
    }

    private void OnEnable()
    {
        levelLoader.OnLevelLoaded += OnLevelLoaded;
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
    
    private bool AreAllTargetsOccupied()
    {
        return _targets.Length == 0 || _targets.All(target => target.IsOccupied);
    }
    
    private void OnTargetOccupied()
    {
        if (AreAllTargetsOccupied())
        {
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
}