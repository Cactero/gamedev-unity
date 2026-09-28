using UnityEngine;
using TMPro;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] private GameObject textObject; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (textObject != null)
        {
            textObject.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
