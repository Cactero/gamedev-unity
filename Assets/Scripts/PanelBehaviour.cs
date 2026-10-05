using UnityEngine;

public class PanelBehaviour : MonoBehaviour
{
    public enum BonusType { Health, SlowMotion }

    [Header("Object Settings")]
    [SerializeField] private BonusType bonusType;
    
    [SerializeField] private GameObject popupTextObject; 
    

    [Header("Bonus Settings")]
    [SerializeField] private float healthBonus = 20f; 
    [SerializeField] private float slowDuration = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                switch (bonusType)
                {
                    case BonusType.Health:
                        player.IncreaseHealth(healthBonus);
                        break;
                    case BonusType.SlowMotion:
                        player.TriggerSlowMotion(slowDuration, 0.5f);
                        break;
                }

                if (popupTextObject != null)
                {
                    popupTextObject.SetActive(true);
                }

                Destroy(gameObject);
            }
        }
    }
}