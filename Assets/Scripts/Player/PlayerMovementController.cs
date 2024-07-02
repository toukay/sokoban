using System;
using Player.Commands;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : Movable
    {
        private MoveCommand _moveUpCommand;
        private MoveCommand _moveDownCommand;
        private MoveCommand _moveLeftCommand;
        private MoveCommand _moveRightCommand;
        
        private void Start()    
        {
            _moveUpCommand = new MoveCommand(this, Direction.Up, DefaultDistance);
            _moveDownCommand = new MoveCommand(this, Direction.Down, DefaultDistance);
            _moveLeftCommand = new MoveCommand(this, Direction.Left, DefaultDistance);
            _moveRightCommand = new MoveCommand(this, Direction.Right, DefaultDistance);
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
