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
        
        public void Move(Vector3 direction, float distance)
        {
            direction.Normalize();
            
            GameObject obstacle = GetObstacle(transform, direction, distance);
            
            if (obstacle != null)
            {
                if (obstacle.CompareTag(crateTag) && CanPushCrate(obstacle, direction, distance))
                {
                    PushCrate(obstacle, direction, distance);
                }
                else
                {
                    return;
                }
            }
            
            transform.position += direction * distance;
        }
        
        private void PushCrate(GameObject crate, Vector3 direction, float distance)
        {
            direction.Normalize();
            
            if (IsBlockedByObstacle(crate.transform, direction, distance)) return;
                
            crate.transform.position += direction * distance;
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

        private bool IsValidMove(Vector3 direction, float distance)
        {
            direction.Normalize();
            
            return !IsBlockedByObstacle(transform, direction, distance);
        }
        
        private bool CanPushCrate(GameObject crate, Vector3 direction, float distance)
        { 
            direction.Normalize();

            if (IsBlockedByObstacle(crate.transform, direction, distance))
                return false;
            
            return true;
        }
        
        private bool IsBlockedByObstacle(Transform originTransform, Vector3 direction, float distance)
        {
            GameObject obstacle = GetObstacle(originTransform, direction, distance);

            if (obstacle == null) return false;

            if (obstacle.CompareTag(crateTag))
                return !CanPushCrate(obstacle, direction, distance);

            return true;
        }
        
        private GameObject GetObstacle(Transform originTransform, Vector3 direction, float distance)
        {
            direction.Normalize();
            
            Vector3 origin = originTransform.position + direction.normalized * RayCastOffset;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance * RayCastDistanceMultiplier, obstacleLayer);

            if (hit.collider == null) return null;
            
            return hit.collider.gameObject;
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
            direction.Normalize();
            
            bool isValid = IsValidMove(direction, distance);
            Gizmos.color = isValid ? Color.green : Color.red;
            Gizmos.DrawRay(origin, direction * distance);
        }
        #endif
        #endregion
    }
}
