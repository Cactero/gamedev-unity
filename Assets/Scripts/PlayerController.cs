using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float health = 100f;
    [SerializeField] public float damageAmount = 20f;
    
    [SerializeField] private TextMeshProUGUI healthText; 
    

    void Start()
    {
        UpdateHealthUI();
    }

    public void GetDamaged()
    {
        health -= damageAmount;
        UpdateHealthUI();
    }

    public void IncreaseHealth(float amount)
    {
        health += amount;
        UpdateHealthUI();
    }

    public void TriggerSlowMotion(float duration, float timeScaleMultiplier = 0.5f)
    {
        StartCoroutine(SlowMotionRoutine(duration, timeScaleMultiplier));
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = health.ToString("F0");
        }
    }

    private IEnumerator SlowMotionRoutine(float duration, float targetTimeScale)
    {
        Time.timeScale = targetTimeScale;
        
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; // 0.02 is Unity's default fixedDeltaTime
    }
}