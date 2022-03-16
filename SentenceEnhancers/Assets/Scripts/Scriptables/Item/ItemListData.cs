using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item List Data", menuName = "Items/ItemListData", order = 1)]
public class ItemListData : ScriptableObject
{
    [System.Serializable]
    public class WrappedItemList
    {
        public ItemData itemData;
        public int copiesInDeck;
    }

    public List<WrappedItemList> wrappedItemLists;
}
