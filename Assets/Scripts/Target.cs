using System;
using Audio;
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

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySfx(AudioManager.Instance.crateTargetInSfx);
        }
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(crateTag))
        {
            _isOccupied = false;
            
            if (AudioManager.Instance != null && GameManager.Instance != null && !GameManager.Instance.IsGamePaused)
                AudioManager.Instance.PlaySfx(AudioManager.Instance.crateTargetOutSfx);
        }
    }
}