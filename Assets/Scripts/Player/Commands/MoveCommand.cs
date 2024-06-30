using UnityEngine;

namespace Player.Commands
{
    public class MoveCommand: IPlayerCommand
    {
        private PlayerMovementController _playerMovementController;
        private Vector3 _displacement;
        
        public MoveCommand(PlayerMovementController playerMovementController, Direction direction, float distance)
        {
            _playerMovementController = playerMovementController;
            _displacement = ComputeDisplacement(direction, distance);
        }
        
        public void Execute()
        {
            _playerMovementController.Move(_displacement);
        }
        
        public void Undo()
        {
            _playerMovementController.Move(-_displacement);
        }
        
        public void Redo()
        {
            Execute();
        }
        
        private static Vector3 ComputeDisplacement(Direction direction, float distance)
        {
            return DirectionToVector(direction) * distance;
        }
        
        private static Vector3 DirectionToVector(Direction direction)
        {
            return direction switch {
                Direction.Up => Vector3.up,
                Direction.Down => Vector3.down,
                Direction.Left => Vector3.left,
                Direction.Right => Vector3.right,
                _ => Vector3.zero
            };
        }
    }
    
    public enum Direction { Up, Down, Left, Right }
}