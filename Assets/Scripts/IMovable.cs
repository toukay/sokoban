using UnityEngine;

public interface IMovable
{
    void Move(Vector3 direction, float distance);
    bool CanMove(Vector3 direction, float distance);
}