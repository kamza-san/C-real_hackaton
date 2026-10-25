using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public Sprite image;
    public string itemName; 
    [TextArea]
    public string explain;
    public bool hidden;
}