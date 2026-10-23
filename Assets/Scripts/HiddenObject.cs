using UnityEngine;
using System.Collections; 

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class HiddenObject : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Collider objCollider;
    private bool isBeingLookedAt = false;

    [Header("Stun Settings")]
    public float stunDuration = 3f;
    private bool isPlayerNear = false;
    private bool isActivated = false; 

    [Header("UI Settings")]
    public GameObject stunTextObject; 

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        objCollider = GetComponent<Collider>();
        meshRenderer.enabled = false; 

        if (stunTextObject != null) stunTextObject.SetActive(false);
    }

    public void Reveal()
    {
        if (isActivated) return; 
        
        isBeingLookedAt = true;
        meshRenderer.enabled = true;
    }

    public void Hide()
    {
        isBeingLookedAt = false;
        meshRenderer.enabled = false;
    }

    private void Update()
    {
        if (isPlayerNear && meshRenderer.enabled && !isActivated)
        {
            StunEnemies();
        }
    }

    private void LateUpdate()
    {
        if (!isBeingLookedAt && !isActivated)
        {
            meshRenderer.enabled = false;
        }
        
        isBeingLookedAt = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    private void StunEnemies()
    {
        isActivated = true;
        Debug.Log("Hidden switch pressed! Stunning all AI.");

        HazardAI[] allHazards = FindObjectsByType<HazardAI>();
        foreach (HazardAI hazard in allHazards)
        {
            hazard.ApplyEMP(stunDuration);
        }

        StartCoroutine(ShowTextAndDestroy());
    }

    private IEnumerator ShowTextAndDestroy()
    {
        meshRenderer.enabled = false;
        objCollider.enabled = false;

        if (stunTextObject != null) stunTextObject.SetActive(true);

        yield return new WaitForSeconds(stunDuration);

        if (stunTextObject != null) stunTextObject.SetActive(false);
        Destroy(gameObject);
    }
}