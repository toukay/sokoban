using System;
using Audio;
using UnityEngine;

[Serializable]
public class Movable : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    
    public const float DefaultDistance = 1f;
    private const float RayCastOffset = DefaultDistance * 0.6f;
    private const float RayCastDistanceMultiplier = 0.8f;
    
    public virtual void Move(Vector3 direction, float distance, bool force = false)
    {
        if (!force && !CanMove(direction, distance)) return;
        
        transform.position += direction * distance;
    }

    public virtual bool CanMove(Vector3 direction, float distance, bool withMovable = false)
    {
        GameObject obstacle = GetObstacle(direction, distance);

        return obstacle == null 
               || (withMovable && obstacle.TryGetComponent(out Movable movable)
                   && !ReferenceEquals(this, movable)
                   && movable.CanMove(direction, distance));
    }
        
    public virtual GameObject GetObstacle(Vector3 direction, float distance)
    {
        direction.Normalize();
            
        Vector3 origin = transform.position + direction.normalized * RayCastOffset;

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
            
        bool canMove = CanMove(direction, distance, withMovable:true);
        Gizmos.color = canMove ? Color.green : Color.red;
        Gizmos.DrawRay(origin, direction * distance);
    }
    #endif
    #endregion
}