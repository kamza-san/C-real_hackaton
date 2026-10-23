using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemTable : MonoBehaviour, IInteractable
{
    private bool isgetted = false;
    [SerializeField]
    private bool ifdestroy = false;
    [SerializeField]
    private ItemData itemData;
    public void Interact(GameObject player)
    {
        if (isgetted)
        {
            return;
        }

        if (itemData != null)
        {
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddItem(itemData);
            }

            isgetted = true;

            if (ifdestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}