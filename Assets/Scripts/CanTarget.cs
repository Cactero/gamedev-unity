using UnityEngine;

public class TargetCan : MonoBehaviour
{
    public float knockDownAngle = 45f;
    private bool _isKnockedDown = false;
    
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        if (_isKnockedDown) return;

        if (Vector3.Angle(transform.up, Vector3.up) > knockDownAngle)
        {
            _isKnockedDown = true;
            
            if (_gameManager != null)
            {
                _gameManager.CanKnockedDown();
            }
        }
    }
}