using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("UI Component")]
    public TMP_Text timerText;
    public GameObject timerUI;

    [Header("Timer Settings")]
    public float currentTime = 5f;
    public bool isCountdown = true; 

    public bool isRestarting = false;

    void Update()
    {
        if (isCountdown)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0; 
            }
        }
        else
        {
            currentTime += Time.deltaTime;
        }

        if(isRestarting)
        {
            DisplaySeconds(currentTime);
            if(currentTime <= 0)
            {   
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            DisplaySeconds(currentTime);
        }
    }

    void DisplaySeconds(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("Restarting in {0}", seconds);
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartCountdown()
    {

        if (!isRestarting)
        {
            isRestarting = true;
            timerUI.gameObject.SetActive(true);
            }
    }
}