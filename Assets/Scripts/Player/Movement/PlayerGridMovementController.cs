using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    public class PlayerGridMovementController : MonoBehaviour
    {
        private Grid _grid;
        private Vector3Int _currentCellPosition;
        
        private void Start()
        {
            _grid = FindObjectOfType<Grid>();
            if (_grid == null)
            {
                # if UNITY_EDITOR
                Debug.LogError("Grid not found in the scene");
                # endif
            }
            _currentCellPosition = _grid.WorldToCell(transform.position);
        }
        
        public void SetMovementVectorOnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 inputVector = context.ReadValue<Vector2>();
                Move(inputVector);
            }
        }
        
        private void Move(Vector2 direction)
        {
            _currentCellPosition += new Vector3Int((int)direction.x, (int)direction.y, 0);
            UpdatePositionSnapToGrid();
        }

        private void UpdatePositionSnapToGrid()
        {
            Vector3 worldPosition = _grid.CellToWorld(_currentCellPosition);
            transform.position = worldPosition;
        }
    }
}
