using System;
using UnityEngine;

[Serializable]
public class Movable : MonoBehaviour, IMovable
{
    [SerializeField] private LayerMask obstacleLayer;
    
    protected const float DefaultDistance = 1f;
    private const float RayCastOffset = DefaultDistance * 0.6f;
    private const float RayCastDistanceMultiplier = 0.8f;
    
    public void Move(Vector3 direction, float distance)
    {
        GameObject obstacle = GetObstacle(transform, direction, distance);
        
        if (obstacle != null)
        {
            if (obstacle.TryGetComponent(out IMovable movable) && !ReferenceEquals(this, movable) && movable.CanMove(direction, distance))
            {
                movable.Move(direction, distance);
            }
            else
            {
                return;
            }
        }
        
        transform.position += direction * distance;
    }

    public bool CanMove(Vector3 direction, float distance)
    {
        GameObject obstacle = GetObstacle(transform, direction, distance);

        return obstacle == null || (
            obstacle.TryGetComponent(out IMovable movable) 
            && !ReferenceEquals(this, movable) 
            && movable.CanMove(direction, distance)
        );
    }
        
    private GameObject GetObstacle(Transform originTransform, Vector3 direction, float distance)
    {
        direction.Normalize();
            
        Vector3 origin = originTransform.position + direction.normalized * RayCastOffset;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance * RayCastDistanceMultiplier, obstacleLayer);

        return hit.collider != null ? hit.collider.gameObject : null;
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
            
        bool canMove = CanMove(direction, distance);
        Gizmos.color = canMove ? Color.green : Color.red;
        Gizmos.DrawRay(origin, direction * distance);
    }
    #endif
    #endregion
}