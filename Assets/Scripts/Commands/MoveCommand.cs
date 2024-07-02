using UnityEngine;

namespace Commands
{
    public class MoveCommand: Command
    {
        private Movable _movable;
        private Direction _direction;
        private Vector3 _directionVector;
        private float _distance;
        
        public MoveCommand(Movable movable, Direction direction, float distance)
        {
            _movable = movable;
            _direction = direction;
            _directionVector = DirectionToVector(direction);
            _distance = distance;
        }
        
        protected override void ExecuteCommand()
        {
            _movable.Move(_directionVector, _distance);
        }
        
        public override void Undo()
        {
            _movable.Move(-_directionVector, _distance);
        }
        
        public override void Redo()
        {
            ExecuteCommand();
        }
        
        public override Command Clone()
        {
            return new MoveCommand(_movable, _direction, _distance);
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