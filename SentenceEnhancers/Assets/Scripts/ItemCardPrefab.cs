using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemCardPrefab : MonoBehaviour
{
    //Based on item data, do something when played.
    public ItemData itemData;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public bool isFrozen;

    private GameplayManager gameplayManager;
    private Button thisButton;
    const string GAMEMANAGER = "GameManager";

    private void Start()
    {
        gameplayManager = GameObject.Find(GAMEMANAGER).GetComponent<GameplayManager>();
        thisButton = GetComponent<Button>();
    }

    private void Update()
    {
        if (gameplayManager.GetPhase() != GameplayManager.Phase.item)
        {
            thisButton.interactable = false;
        }
        else
        {
            if (!isFrozen)
            {
                thisButton.interactable = true;
            }
        }
    }
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
