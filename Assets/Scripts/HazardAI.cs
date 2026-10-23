using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HazardAI : MonoBehaviour
{
    public enum MovementType { Stationary, Panning, Patrol }
    public enum DetectionShape { Sphere, BoxCone }

    [Header("Behavior Setup")]
    public MovementType movementType = MovementType.Patrol;
    public DetectionShape detectionShape = DetectionShape.BoxCone;

    [Header("Movement Settings")]
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float panAngle = 45f;
    public float panSpeed = 2f;
    private int currentWaypointIndex = 0;
    private Quaternion startRotation;

    [Header("Detection Settings")]
    public Transform player;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private static int enemiesSpottingPlayer = 0;
    private bool amISpottingPlayer = false;
    
    public float detectionRadius = 8f;
    public Vector3 boxDimensions = new Vector3(5f, 5f, 10f);
    

    [Header("Game Over Settings")]
    public float timeToDiscover = 3f; 
    private float detectionTimer = 0f;

    [Header("UI Settings")]
    public Slider stealthSlider;

    [Header("Visuals & State")]
    public Light hazardLight;
    public Color normalColor = Color.white;
    public Color alertColor = Color.red;
    public Color stunnedColor = Color.blue;
    private bool isStunned = false;

    private void Start()
    {
        startRotation = transform.rotation;
        if (hazardLight != null) hazardLight.color = normalColor;
        
        if (stealthSlider != null)
        {
            stealthSlider.maxValue = timeToDiscover;
            stealthSlider.value = 0f;
            stealthSlider.gameObject.SetActive(false); 
        }
    }

    private void Update()
    {
        if (isStunned) return;

        HandleMovement();
        HandleDetection();
    }

    private void HandleMovement()
    {
        switch (movementType)
        {
            case MovementType.Stationary:
                break;

            case MovementType.Panning:
                float angle = Mathf.Sin(Time.time * panSpeed) * panAngle;
                transform.rotation = startRotation * Quaternion.Euler(0, angle, 0);
                break;

            case MovementType.Patrol:
                if (waypoints.Length == 0) return;
                Transform target = waypoints[currentWaypointIndex];
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

                Vector3 dir = (target.position - transform.position).normalized;
                if (dir != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
                }

                if (Vector3.Distance(transform.position, target.position) < 0.2f)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                }
                break;
        }
    }

    private void HandleDetection()
    {
        bool playerInVolume = false;
        bool isDetectedThisFrame = false;

        if (detectionShape == DetectionShape.Sphere)
        {
            playerInVolume = Physics.CheckSphere(transform.position, detectionRadius, playerLayer);
        }
        else if (detectionShape == DetectionShape.BoxCone)
        {
            Vector3 boxCenter = transform.position + (transform.forward * (boxDimensions.z / 2f));
            playerInVolume = Physics.CheckBox(boxCenter, boxDimensions / 2f, transform.rotation, playerLayer);
        }

        if (playerInVolume)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float distToPlayer = Vector3.Distance(transform.position, player.position);
            int mask = playerLayer | obstacleLayer;

            if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, distToPlayer, mask))
            {
                if (hit.collider.gameObject.layer == Mathf.Log(playerLayer.value, 2))
                {
                    isDetectedThisFrame = true; 
                }
            }
        }

        if (isDetectedThisFrame)
        {
            if (!amISpottingPlayer) 
            {
                amISpottingPlayer = true;
                enemiesSpottingPlayer++;
            }

            if (hazardLight != null) hazardLight.color = alertColor;
            
            detectionTimer += Time.deltaTime; 
            
            if (stealthSlider != null)
            {
                stealthSlider.gameObject.SetActive(true);
                if (detectionTimer >= stealthSlider.value || stealthSlider.value == 0)
                {
                    stealthSlider.value = detectionTimer;
                }
            }

            if (detectionTimer >= timeToDiscover)
            {
                RestartScene();
            }
        }
        else
        {
            if (amISpottingPlayer) 
            {
                amISpottingPlayer = false;
                enemiesSpottingPlayer--;
            }

            if (hazardLight != null) hazardLight.color = normalColor;
            detectionTimer = 0f; 

            if (enemiesSpottingPlayer <= 0 && stealthSlider != null)
            {
                stealthSlider.value = 0f;
                stealthSlider.gameObject.SetActive(false);
                enemiesSpottingPlayer = 0;
            }
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void ApplyEMP(float duration)
    {
        StartCoroutine(EMPRoutine(duration));
    }

    private IEnumerator EMPRoutine(float duration)
    {
        isStunned = true;
        if (hazardLight != null) hazardLight.color = stunnedColor;
        
        yield return new WaitForSeconds(duration);
        
        if (hazardLight != null) hazardLight.color = normalColor;
        isStunned = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (detectionShape == DetectionShape.Sphere)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
        else if (detectionShape == DetectionShape.BoxCone)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(new Vector3(0, 0, boxDimensions.z / 2f), boxDimensions);
        }
    }
}