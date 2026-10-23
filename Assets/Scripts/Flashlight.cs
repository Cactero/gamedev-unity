using UnityEngine;
using UnityEngine.InputSystem; 

public class Flashlight : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference toggleAction;

    [Header("Flashlight Visuals")]
    public Light spotlight; 
    public bool isFlashlightOn = true;

    [Header("Beam Physics (OverlapCapsule)")]
    public float beamLength = 15f;
    public float beamRadius = 3f; 

    [Header("Layers")]
    public LayerMask hiddenObjectLayer;
    public LayerMask obstacleLayer;

    private void OnEnable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.Enable();
            toggleAction.action.performed += OnToggleFlashlight;
        }
    }

    private void OnDisable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.performed -= OnToggleFlashlight;
            toggleAction.action.Disable();
        }
    }

    private void OnToggleFlashlight(InputAction.CallbackContext context)
    {
        isFlashlightOn = !isFlashlightOn;
        if (spotlight != null) spotlight.enabled = isFlashlightOn;
    }

    private void Start()
    {
        if (spotlight != null) spotlight.enabled = isFlashlightOn;
    }

    private void Update()
    {
        if (isFlashlightOn)
        {
            ScanForHiddenObjects();
        }
    }

    private void ScanForHiddenObjects()
    {
        Vector3 capsuleStart = transform.position;
        Vector3 capsuleEnd = transform.position + (transform.forward * beamLength);

        Collider[] hitObjects = Physics.OverlapCapsule(capsuleStart, capsuleEnd, beamRadius, hiddenObjectLayer);

        foreach (Collider col in hitObjects)
        {
            HiddenObject hiddenObj = col.GetComponent<HiddenObject>();
            if (hiddenObj == null) continue;

            Vector3 dirToObject = (col.transform.position - transform.position).normalized;
            float distanceToObject = Vector3.Distance(transform.position, col.transform.position);
            
            if (!Physics.Raycast(transform.position, dirToObject, distanceToObject, obstacleLayer))
            {
                hiddenObj.Reveal();
            }
            else
            {
                hiddenObj.Hide();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 capsuleStart = transform.position;
        Vector3 capsuleEnd = transform.position + (transform.forward * beamLength);

        Gizmos.DrawWireSphere(capsuleStart, beamRadius);
        Gizmos.DrawWireSphere(capsuleEnd, beamRadius);
        
        Gizmos.DrawLine(capsuleStart + transform.up * beamRadius, capsuleEnd + transform.up * beamRadius);
        Gizmos.DrawLine(capsuleStart - transform.up * beamRadius, capsuleEnd - transform.up * beamRadius);
        Gizmos.DrawLine(capsuleStart + transform.right * beamRadius, capsuleEnd + transform.right * beamRadius);
        Gizmos.DrawLine(capsuleStart - transform.right * beamRadius, capsuleEnd - transform.right * beamRadius);
    }
}