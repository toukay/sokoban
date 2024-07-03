using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using UnityEngine;

namespace Level
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Level layout Data")]
        [SerializeField] private TextAsset[] levels;
        [SerializeField] private LevelKeys levelKeys;

        [Header("Game objects Prefabs")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject cratePrefab;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject targetPrefab;
        [SerializeField] private GameObject emptySpacePrefab;

        [Header("Grid Settings")]
        [SerializeField] private Grid grid;
        
        public event Action OnLevelLoaded;
        public bool HasNextLevel => _currentLevelIndex + 1 < levels.Length;

        private Dictionary<char, GameObject> _tileMap;
        private Dictionary<char, GameObject> _objectMap;
        private int _currentLevelIndex = -1;
        private Camera _camera;
        private List<GameObject> _currentLevelObjects;

        private void Start()
        {
            if (grid == null)
            {
                # if UNITY_EDITOR
                Debug.LogError("Grid is not assigned.");
                # endif
                return;
            }

            _camera = Camera.main;

            _tileMap = new Dictionary<char, GameObject>
            {
                { levelKeys.WallKey, wallPrefab },
                { levelKeys.EmptySpaceKey, emptySpacePrefab },
            };

            _objectMap = new Dictionary<char, GameObject>
            {
                { levelKeys.PlayerKey, playerPrefab },
                { levelKeys.CrateKey, cratePrefab },
                { levelKeys.TargetKey, targetPrefab }
            };

            _currentLevelObjects = new List<GameObject>();
            LoadNextLevel();
        }

        public void LoadNextLevel()
        {
            if (_tileMap == null || levels == null)
                return;

            if (_currentLevelIndex + 1 >= levels.Length || levels[_currentLevelIndex + 1] == null)
            {
                # if UNITY_EDITOR
                Debug.Log("No more levels to load.");
                # endif
                return;
            }
            
            _currentLevelIndex++;

            ClearCurrentLevelObjects();
            LoadLevel(levels[_currentLevelIndex].text);

            OnLevelLoaded?.Invoke();
        }

        private void LoadLevel(string levelText)
        {
            string[] levelLines = levelText.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            Bounds levelBounds = new Bounds();

            for (int y = 0; y < levelLines.Length; y++)
            {
                for (int x = 0; x < levelLines[y].Length; x++)
                {
                    char key = levelLines[y][x];
                    Vector3Int cellPosition = new Vector3Int(x, -y, 0);
                    Vector3 worldPosition = grid.CellToWorld(cellPosition);

                    if (_tileMap.TryGetValue(key, out GameObject tilePrefab))
                    {
                        if (tilePrefab != null)
                        {
                            GameObject instance = Instantiate(tilePrefab, worldPosition, Quaternion.identity, transform);
                            _currentLevelObjects.Add(instance);

                            if (tilePrefab == wallPrefab)
                            {
                                instance.layer = LayerMask.NameToLayer("Obstacle");
                            }
                        }
                    }

                    if (_objectMap.TryGetValue(key, out GameObject objectPrefab))
                    {
                        if (emptySpacePrefab != null)
                        {
                            GameObject emptySpaceInstance = Instantiate(emptySpacePrefab, worldPosition, Quaternion.identity, transform);
                            _currentLevelObjects.Add(emptySpaceInstance);
                        }

                        if (objectPrefab != null)
                        {
                            GameObject objectInstance = Instantiate(objectPrefab, worldPosition, Quaternion.identity);
                            _currentLevelObjects.Add(objectInstance);
                        }
                    }

                    levelBounds.Encapsulate(worldPosition);
                }
            }

            SetCamera(levelBounds);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayLevelMusic(_currentLevelIndex+1);
        }

        public void RestartLevel()
        {
            ClearCurrentLevelObjects();
            LoadLevel(levels[_currentLevelIndex].text);
            
            OnLevelLoaded?.Invoke();
        }

        private void ClearCurrentLevelObjects()
        {
            foreach (GameObject obj in _currentLevelObjects) 
                Destroy(obj);
            
            _currentLevelObjects.Clear();
        }

        private void SetCamera(Bounds bounds)
        {
            bounds.Expand(1);

            float verticalSize = bounds.size.y / 2f;
            float horizontalSize = bounds.size.x * _camera.pixelHeight / _camera.pixelWidth / 2f;

            _camera.transform.position = new Vector3(bounds.center.x, bounds.center.y, -10f);
            _camera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
        }
        
        public T[] GetObjectsOfType<T>() where T : MonoBehaviour
        {
            return _currentLevelObjects
                .Select(obj => obj.GetComponent<T>())
                .Where(component => component != null)
                .ToArray();
        }
        
        public GameObject GetObjectOfTag(string goTag)
        {
            return _currentLevelObjects
                .FirstOrDefault(obj => obj.CompareTag(goTag));
        }
        
        public T GetObjectOfTypeWithTag<T>(string goTag) where T : MonoBehaviour
        {
            return _currentLevelObjects
                .Select(obj => obj.GetComponent<T>())
                .FirstOrDefault(component => component != null && component.CompareTag(goTag));
        }
    }
}
