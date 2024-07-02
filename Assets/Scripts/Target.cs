using System;
using UnityEngine;

public class Target: MonoBehaviour
{
    [SerializeField] private string crateTag = "Crate";
    
    public event Action OnOccupied;
    public bool IsOccupied => _isOccupied;
    
    private bool _isOccupied;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(crateTag))
        {
            _isOccupied = true;
            OnOccupied?.Invoke();
        }
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(crateTag))
        {
            _isOccupied = false;
        }
    }
}