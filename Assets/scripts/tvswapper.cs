using UnityEngine;

[RequireComponent(typeof(ItemTable))]
public class SpriteSwapper : MonoBehaviour
{
    [SerializeField] private SpriteRenderer targetRenderer;   // 비워 두면 이 오브젝트의 SpriteRenderer 사용
    [SerializeField] private Sprite changedSprite;            // 상호작용 후 바뀔 스프라이트 (깨진 TV)

    private ItemTable table;


    private void Start()
    {
        if (LoopManager.IsDone(gameObject.name))
        {
            SwapSprite();
        }
    }
    private void Awake()
    {
        table = GetComponent<ItemTable>();
        if (targetRenderer == null) targetRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        table.OnInteracted += SwapSprite;
    }

    private void OnDisable()
    {
        table.OnInteracted -= SwapSprite;
    }

    private void SwapSprite()
    {
        if (targetRenderer != null && changedSprite != null)
        {
            targetRenderer.sprite = changedSprite;
        }
    }
}