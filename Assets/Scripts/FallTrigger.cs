using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    private Vector3 startPos;
    private CharacterController playerController;

    void Awake()
    {
        if (obj != null)
        {
            startPos = obj.transform.position;
            playerController = obj.GetComponent<CharacterController>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && obj != null && playerController != null)
        {
            playerController.enabled = false;
            obj.transform.position = startPos;
            Debug.Log("Player fell out of bounds and respawned!");
            playerController.enabled = true;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
