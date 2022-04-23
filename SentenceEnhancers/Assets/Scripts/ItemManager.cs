//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour
{
    private const string PARENT_CANVAS = "Parent_Canvas";
    private const string PLAYER_PANEL = "Player_Panel";
    private const string GAMEMANAGER = "GameManager";
    GameplayManager gameplayManager;

    private void Start()
    {
        gameplayManager = GameObject.Find(GAMEMANAGER).GetComponent<GameplayManager>();
  
    }
    public void ItemTypeHandler(ItemData itemData, Player_Gameplay currentPlayer, List<Player_Gameplay> playersList)  
    {
        switch(itemData.itemType)
        {
            case ItemData.ItemType.Freeze:
                {
                    itemData.itemCategory = ItemData.ItemCategory.Attack;
                    break;
                }
            case ItemData.ItemType.ShuffleWord:
                {
                    itemData.itemCategory = ItemData.ItemCategory.CardManipulation;
                   
                    break;
                }
            default:
                {
                    Debug.LogWarning("Couldn't find a valid itemType for this item!");
                    return;
                    //break;
                }
        }

        OpponentTargetPopup(itemData, currentPlayer, playersList);
    }

    //We'll need another parameter to determine what listener to add to the buttons when they are instantiated!
    public void OpponentTargetPopup(ItemData itemData, Player_Gameplay currentPlayer, List<Player_Gameplay> playersList)
    {
        //NEED A WAY TO CALL THIS.
        GameObject playerPanel = GameObject.Find(PARENT_CANVAS);
        if (playerPanel is null)
        {
            Debug.LogWarning("Can't find Parent_Canvas !!!!");
            return;
        }
        GameObject tempObj = Instantiate(PopupDialougeManager.instance.opponentItemPopupPrefab, playerPanel.transform, false);
        tempObj.GetComponent<PopupDialouge>().SetupOpponentPopup($"Choose an opponent to target with {itemData.itemType}", PopupDialougeManager.instance.opponentButtonPrefab, playersList.Count, currentPlayer, playersList, itemData);
    }

    //Item effects to pass into the button.
    public void FreezeWord(Player_Gameplay playerToFreeze, ItemData itemUsed, Player_Gameplay currentPlayer)
    {
        List<WordCardPrefab> unfrozenWordCardPrefabs = new List<WordCardPrefab>();

        foreach(WordCardPrefab wordCardPrefab in playerToFreeze.wordPrefabsInHand)
        {
            if(!wordCardPrefab.GetFrozenState())
            {
                unfrozenWordCardPrefabs.Add(wordCardPrefab);
            }
        }

        if(unfrozenWordCardPrefabs.Count > 0)
        {
            int wordToFreeze = Random.Range(0, unfrozenWordCardPrefabs.Count);
            unfrozenWordCardPrefabs[wordToFreeze].SetFrozenState(true);
            gameplayManager.UpdatePlayerHandAfterUsingItemCard(itemUsed, currentPlayer);
            Debug.Log($"The: {unfrozenWordCardPrefabs[wordToFreeze].cardWordText.text} was frozen in player: {playerToFreeze.playerID} 's hand.");
        }

        else
        {
            Debug.LogWarning("All cards in this player's hand are frozen.");
            //Need to find some way to gray out that option so the user can't pick it.
        }
       
    }

}
