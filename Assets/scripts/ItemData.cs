using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public int id;
    public Sprite image;
    public string itemName; 
    [TextArea]
    public string explain;
}