using Player.Commands;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private LayerMask obstacleLayer;
        
        private readonly float _defaultDistance = 1f;
        
        private MoveCommand _moveUpCommand;
        private MoveCommand _moveDownCommand;
        private MoveCommand _moveLeftCommand;
        private MoveCommand _moveRightCommand;
        
        private void Start()
        {
            _moveUpCommand = new MoveCommand(this, Direction.Up, _defaultDistance);
            _moveDownCommand = new MoveCommand(this, Direction.Down, _defaultDistance);
            _moveLeftCommand = new MoveCommand(this, Direction.Left, _defaultDistance);
            _moveRightCommand = new MoveCommand(this, Direction.Right, _defaultDistance);
        }
        
        public void Move(Vector3 displacement)
        {
            if (!IsValidMove(displacement)) return;
            
            transform.position += displacement;
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

        private bool IsValidMove(Vector3 displacement)
        {
            Vector3 origin = transform.position;
            Vector3 direction = displacement.normalized;
            float distance = displacement.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, obstacleLayer);
                
            return hit.collider == null;
        }

        #region Editor Debugging
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 origin = transform.position;

            DrawRay(origin, Vector3.up, _defaultDistance);
            DrawRay(origin, Vector3.down, _defaultDistance);
            DrawRay(origin, Vector3.left, _defaultDistance);
            DrawRay(origin, Vector3.right, _defaultDistance);
        }

        private void DrawRay(Vector3 origin, Vector3 direction, float distance)
        {
            bool isValid = IsValidMove(direction * distance);
            Gizmos.color = isValid ? Color.green : Color.red;
            Gizmos.DrawRay(origin, direction * distance);
        }
        #endif
        #endregion
    }
}
