using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float checkRadius = 2f; 
    [SerializeField] private LayerMask interactableLayer; 

    [SerializeField] private GameObject interactionUI; 
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 1.2f, 0); 

    private PlayerInventory playerInventory;
    private ItemTable currentItemTable;
    private Transform targetTransform;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    private void Update()
    {
        FindClosestInteractable();
        UpdateUIPosition();
    }

    private void FindClosestInteractable()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius, interactableLayer);
        
        float minDistance = float.MaxValue;
        ItemTable closestTable = null;
        Transform closestTransform = null;

        foreach (var col in colliders)
        {
            ItemTable itemTable = col.GetComponent<ItemTable>();
            
            if (itemTable != null && itemTable.CanInteract(playerInventory))
            {
                float distance = Vector2.Distance(transform.position, col.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTable = itemTable;
                    closestTransform = col.transform;
                }
            }
        }

        currentItemTable = closestTable;
        targetTransform = closestTransform;
    }

    private void UpdateUIPosition()
    {
        if (currentItemTable != null && targetTransform != null && interactionUI != null)
        {
            if (!interactionUI.activeSelf)
            {
                interactionUI.SetActive(true);
            }

            Vector3 worldPos = targetTransform.position + worldOffset;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            interactionUI.transform.position = screenPos;
        }
        else
        {
            if (interactionUI != null && interactionUI.activeSelf)
            {
                interactionUI.SetActive(false);
            }
        }
    }

    public void OnInteract(InputValue value)
    {
        if (value.isPressed && currentItemTable != null)
        {
            currentItemTable.Interact(gameObject);
            
            if (currentItemTable.isInteracted)
            {
                currentItemTable = null;
                if (interactionUI != null) interactionUI.SetActive(false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}