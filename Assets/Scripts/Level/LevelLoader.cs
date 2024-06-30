using System.Collections.Generic;
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
        
        [Header("Layers")]
        [SerializeField] private LayerMask obstacleLayer;

        private Dictionary<char, GameObject> _tileMap;
        private Dictionary<char, GameObject> _objectMap;
        private int _currentLevelIndex = -1;
        private Camera _camera;

        private void Start()
        {
            if (grid == null)
            {
                Debug.LogError("Grid is not assigned.");
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

            LoadNextLevel();
        }

        private void LoadNextLevel()
        {
            if (_tileMap == null || levels == null)
                return;

            if (_currentLevelIndex + 1 >= levels.Length || levels[_currentLevelIndex + 1] == null)
                return;

            _currentLevelIndex++;

            string[] levelLines = levels[_currentLevelIndex].text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
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
                            Instantiate(emptySpacePrefab, worldPosition, Quaternion.identity, transform);
                        }

                        if (objectPrefab != null)
                        {
                            Instantiate(objectPrefab, worldPosition, Quaternion.identity);
                        }
                    }

                    levelBounds.Encapsulate(worldPosition);
                }
            }

            SetCamera(levelBounds);
        }

        private void SetCamera(Bounds bounds)
        {
            bounds.Expand(1);

            float verticalSize = bounds.size.y / 2f;
            float horizontalSize = bounds.size.x * _camera.pixelHeight / _camera.pixelWidth / 2f;

            _camera.transform.position = new Vector3(bounds.center.x, bounds.center.y, -10f);
            _camera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
        }
    }
}
