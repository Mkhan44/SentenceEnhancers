using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles logic for a player during gameplay.
public class Player_Gameplay : MonoBehaviour
{
    //Score related
    public int currentPoints;
    public int currentChain;

    //Items tba
    public bool usedItemThisTurn;

    //Hand related
    public int handMaxSize;
    public int cardsInHand;
   // public List<WordData> wordsInHand;
    //public List<ItemData> itemsInHand;
    public List<WordCardPrefab> wordPrefabsInHand;
    public List<ItemCardPrefab> itemPrefabsInHand;

    //Info related
    public string playerName;
    public int playerID;
    public PlayerInfoUI uiInfoInstance;

    public bool isCurrentlyJudge;
    

    public void UpdatePoints(int points)
    {
        currentPoints = points;
        uiInfoInstance.pointsText.text = ($"Points: {currentPoints}");
    }

    public void UpdateChain(int chainValue)
    {
        currentChain = chainValue;
        uiInfoInstance.chainText.text = ($"Chain: {currentChain}");
    }

    public void UpdateCardsInHand(int cardsTotal)
    {
        cardsInHand = cardsTotal;
        uiInfoInstance.cardsInHandText.text = ($"Cards: {cardsTotal}");
    }

    public void ToggleUsedItem(bool isUsed)
    {
        usedItemThisTurn = isUsed;
    }
}
