using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CanTarget : MonoBehaviour
{
    public float speed = 2f;
    
    public float distance = 3f;

    private Rigidbody rb;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = rb.position;
        
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (Vector3.Angle(Vector3.up, transform.up) > 2f)
        {
            rb.useGravity = true; 
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CanKnockedDown();
            }

            this.enabled = false;
            
            return;
        }

        float offset = Mathf.Sin(Time.time * speed) * distance;
        
        Vector3 newPosition = new Vector3(startPosition.x + offset, rb.position.y, rb.position.z);
        rb.MovePosition(newPosition);
    }
}