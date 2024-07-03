using System;
using UnityEngine;

public class Player: Movable
{
    [SerializeField] private Animator animator;
    [SerializeField] private string moveXParameter = "MoveX";
    [SerializeField] private string moveYParameter = "MoveY";
    [SerializeField] private string isMovingParameter = "IsMoving";

    private bool _isAnimating = false;

    public override void Move(Vector3 direction, float distance, bool force = false)
    {
        base.Move(direction, distance, force);
        
        animator.SetBool(isMovingParameter, false);
        _isAnimating = false;
        
        animator.SetFloat(moveXParameter, direction.x);
        animator.SetFloat(moveYParameter, direction.y);
        
        animator.SetBool(isMovingParameter, true);
        _isAnimating = true;
    }
    
    public void StopAnimation()
    {
        if (_isAnimating)
        {
            animator.SetBool(isMovingParameter, false);
            _isAnimating = false;
        }
    }
}