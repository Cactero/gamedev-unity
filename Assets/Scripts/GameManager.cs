using UnityEngine;
using System; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int cansRemaining;
    
    public event Action OnGameWon;
    public event Action<int> OnScoreChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void CanKnockedDown()
    {
        cansRemaining--;
        
        OnScoreChanged?.Invoke(cansRemaining);
        
        if (cansRemaining <= 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        OnGameWon?.Invoke();
    }
}