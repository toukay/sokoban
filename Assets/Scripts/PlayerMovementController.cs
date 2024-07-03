using Commands;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        private Movable _player;
        
        private MoveCommand _moveUpCommand;
        private MoveCommand _moveDownCommand;
        private MoveCommand _moveLeftCommand;
        private MoveCommand _moveRightCommand;
        
        public void SetPlayer(Movable player)
        {
            _player = player;
            _moveUpCommand = new MoveCommand(_player, Direction.Up, Movable.DefaultDistance);
            _moveDownCommand = new MoveCommand(_player, Direction.Down, Movable.DefaultDistance);
            _moveLeftCommand = new MoveCommand(_player, Direction.Left, Movable.DefaultDistance);
            _moveRightCommand = new MoveCommand(_player, Direction.Right, Movable.DefaultDistance);
        }

        public void OnInputMoveUp(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            _moveUpCommand.Execute();
        }
        
        public void OnInputMoveDown(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            _moveDownCommand.Execute();
        }
        
        public void OnInputMoveLeft(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            _moveLeftCommand.Execute();
        }
        
        public void OnInputMoveRight(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            _moveRightCommand.Execute();
        }
    }
}