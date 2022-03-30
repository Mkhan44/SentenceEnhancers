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
    public void ItemTypeHandler(ItemData itemData, Player_Gameplay currentPlayer, List<Player_Gameplay> playersList)  
    {
        switch(itemData.itemType)
        {
            case ItemData.ItemType.Freeze:
                {
                    itemData.itemCategory = ItemData.ItemCategory.Attack;
                    OpponentTargetPopup(itemData, currentPlayer, playersList);
                    break;
                }
            case ItemData.ItemType.ShuffleWord:
                {
                    itemData.itemCategory = ItemData.ItemCategory.CardManipulation;
                    OpponentTargetPopup(itemData, currentPlayer, playersList);
                    break;
                }
            default:
                {
                    Debug.Log("Couldn't find a valid itemType for this item!");
                    break;
                }
        }
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
        tempObj.GetComponent<PopupDialouge>().SetupOpponentPopup($"Choose an opponent to target with {itemData.itemType}", PopupDialougeManager.instance.opponentButtonPrefab, playersList.Count, currentPlayer, playersList);
    }

    //Item effects to pass into the button.

    public void FreezeWord(Player_Gameplay playerToFreeze)
    {
        GameObject playerPanel = GameObject.Find(PLAYER_PANEL);
        
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
            Debug.Log($"The: {unfrozenWordCardPrefabs[wordToFreeze].cardWordText.text} was frozen in player: {playerToFreeze.playerID} 's hand.");
        }

        else
        {
            Debug.LogWarning("All cards in this player's hand are frozen.");
            //Need to find some way to gray out that option so the user can't pick it.
        }
       
    }

}
