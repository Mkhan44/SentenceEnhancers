using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Items/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Freeze,
        ShuffleWord,
        Etc,
    }

    public enum ItemCategory
    {
        Attack,
        Points,
        CardManipulation,
        Special
    }

    public ItemType itemType;
    [HideInInspector] public ItemCategory itemCategory;
    public Sprite itemSprite;
    public Color spriteColor;

    [TextArea(3,3)]
    public string description;
}
