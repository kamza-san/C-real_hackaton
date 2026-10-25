using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemTable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool ifdestroy = true;
    [SerializeField] private ItemData giveItemData;
    [SerializeField] private ItemData consumeItemData;
    [SerializeField] private string hintText;

    [SerializeField] private bool skipconfirm;

    public bool isInteracted = false;

    public event Action OnInteracted;

    private bool doneInPreviousLoop;

    private void Awake()
    {
        // 이 루프가 시작될 때의 기록만 읽음 (이번 루프에서 해결한 건 포함되지 않음)
        doneInPreviousLoop = LoopManager.IsDone(gameObject.name);
    }

    public bool CanInteract(PlayerInventory inventory)
    {
        if (isInteracted) return false;
        if (PlayerInventory.Instance == null) return false;

        return true;
    }

    // 소모 아이템을 사용할 준비가 됐는지
    // 일반 아이템: 손에 들고(selectItem) 있어야 함
    // 숨김 아이템: UI에 보이지 않아 선택할 수 없으니 갖고만 있으면 됨
    private bool HasConsumeItemReady(PlayerInventory inventory)
    {
        if (consumeItemData.hidden)
        {
            return inventory.items.Contains(consumeItemData);
        }
        return inventory.selectItem == consumeItemData;
    }

    public void Interact(GameObject player)
    {
        PlayerInventory inventory = PlayerInventory.Instance;
        if (inventory == null || !CanInteract(inventory)) return;

        // 이전 루프에서 해결한 적 있으면 소모 아이템, 힌트, 확인창 없이 바로 실행
        if (doneInPreviousLoop)
        {
            ExecuteInteraction(inventory);
            return;
        }

        // 1. 소모할 아이템이 필요한데, 사용할 준비가 안 된 경우 -> 힌트 팝업
        if (consumeItemData != null && !HasConsumeItemReady(inventory))
        {
            if (ItemPopupManager.Instance != null)
            {
                ItemPopupManager.Instance.CantInterect(hintText);
            }
            return;
        }

        // 2. 소모할 아이템은 없는데(null), hintText가 있는 경우 -> 예/아니오 팝업
        if (consumeItemData == null && !string.IsNullOrEmpty(hintText))
        {
            if (ItemPopupManager.Instance != null)
            {
                ItemPopupManager.Instance.ShowConfirm(
                    hintText,
                    () => {
                        ExecuteInteraction(inventory);
                    }
                );
            }
            return;
        }

        // 3. 소모할 아이템을 사용할 준비가 된 경우
        if (consumeItemData != null)
        {
            if (skipconfirm)
            {
                if (inventory.UseItem(consumeItemData))
                {
                    ExecuteInteraction(inventory);
                }
                return;
            }

            if (ItemPopupManager.Instance != null)
            {
                ItemPopupManager.Instance.ShowConfirm(
                    $"{consumeItemData.itemName}을(를) 사용하시겠습니까?",
                    () => {
                        // UseItem이 인벤토리에서 소모하고 selectItem도 null로 초기화함
                        if (inventory.UseItem(consumeItemData))
                        {
                            ExecuteInteraction(inventory);
                        }
                    }
                );
            }
        }
        else
        {
            // 4. 소모할 아이템도 없고 hintText도 없다면 곧바로 상호작용 실행
            ExecuteInteraction(inventory);
        }
    }

    private void ExecuteInteraction(PlayerInventory inventory)
    {
        if (giveItemData != null)
        {
            inventory.AddItem(giveItemData);
        }

        isInteracted = true;
        LoopManager.MarkDone(gameObject.name);

        OnInteracted?.Invoke();

        if (ifdestroy)
        {
            Destroy(gameObject);
        }
    }
}