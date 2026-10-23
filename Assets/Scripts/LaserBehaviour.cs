using UnityEngine;

public class LaserMover : MonoBehaviour
{
    private enum MovementStyle { Straight, Rotating, ReverseRotating, VerticalSweep, HorizontalSweep}
    private MovementStyle currentStyle; 

    [Header("Base Settings")]
    public float forwardSpeed = 7f;
    public float lifetime = 12f;

    [Header("Sweeping Settings")]
    public float sweepSpeed = 3f;      
    public float verticalSweepDistance = 2f;   
    public float horizontalSweepDistance = 4.5f;

    public float heightOffset = 0.4f;

    [Header("Rotating Settings")]
    public float rotationSpeed = 180f; 

    private float timeAlive;
    private Vector3 directionToPlayer;


    void Start()
    {
        Destroy(gameObject, lifetime);

        currentStyle = (MovementStyle)Random.Range(0, 5);

        if (currentStyle == MovementStyle.HorizontalSweep)
        {
            transform.Rotate(0f, 0f, 90f);
            transform.position += new Vector3(0f, heightOffset, 0f);
        }
        else if (currentStyle == MovementStyle.VerticalSweep || currentStyle == MovementStyle.Rotating || currentStyle == MovementStyle.ReverseRotating)
        {
            transform.position += new Vector3(0f, heightOffset, 0f);
        }

        directionToPlayer = Vector3.back;
    }

    void Update()
    {
        timeAlive += Time.deltaTime;

        transform.position += directionToPlayer * forwardSpeed * Time.deltaTime;

        switch (currentStyle)
        {
            case MovementStyle.Straight:
                break;

            case MovementStyle.VerticalSweep:
                float verticalMove = Mathf.Cos(timeAlive * sweepSpeed) * verticalSweepDistance * Time.deltaTime;
                transform.position += Vector3.up * verticalMove;
                break;

            case MovementStyle.HorizontalSweep: 
                float horizontalMove = Mathf.Cos(timeAlive * sweepSpeed) * horizontalSweepDistance * Time.deltaTime;
                transform.position += Vector3.right * horizontalMove;
                break;

            case MovementStyle.Rotating:
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime, Space.Self);
                break;

            case MovementStyle.ReverseRotating:
                transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime, Space.Self);
                break; 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by laser!");  
            other.GetComponent<PlayerController>().GetDamaged();
            if (other.GetComponent<PlayerController>().health <= 0)
            {
                Debug.Log("Player is dead!");
                Timer timerScript = FindAnyObjectByType<Timer>();
        
                if (timerScript != null)
                {
                    timerScript.currentTime = 5f;
                    timerScript.StartCountdown();
                }

                Destroy(other.gameObject);
            }
        }
    }
}