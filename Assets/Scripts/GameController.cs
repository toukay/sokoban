using Commands;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController: MonoBehaviour
{
    public void OnInputUndo(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        CommandHistoryHandler.Instance.Undo();
    }
    
    public void OnInputRedo(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        CommandHistoryHandler.Instance.Redo();
    }
}