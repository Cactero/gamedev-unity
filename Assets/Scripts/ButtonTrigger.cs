using System.IO;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{

    public enum MovementType
    {
        Horizontal_Rotation,
        Vertical_Rotation,
        Horizontal_Translation,
        Vertical_Translation
    }

    [Header("Object Settings")]
    [SerializeField] private GameObject obj;
    [SerializeField] private MovementType movementType;


    [Header("Movement Settings")]
    [SerializeField] private float rotationSpeed = 45f; 

    [Header("Translation Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minBound = -5f;
    [SerializeField] private float maxBound = 5f;    

    private Vector3 startPos;
    private float direction = 1f;

    private void Start()
    {
        if (obj != null)
        {
            startPos = obj.transform.position;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (movementType)
            {
                case MovementType.Horizontal_Rotation:
                    obj.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                    break;
                case MovementType.Vertical_Rotation:
                    obj.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
                    break;
                case MovementType.Horizontal_Translation:
                    float newX = obj.transform.position.x + (moveSpeed * direction * Time.deltaTime);

                    if (newX >= startPos.x + maxBound)
                    {
                        newX = startPos.x + maxBound;
                        direction *= -1f; 
                    }
                    else if (newX <= startPos.x + minBound)
                    {
                        newX = startPos.x + minBound;
                        direction *= -1f; 
                    }
                    

                    obj.transform.position = new Vector3(newX, obj.transform.position.y, obj.transform.position.z);
                    break;                  
                case MovementType.Vertical_Translation:
                    float newY = obj.transform.position.y + (moveSpeed * direction * Time.deltaTime);

                    // Check Top and Bottom bounds relative to where the object started
                    if (newY >= startPos.y + maxBound)
                    {
                        newY = startPos.y + maxBound;
                        direction *= -1f; 
                    }
                    else if (newY <= startPos.y + minBound)
                    {
                        newY = startPos.y + minBound;
                        direction *= -1f; 
                    }

                    obj.transform.position = new Vector3(obj.transform.position.x, newY, obj.transform.position.z);
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }
}