using Player.Commands;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private string wallTag = "Wall";
        [SerializeField] private string crateTag = "Crate";
        [SerializeField] private string targetTag = "Target";
        
        private const float DefaultDistance = 1f;
        private const float RayCastOffset = DefaultDistance * 0.6f;
        private const float RayCastDistanceMultiplier = 0.8f;
        
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
            Vector3 direction = displacement.normalized;
            float distance = displacement.magnitude;
            
            return !IsBlockedByObstacle(transform, direction, distance);
        }
        
        private bool CanPushCrate(GameObject crate, Vector3 direction, float distance)
        {
           return !IsBlockedByObstacle(crate.transform, direction, distance);
        }
        
        private bool IsBlockedByObstacle(Transform originTransform, Vector3 direction, float distance)
        {
            Vector3 origin = originTransform.position + direction.normalized * RayCastOffset;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance * RayCastDistanceMultiplier, obstacleLayer);

            if (hit.collider == null) return false;
            
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag(crateTag))
                return !CanPushCrate(hitObject, direction, distance);

            return true;
        }


        #region Editor Debugging
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 origin = transform.position;
            float raycastDistance = DefaultDistance * RayCastDistanceMultiplier;

            DrawRay(origin + Vector3.up * RayCastOffset, Vector3.up, raycastDistance);
            DrawRay(origin + Vector3.down * RayCastOffset, Vector3.down, raycastDistance);
            DrawRay(origin + Vector3.left * RayCastOffset, Vector3.left, raycastDistance);
            DrawRay(origin + Vector3.right * RayCastOffset, Vector3.right, raycastDistance);
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
