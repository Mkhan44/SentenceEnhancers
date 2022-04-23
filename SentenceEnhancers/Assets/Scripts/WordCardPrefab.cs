//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WordCardPrefab : MonoBehaviour
{
    private GameplayManager gameplayManager;
    private Button thisButton;
    const string GAMEMANAGER = "GameManager";
    public WordData wordData;
    public TextMeshProUGUI cardTypeText;
    public TextMeshProUGUI cardWordText;

    public bool isFrozen;

    private void Start()
    {
        gameplayManager = GameObject.Find(GAMEMANAGER).GetComponent<GameplayManager>();
        thisButton = GetComponent<Button>();
    }

    private void Update()
    {
        if(gameplayManager.GetPhase() != GameplayManager.Phase.placeWord)
        {
            thisButton.interactable = false;
        }
        else
        {
            if(!isFrozen)
            {
                thisButton.interactable = true;
            }
        }
    }
    public WordCardPrefab()
    {
        wordData = null;
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
