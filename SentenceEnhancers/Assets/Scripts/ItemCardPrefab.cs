using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCardPrefab : MonoBehaviour
{
    //Based on item data, do something when played.
    public ItemData itemData;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public bool isFrozen;
    public void ResetExternalData()
    {
        isFrozen = false;
    }

    public void SetFrozenState(bool frozenStatus)
    {
        isFrozen = frozenStatus;
    }

    public bool GetFrozenState()
    {
        return isFrozen;
    }
}
