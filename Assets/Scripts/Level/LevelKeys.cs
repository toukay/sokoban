using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelKeys", menuName = "Level/LevelKeys")]
    public class LevelKeys : ScriptableObject
    {
        [SerializeField] private char wallKey = '#';
        [SerializeField] private char emptySpaceKey = '.';
        [SerializeField] private char playerKey = '@';
        [SerializeField] private char crateKey = 'C';
        [SerializeField] private char targetKey = 'T';
        
        public char WallKey => wallKey;
        public char EmptySpaceKey => emptySpaceKey;
        public char PlayerKey => playerKey;
        public char CrateKey => crateKey;
        public char TargetKey => targetKey;
    }
}