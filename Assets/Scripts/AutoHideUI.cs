using System.Collections;
using UnityEngine;

public class AutoHideUI : MonoBehaviour
{
    public float displayDuration = 3f;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(displayDuration);
        gameObject.SetActive(false);
    }
}