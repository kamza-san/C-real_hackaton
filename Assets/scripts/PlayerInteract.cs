using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask tableLayer;

    private IInteractable nearestInteractable;

    private void Update()
    {
        FindNearestInteractable();
    }

    private void FindNearestInteractable()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange, tableLayer);
        
        float minDistance = float.MaxValue;
        IInteractable closest = null;
        foreach (Collider2D coll in colliders)
        {
            IInteractable interactable = coll.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float distance = Vector2.Distance(transform.position, coll.transform.position);
                
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = interactable;
                }
            }
        }
        nearestInteractable = closest;
    }
    public void OnInteract(InputValue value)
    {
        if (value.isPressed && nearestInteractable != null)
        {
            nearestInteractable.Interact(gameObject);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}