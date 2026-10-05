using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public GameObject winScreenUI;         

    private bool hasFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasFinished)
        {
            hasFinished = true; 
            Debug.Log("Level Complete! Freezing scene.");

            if (winScreenUI != null)
            {
                winScreenUI.SetActive(true);
            }

            Time.timeScale = 0f;
        }
    }
}