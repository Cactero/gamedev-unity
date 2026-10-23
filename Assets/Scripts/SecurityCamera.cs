using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [Header("Camera Panning")]
    public bool enablePanning = true;
    public float sweepAngle = 45f;
    public float sweepSpeed = 2f;
    private Quaternion startRotation;

    [Header("Detection Settings (CheckBox)")]
    public Transform player;
    public Vector3 boxDimensions = new Vector3(5f, 5f, 10f);
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    [Header("Visualization")]
    public Light indicatorLight;
    public Color normalColor = Color.green;
    public Color alertColor = Color.red;

    private void Start()
    {
        startRotation = transform.rotation;
        if (indicatorLight != null) indicatorLight.color = normalColor;
    }

    private void Update()
    {
        if (enablePanning)
        {
            PanCamera();
        }
        
        DetectPlayer();
    }

    private void PanCamera()
    {
        float angle = Mathf.Sin(Time.time * sweepSpeed) * sweepAngle;
        transform.rotation = startRotation * Quaternion.Euler(0, angle, 0);
    }

    private void DetectPlayer()
{
    Vector3 boxCenter = transform.position + (transform.forward * (boxDimensions.z / 2f));
    
    Vector3 halfExtents = boxDimensions / 2f;

    bool playerInBox = Physics.CheckBox(boxCenter, halfExtents, transform.rotation, playerLayer);

    if (playerInBox)
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        int mask = playerLayer | obstacleLayer;

        if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, distanceToPlayer, mask))
        {
            if (hit.collider.gameObject.layer == Mathf.Log(playerLayer.value, 2))
            {
                Debug.Log("Player Detected in Spotlight!");
                if (indicatorLight != null) indicatorLight.color = alertColor;
                return;
            }
        }
    }
    if (indicatorLight != null) indicatorLight.color = normalColor;
}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        
        Vector3 localCenter = new Vector3(0, 0, boxDimensions.z / 2f);
        Gizmos.DrawWireCube(localCenter, boxDimensions);
    }
}