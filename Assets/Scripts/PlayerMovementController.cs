using System;
using Commands;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private Movable _player;
        
    private MoveCommand _moveUpCommand;
    private MoveCommand _moveDownCommand;
    private MoveCommand _moveLeftCommand;
    private MoveCommand _moveRightCommand;
    
    private MoveCommand _currentCommand;
    private const float CommandRepeatDelay = 0.4f;
    private const float CommandRepeatRate = 0.2f;

    public void SetPlayer(Movable player)
    {
        _player = player;
        _moveUpCommand = new MoveCommand(_player, Direction.Up, Movable.DefaultDistance);
        _moveDownCommand = new MoveCommand(_player, Direction.Down, Movable.DefaultDistance);
        _moveLeftCommand = new MoveCommand(_player, Direction.Left, Movable.DefaultDistance);
        _moveRightCommand = new MoveCommand(_player, Direction.Right, Movable.DefaultDistance);
    }
    
    private void ExecuteRepeatCommand()
    {
        if (_currentCommand == null || !IsExecutionAllowed()) return;
        
        _currentCommand.Clone().Execute();
    }

    public void OnInputMoveUp(InputAction.CallbackContext context)
    {
        MoveCommand command = _moveUpCommand;
        
        UpdateRepeatingCommand(context, command);
        
        if (!IsExecutionAllowed(context)) return;
            
        command.Clone().Execute();
    }
        
    public void OnInputMoveDown(InputAction.CallbackContext context)
    {
        MoveCommand command = _moveDownCommand;
        
        UpdateRepeatingCommand(context, command);
        
        if (!IsExecutionAllowed(context)) return;
            
        command.Clone().Execute();
    }
        
    public void OnInputMoveLeft(InputAction.CallbackContext context)
    {
        MoveCommand command = _moveLeftCommand;
        
        UpdateRepeatingCommand(context, command);
        
        if (!IsExecutionAllowed(context)) return;
            
        command.Clone().Execute();
    }
        
    public void OnInputMoveRight(InputAction.CallbackContext context)
    {
        MoveCommand command = _moveRightCommand;
        
        UpdateRepeatingCommand(context, command);
        
        if (!IsExecutionAllowed(context)) return;
            
        command.Clone().Execute();
    }

    private bool IsExecutionAllowed(InputAction.CallbackContext context)
    {
        return !GameManager.Instance.IsGamePaused && context.performed;
    }
    
    private bool IsExecutionAllowed()
    {
        return !GameManager.Instance.IsGamePaused;
    }

    private void UpdateRepeatingCommand(InputAction.CallbackContext context, MoveCommand command)
    {
        if (context.started)
        {
            if (_currentCommand == null || _currentCommand == command) return;
            
            _currentCommand = null;
            CancelInvoke(nameof(ExecuteRepeatCommand));
        }
        else if (context.performed)
        {
            if (_currentCommand == command) return;
            
            if (_currentCommand != null)
                CancelInvoke(nameof(ExecuteRepeatCommand));
            
            _currentCommand = command;
            InvokeRepeating(nameof(ExecuteRepeatCommand), CommandRepeatDelay, CommandRepeatRate);
        } 
        else if (context.canceled)
        {
            if (_currentCommand == null || _currentCommand != command) return;
            
            _currentCommand = null;
            CancelInvoke(nameof(ExecuteRepeatCommand));
        }
        else
        {
            if (_currentCommand == null) return;
            
            _currentCommand = null;
            CancelInvoke(nameof(ExecuteRepeatCommand));
        }
    }
}