//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this game developed by BukuGames.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
using static ItemListData;

//Sets up and manages the game flow.
public class GameplayManager : MonoBehaviour
{
    public enum Phase 
    {
        playerStart,
        judgeStart,
        item,
        placeWord,
        judging
    }
    
    //TURN THESE ALL INTO PROPERTIES (If they aren't being assigned in inspector)
    public GameObject playerCanvasParent;
    public GameObject wordCardPrefab;
    public GameObject itemCardPrefab;
    public GameObject sentenceCardPrefab;
    public Phase currentPhase;

    //Phases:
    public int submittedWords;

    //Game rules:
    private int pointsToWin;
    private bool specialWinCondition;

    //UI Related
    public GameObject playerInfoInstancePrefab;
    public GameObject playerUIInfoPanel;
    public GameObject playerTableInfoPrefab;
    public GameObject playerTableInfoPanel;
    public TextMeshProUGUI playerUIInfoText;

    //Player related
    public int numPlayers;
    public GameObject playerManagerPrefab;
    public GameObject playersManagerReference;
    public List<Player_Gameplay> playersList;

    //Scriptable References:
    public WordListData nounWordData;
    public WordListData verbWordData;
    public SentenceListData sentencesData;
    public ItemListData itemDataListToUse;

    //Decks
    public List<WordData> wordsDeck;

    public List<WordData> usedNounWordCards;
    public List<WordData> usedVerbWordCards;

    public List<ItemData> itemDeck;


    public int maxWordsInDeck;
    public int maxSentencesInDeck;

    //Sentences
    public List<SentenceData> thisGameSentencesDeck;
    public List<GameObject> sentenceCardJudgeInstances;
    public GameObject sentenceCardSpawnArea;

    public GameObject sentenceDisplayPanel;
    public SentenceData currentSentence;
    public Category.ChainCategory currentBlankCategory;
    public Tuple<Category.ChainCategory,int> currentBlankSubCategory;

    //Items
    public ItemManager itemMangerReference;

    bool firstTurn;

    //Properties
    public int PointsToWin { get => pointsToWin; set => pointsToWin = value; }
    public bool SpecialWinCondition { get => specialWinCondition; set => specialWinCondition = value; }

    private void Start()
    {
        firstTurn = true;
        InitializeGameState();
    }

    private void InitializeGameState()
    {
        if (numPlayers < 3)
        {
            numPlayers = 3;
        }

        CreateDecks();
        SpecialWinCondition = false;
        submittedWords = 0;
        pointsToWin = 10;
        currentPhase = Phase.playerStart;
        GameObject playerTableInfoUI = Instantiate(playerTableInfoPrefab, playerTableInfoPanel.transform);
        playerUIInfoText = playerTableInfoUI.GetComponentInChildren<TextMeshProUGUI>(); 

        SetPhase(Phase.playerStart);
    }

    //Plays the Word card to fill in the sentence blank.
    public void PlayWordCard(Button theButton, WordData wordPlayed, int playerID)
    {
        WordCardPrefab theWordCard = theButton.gameObject.GetComponent<WordCardPrefab>();

        if(theWordCard.GetFrozenState())
        {
            GameObject tempObj = Instantiate(PopupDialougeManager.instance.popupPrefab, playerCanvasParent.GetComponentInParent<Canvas>().transform, false);
            tempObj.GetComponent<PopupDialouge>().SetupPopup("This word is frozen! You can't use it this turn.");
            return;
        }

        TextMeshProUGUI sentenceDisplayText = sentenceDisplayPanel.GetComponentInChildren<SentenceCardPrefab>().sentenceText;
        TextMeshProUGUI sentenceBlankText = sentenceDisplayPanel.GetComponentInChildren<SentenceCardPrefab>().sentenceBlankType;

        string tempString = sentenceDisplayText.text;


        for (int i = 0; i < sentenceCardJudgeInstances.Count; i++)
        {
            sentenceCardJudgeInstances[i].SetActive(true);
            if (sentenceCardJudgeInstances[i].GetComponentInChildren<SentenceCardPrefab>().sentenceText.text == tempString)
            {
               // Debug.Log($"ACTIVATING CARD: {i}");
                sentenceCardJudgeInstances[i].GetComponentInChildren<SentenceCardPrefab>().sentenceText.text = tempString.Replace("____", wordPlayed.Word);
                sentenceCardJudgeInstances[i].GetComponentInChildren<SentenceCardPrefab>().sentenceBlankType.text = sentenceBlankText.text;
                sentenceCardJudgeInstances[i].SetActive(false);
                break;
            }
            sentenceCardJudgeInstances[i].SetActive(false);
        }
       

        bool foundWord = false;
        foreach(Player_Gameplay player in playersList)
        {
            if(foundWord)
            {
                if(!player.isCurrentlyJudge)
                {
                    //If we're the last player, then check if player 1 = judge. If they are not, draw their card. Otherwise draw player 2.
                    if ((player.playerID + 1) == playersList.Count || playersList[playerID+1].playerID == playersList.Count)
                    {
                        RecreateHand(player.playerID);
                        break;
                    }
                    //Recreate the next player's hand.
                    if(!playersList[player.playerID + 1].isCurrentlyJudge)
                    {
                        RecreateHand(player.playerID);
                        break;
                    }
                    //Recreate the first player's hand since the last player is the judge and we are the 2nd to last player.
                    else if (playersList[player.playerID +1].playerID == playersList.Count-1)
                    {
                        RecreateHand(0);
                        break;
                    }
                    continue;
                }
               
            }
            if(player.playerID == playerID && !foundWord)
            {
                foreach(WordCardPrefab wordCardPrefab in player.wordPrefabsInHand)
                {
                    if(wordCardPrefab.wordData == wordPlayed)
                    {
                        //Check based on the Blank type if the submitted word matches. If it does, increase chain bonus. This will need to be refactored for versatility with multiple categories and what not in the future.
                        //This switch is for seeing if player chain can be increased by playing the right type of word.
                        switch(currentBlankCategory)
                        {
                            case Category.ChainCategory.Length:
                                {
                                    //TEST
                                    if (wordCardPrefab.wordData.Word.Length >= 4)
                                    {
                                        CheckChain(player);
                                    }
                                    break;
                                }
                            case Category.ChainCategory.LetterPreference:
                                {
                                   // Debug.Log("LETTER PREFERENCE!");
                                    break;
                                }
                            default:
                                {
                                    Debug.LogError("Couldn't find currentBlankCategory!!!");
                                    break;
                                }
                        }
                      
                        player.wordPrefabsInHand.Remove(wordCardPrefab);
                        Destroy(wordCardPrefab.gameObject);
                        player.cardsInHand -= 1;
                        player.UpdateCardsInHand(player.cardsInHand);
                        SubmitWord();
                        foundWord = true;
                        if ((player.playerID + 1) == playersList.Count || playersList[playerID + 1].playerID == playersList.Count)
                        {
                            //BAD NEED TO CHANGE THIS!!!!
                            if (!playersList[0].isCurrentlyJudge)
                            {
                                RecreateHand(0);
                            }
                            else
                            {
                                RecreateHand(1);
                            }
                            break;
                        }
                        //Recreate the first player's hand since the last player is the judge and we are the 2nd to last player.
                        else if (playersList[player.playerID + 1].playerID == playersList.Count - 1)
                        {
                            RecreateHand(0);
                            break;
                        }

                        break;
                    }

                }
            }

        }

        //For now, we'll have the other players just play their word cards.
        theButton.onClick.RemoveAllListeners();
    }

    public void CheckChain(Player_Gameplay player)
    {
        if (player.currentChain < 5)
        {
            player.currentChain += 1;
            player.UpdateChain(player.currentChain);
        }
    }

    //Call the function from the ItemManager that we have to do a certain card effect.
    public void PlayItemCard(Button theButton, ItemData itemPlayed, Player_Gameplay currentPlayer)
    {
        if (currentPlayer.usedItemThisTurn)
        {
            GameObject tempObj = Instantiate(PopupDialougeManager.instance.popupPrefab, playerCanvasParent.GetComponentInParent<Canvas>().transform, false);
            tempObj.GetComponent<PopupDialouge>().SetupPopup("You already used an item this turn!");
            Debug.LogWarning("You already used an item this turn!");
            return;
        }
        else
        {

            itemMangerReference.ItemTypeHandler(itemPlayed, currentPlayer, playersList);

            //Execute the code below if they used the item via the dialouge box...For now just leave this here.
            foreach (Player_Gameplay player in playersList)
            {
                if (player.playerID == currentPlayer.playerID)
                {

                    List<ItemCardPrefab> cachedListOfItems = new List<ItemCardPrefab>(player.itemPrefabsInHand);
                    //Could have multiple of the same item...The index might need to be used here.
                    foreach (ItemCardPrefab itemPrefab in cachedListOfItems)
                    {
                        if (itemPrefab.itemData == itemPlayed)
                        {
                            Debug.LogWarning($"Used the {itemPlayed.itemType.ToString()} item!");
                            player.itemPrefabsInHand.Remove(itemPrefab);
                            Destroy(itemPrefab.gameObject);
                            player.cardsInHand -= 1;
                            player.UpdateCardsInHand(player.cardsInHand);
                            RecreateHand(currentPlayer.playerID);
                            break;
                        }
                    }
                }
            }

            currentPlayer.ToggleUsedItem(true);
        }

        //For now, we'll have the other players just play their word cards.
        theButton.onClick.RemoveAllListeners();
    }


    public void RecreateHand(int id)
    {
        //foreach(Transform child in playerCanvasParent.transform)
        //{
        //    Destroy(child.gameObject);
        //}

        foreach(Player_Gameplay player in playersList)
        {
            //Activate current player's cards.
            if(player.playerID == id)
            {
                int currentWordCardNum = 0;

                foreach(WordCardPrefab wordCardPrefab in player.wordPrefabsInHand)
                {
                    wordCardPrefab.gameObject.SetActive(true);
                    Button wordCardPrefabButton = wordCardPrefab.GetComponent<Button>();

                    wordCardPrefabButton.onClick.RemoveAllListeners();
                    wordCardPrefabButton.onClick.AddListener(() => PlayWordCard(wordCardPrefabButton, wordCardPrefab.wordData, player.playerID));
                    currentWordCardNum += 1;
                }

                int currentItemCardNum = 0;
                foreach (ItemCardPrefab itemCardPrefab in player.itemPrefabsInHand)
                {
                    itemCardPrefab.gameObject.SetActive(true);
                    Button itemCardPrefabButton = itemCardPrefab.GetComponent<Button>();

                    itemCardPrefabButton.onClick.RemoveAllListeners();
                    itemCardPrefabButton.onClick.AddListener(() => PlayItemCard(itemCardPrefabButton, itemCardPrefab.itemData, player));
                    currentItemCardNum += 1;
                }

              

                if (currentPhase != Phase.judging)
                {
                    //Debug.Log("WE'RE CALLING RECREATEHAND THE ID IS:" + id);
                    playerUIInfoText.text = $"Player {player.playerID}'s turn";
                }
               
            }
            else
            {
                //Deactivate all other player's cards. Might want a way to just hide these instead.
                foreach (WordCardPrefab wordCardPrefab in player.wordPrefabsInHand)
                {
                    wordCardPrefab.gameObject.SetActive(false);
                }

                foreach (ItemCardPrefab itemCardPrefab in player.itemPrefabsInHand)
                {
                    itemCardPrefab.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SubmitWord()
    {
        submittedWords += 1;
       // Debug.Log($"Submitted words is: {submittedWords}");

        //Account for judge being in the mix.
        if(submittedWords >= numPlayers-1)
        {

            //Same loop being used below..let's find a better way to do this.
            foreach (Player_Gameplay player in playersList)
            {
                if (player.isCurrentlyJudge)
                {
                    playerUIInfoText.text = $"Player {player.playerID}'s judging";
                    break;
                }
            }

            SetPhase(Phase.judging);
            submittedWords = 0;
        }
        
    }

    //Only used if we decide to let them use more than 1 item in a turn.
    public void SubmitItem(Player_Gameplay player)
    {
        player.ToggleUsedItem(true);
    }


    private void CreateDecks()
    {
        //This list is for what we've already tried so we don't put the same words into the deck twice.
        int randNum;

        //Nouns deck
        if(nounWordData.wordList.Count >= maxWordsInDeck)
        {
            while (wordsDeck.Count < maxWordsInDeck)
            {
                randNum = Random.Range(0, nounWordData.wordList.Count);
               // Debug.LogWarning(randNum);
                WordData wordToCheck = nounWordData.wordList[randNum];

                if (wordsDeck.Count > 0)
                {
                    if (!wordsDeck.Contains(wordToCheck))
                    {
                        wordsDeck.Add(wordToCheck);
                    }

                }
                //Add first one for sure since we know that there is not a duplicate.
                else
                {
                    wordsDeck.Add(wordToCheck);
                }
            }
        }
        else
        {
            Debug.LogWarning("The word List isn't big enough for the deck we want to create!");
        }


        //Items deck
        int maxNumItemsInDeck = 5;

        //Get a reference for each maxCopies in the deck so we can lower that value by 1 each time an item of that type is put into the deck.
        List<int> maxCopiesOfEachItem = new List<int>();
        foreach(WrappedItemList item in itemDataListToUse.wrappedItemLists)
        {
            maxCopiesOfEachItem.Add(item.copiesInDeck);
        }

        int randomItemToPick;
        while(itemDeck.Count < maxNumItemsInDeck)
        {
            randomItemToPick = Random.Range(0, itemDataListToUse.wrappedItemLists.Count);
            if(maxCopiesOfEachItem[randomItemToPick] >= 1 && itemDataListToUse.wrappedItemLists[randomItemToPick] != null)
            {
                //Debug.Log($"Found item: {itemDataListToUse.wrappedItemLists[randomItemToPick].itemData.name}");
                //Take away 1 from the current copies.
                maxCopiesOfEachItem[randomItemToPick] -= 1;
                itemDeck.Add(itemDataListToUse.wrappedItemLists[randomItemToPick].itemData);
                continue;
            }
        }


        if (sentencesData.sentenceList.Count >= maxSentencesInDeck)
        {
            while (thisGameSentencesDeck.Count < maxSentencesInDeck)
            {
                randNum = Random.Range(0, sentencesData.sentenceList.Count);
               // Debug.LogWarning(randNum);
                SentenceData sentenceToCheck = sentencesData.sentenceList[randNum];

                if (thisGameSentencesDeck.Count > 0)
                {
                    if (!thisGameSentencesDeck.Contains(sentenceToCheck))
                    {
                        thisGameSentencesDeck.Add(sentenceToCheck);
                    }

                }
                //Add first one for sure since we know that there is not a duplicate.
                else
                {
                    thisGameSentencesDeck.Add(sentenceToCheck);
                }
            }
        }
        else
        {
            Debug.LogWarning("The sentence List isn't big enough for the deck we want to create!");
        }


    }

    public void DrawWordCards(Player_Gameplay currentPlayer, int cardsToDraw)
    {
        int randCardToDraw;
        int currentWordCardsInHand = currentPlayer.wordPrefabsInHand.Count;
        WordData wordDataToCheck;
        List<GameObject> listToDestroy = new List<GameObject>();


        while (currentPlayer.wordPrefabsInHand.Count < (currentWordCardsInHand + cardsToDraw))
        {           
            randCardToDraw = Random.Range(0, wordsDeck.Count);
            wordDataToCheck = wordsDeck[randCardToDraw];
            GameObject tempCard = Instantiate(wordCardPrefab, playerCanvasParent.transform);
            WordCardPrefab cardToDraw = tempCard.GetComponent<WordCardPrefab>();
            cardToDraw.wordData = wordDataToCheck;


            cardToDraw.cardWordText.text = wordDataToCheck.Word;
            cardToDraw.cardTypeText.text = wordDataToCheck.WordGroup.ToString();

            if (currentPlayer.wordPrefabsInHand.Count > 0)
            {
                bool foundWord = false;

                foreach(WordCardPrefab wordCardPrefab in currentPlayer.wordPrefabsInHand)
                {
                    //Check if the player already has this word in their hand. If they don't add it.
                    if (wordCardPrefab.wordData != cardToDraw.wordData)
                    {
                        currentPlayer.wordPrefabsInHand.Add(cardToDraw);
                        currentPlayer.cardsInHand += 1;
                        foundWord = true;
                        break;
                    }

                }

                if(!foundWord)
                {
                    listToDestroy.Add(tempCard);
                }
                
            }

            //Add first one for sure since we know that there is not a duplicate.
            else
            {
                currentPlayer.wordPrefabsInHand.Add(cardToDraw);
                currentPlayer.cardsInHand += 1;
            }
        }

        foreach(GameObject obj in listToDestroy)
        {
            Destroy(obj);
        }
    }

    public void DrawItemCards(Player_Gameplay currentPlayer, int cardsToDraw)
    {
        int randCardToDraw;
        int currentItemCardsInHand = currentPlayer.itemPrefabsInHand.Count;
        ItemData itemToDraw;

        while (currentPlayer.itemPrefabsInHand.Count < (currentItemCardsInHand + cardsToDraw))
        {
            randCardToDraw = Random.Range(0, itemDeck.Count);
            itemToDraw = itemDeck[randCardToDraw];
            GameObject tempCard = Instantiate(itemCardPrefab, playerCanvasParent.transform);
            ItemCardPrefab cardToDraw = tempCard.GetComponent<ItemCardPrefab>();
            cardToDraw.itemData = itemToDraw;
            cardToDraw.nameText.text = itemToDraw.itemType.ToString();
            cardToDraw.descriptionText.text = itemToDraw.description;

            currentPlayer.itemPrefabsInHand.Add(cardToDraw);
            currentPlayer.cardsInHand += 1;
        }
    }

    public void SetPhase(Phase phaseToChangeTo)
    {
        currentPhase = phaseToChangeTo;
        PhaseHandler();
    }

    public Phase GetPhase()
    {
        return currentPhase;
    }

    public void PhaseHandler()
    {
        switch (currentPhase)
        {
            case Phase.playerStart:
                {
                    PlayerStartPhase();
                    Debug.Log("Player start phase.");
                    break;
                }
            case Phase.judgeStart:
                {
                    DrawSentenceCard();
                    Debug.Log("Judge start phase.");
                    break;
                }
            case Phase.item:
                {
                    Debug.Log("Item useage phase.");
                    break;
                }
            case Phase.placeWord:
                {
                    Debug.Log("Place word phase.");
                    break;
                }
            case Phase.judging:
                {
                    Debug.Log("Judging phase.");
                    JudgingCommence();
                    break;
                }
            default:
                {
                    Debug.Log("We're not in a phase...wtf?");
                    break;
                }
        }
    }

    //PHASES

    public void PlayerStartPhase()
    {
        if (firstTurn)
        {
            for (int i = 0; i < numPlayers; i++)
            {
                //Make a list of the new player instances that are spawned in so we can keep track of each player.
                GameObject tempObj = Instantiate(playerManagerPrefab, playersManagerReference.transform);
                Player_Gameplay thisPlayer = tempObj.GetComponent<Player_Gameplay>();
                thisPlayer.playerID = i;
                playersList.Add(thisPlayer);
                GameObject tempPlayerUIInstance = Instantiate(playerInfoInstancePrefab, playerUIInfoPanel.transform);
                thisPlayer.uiInfoInstance = tempPlayerUIInstance.GetComponent<PlayerInfoUI>();
                

                //Hardcoded, randomize this in the future.
                if(thisPlayer.playerID == 1)
                {
                    thisPlayer.isCurrentlyJudge = true;
                }
                DrawWordCards(thisPlayer, 3);

                //Spawn in prefabs for player's hand.
                for (int j = 0; j < thisPlayer.wordPrefabsInHand.Count; j++)
                {
                    WordCardPrefab currentWordCard = thisPlayer.wordPrefabsInHand[j];
                    currentWordCard.cardWordText.text = thisPlayer.wordPrefabsInHand[j].wordData.Word;
                    currentWordCard.cardTypeText.text = thisPlayer.wordPrefabsInHand[j].wordData.WordGroup.ToString();

                    int tempNum = j;
                    // Debug.Log(player.wordsInHand[tempNum]);
                    currentWordCard.GetComponent<Button>().onClick.AddListener(() => PlayWordCard(currentWordCard.GetComponent<Button>(), thisPlayer.wordPrefabsInHand[tempNum].wordData, thisPlayer.playerID));
                }

                DrawItemCards(thisPlayer, 2);

                for (int k = 0; k < thisPlayer.itemPrefabsInHand.Count; k++)
                {
                    ItemCardPrefab currentItemCard = thisPlayer.itemPrefabsInHand[k];
                    currentItemCard.GetComponent<ItemCardPrefab>().nameText.text = thisPlayer.itemPrefabsInHand[k].itemData.itemType.ToString();
                    currentItemCard.GetComponent<ItemCardPrefab>().descriptionText.text = thisPlayer.itemPrefabsInHand[k].itemData.description;

                    int tempNum = k;
                    //Change sprite as well.
                    //  tempCard.GetComponent<ItemCardPrefab>().itemData = player.itemsInHand[j];
                    currentItemCard.GetComponent<Button>().onClick.AddListener(() => PlayItemCard(currentItemCard.GetComponent<Button>(), thisPlayer.itemPrefabsInHand[tempNum].itemData, thisPlayer));
                }

                thisPlayer.uiInfoInstance.playerIDText.text = ($"Player: {thisPlayer.playerID}");
                thisPlayer.uiInfoInstance.cardsInHandText.text = ($"Cards: {thisPlayer.cardsInHand}");
                thisPlayer.uiInfoInstance.chainText.text = ($"Chain: {thisPlayer.currentChain}");
                thisPlayer.uiInfoInstance.pointsText.text = ($"Points: {thisPlayer.currentPoints}");

                thisPlayer.ToggleUsedItem(false);
            }

            //TESTING:
            RecreateHand(0);
            //TESTING:

            //

            firstTurn = false;
        }
        else
        {
            int cardsToAdd = 1;

            int playerIDToRecreate = 0;
            for (int i = 0; i < numPlayers; i++)
            {
                if (!playersList[i].isCurrentlyJudge)
                {
                    //Gonna have the player choose which deck in the future prolly so won't call both all the time.
                    DrawWordCards(playersList[i], cardsToAdd);
                }

                //Draw hand of the player that was judge last turn for testing purposes.
                
                //In final game, will have to always draw the hand of the current player unless they are the judge obviously.
                if(playersList[i].isCurrentlyJudge)
                {
                    Debug.Log($"Player: {playersList[i].playerID} 's hand is being recreated.");
                  //  RecreateHand(i);
                    playerIDToRecreate = i;
                }

                playersList[i].UpdateCardsInHand(playersList[i].cardsInHand);
                playersList[i].ToggleUsedItem(false);
            }

            //Switch judge.
            foreach (Player_Gameplay player in playersList)
            {
                foreach(WordCardPrefab wordCardPrefab in player.wordPrefabsInHand)
                {
                    wordCardPrefab.ResetExternalData();
                }

                if (player.isCurrentlyJudge)
                {
                    player.isCurrentlyJudge = false;
                    if (player.playerID + 1 == playersList.Count)
                    {
                        playersList[0].isCurrentlyJudge = true;
                        DrawItemCards(playersList[0], cardsToAdd);
                    }
                    else
                    {
                        int playerToBeJudge = player.playerID + 1;
                        playersList[playerToBeJudge].isCurrentlyJudge = true;
                        DrawItemCards(playersList[playerToBeJudge], cardsToAdd);
                    }
                }
            }

            RecreateHand(playerIDToRecreate);
        }

        SetPhase(Phase.judgeStart);
    }

    private void DrawSentenceCard()
    {
        int randSentence;
        int randBlankType;
        int randBlankCategory;
        int randBlankSubCategory;
        TextMeshProUGUI sentenceDisplayText = sentenceDisplayPanel.GetComponentInChildren<SentenceCardPrefab>().sentenceText;
        TextMeshProUGUI sentenceBlankText = sentenceDisplayPanel.GetComponentInChildren<SentenceCardPrefab>().sentenceBlankType;

        //Since we're doing so much randomizing...maybe make a 'randomize' function that takes in a min/max and spits out a number?
        randSentence = Random.Range(0, thisGameSentencesDeck.Count);
        randBlankType = Random.Range(0, thisGameSentencesDeck[randSentence].ourBlankVariants.Count);
        randBlankCategory = Random.Range(0, typeof(Category.ChainCategory).GetFields().Length);

        //Randomize the base category for this card. Will need to change this based on single player to skew for certain opponents.
        foreach(Category.ChainCategory chainCategory in Enum.GetValues(typeof(Category.ChainCategory)))
        {
            if((int)chainCategory == randBlankCategory)
            {
                currentBlankCategory = chainCategory;
                break;
            }
        }
        
        //Randomize the subcategory based on our main category.
        switch(currentBlankCategory)
        {
            case Category.ChainCategory.Length:
                {
                    //Randomize between options in length...
                    randBlankSubCategory = Random.Range(0, typeof(WordManager.LengthSize).GetFields().Length);
                    currentBlankSubCategory = new Tuple<Category.ChainCategory, int>(currentBlankCategory, randBlankSubCategory);
                    break;
                }
            case Category.ChainCategory.LetterPreference:
                {
                    //Need something to handle this...Basically 1-26 for alphabet english version.
                    //97 - 123 will be 'a' through 'z' lowercase for the Alphabet. Used ASCII table for this conversion in char form.
                    randBlankSubCategory = Random.Range(97, 123);
                    currentBlankSubCategory = new Tuple<Category.ChainCategory, int>(currentBlankCategory, randBlankSubCategory);
                    break;
                }
            default:
                {
                    //EXTRA, not needed for now. Just a placeholder.
                    randBlankSubCategory = Random.Range(0, typeof(WordManager.WordGroup).GetFields().Length);
                    currentBlankSubCategory = new Tuple<Category.ChainCategory, int>(currentBlankCategory, randBlankSubCategory);
                    break;
                }
        }

        currentSentence = thisGameSentencesDeck[randSentence];
        string tempString = currentSentence.ourBlankVariants[randBlankType].theSentence;

        for (int i = 0; i < (numPlayers - 1); i++)
        {
            GameObject tempSentenceCard = Instantiate(sentenceCardPrefab, sentenceCardSpawnArea.transform);
            sentenceCardJudgeInstances.Add(tempSentenceCard);
            tempSentenceCard.GetComponentInChildren<SentenceCardPrefab>().sentenceText.text = tempString;
            tempSentenceCard.SetActive(false);
        }


        //UPDATE THIS TO RANDOMIZE A SENTENCE WITHIN THE DECK + IT'S TYPE/BLANK.
        sentenceDisplayText.text = tempString;
        sentenceBlankText.text = currentBlankCategory.ToString();

    }

    private Enum GetSubCategory()
    {
        if(currentBlankSubCategory == null)
        {
            return default;
        }

        switch(currentBlankCategory)
        {
            case Category.ChainCategory.Length:
                {
                    foreach (WordManager.LengthSize lengthSize in Enum.GetValues(typeof(WordManager.LengthSize)))
                    {
                        if ((int)lengthSize == currentBlankSubCategory.Item2)
                        {
                            return lengthSize;
                        }
                    }
                    break;
                }
            case Category.ChainCategory.LetterPreference:
                {
                    return Category.ChainCategory.LetterPreference;
                }
            default:
                {
                    //Need a better way to test for subcategories that don't have a base category...
                    foreach (WordManager.WordGroup wordGroup in Enum.GetValues(typeof(WordManager.WordGroup)))
                    {
                        if ((int)wordGroup == currentBlankSubCategory.Item2)
                        {
                            return wordGroup;
                        }
                    }
                    Debug.LogWarning("Could not find the enum Type of the subcategory when GetSubCategory was called!");
                    return default;
                }
        }

        return default;

    }


    private void JudgingCommence()
    {
        playerCanvasParent.SetActive(false);
        sentenceDisplayPanel.SetActive(false);

        for (int i = 0; i < sentenceCardJudgeInstances.Count; i++)
        {
            sentenceCardJudgeInstances[i].SetActive(true);
        }

        int currentPlayerID = 0;
        int idToCheckNext = 0;
        foreach (Player_Gameplay player in playersList)
        {
            //If our current player = last in the list, we make currentPlayer that by doing numPlayers-1.
            if(player.isCurrentlyJudge && player.playerID == 0)
            {
                currentPlayerID = numPlayers-1;
                break;
            }
            else if(player.isCurrentlyJudge && (player.playerID == numPlayers-1))
            {
                currentPlayerID = player.playerID - 1;
                break;
            }
            else
            {
                currentPlayerID = 0;
                break;
            }
        }


        foreach(Transform child in sentenceCardSpawnArea.transform)
        {
            Player_Gameplay playerInfo = playersList[currentPlayerID].GetComponent<Player_Gameplay>();
            Button tempButton = child.gameObject.GetComponent<Button>();
            tempButton.onClick.AddListener(() => JudgeCard(playerInfo.playerID));

            if(currentPlayerID == numPlayers-1)
            {
                idToCheckNext = 0;
            }
            else
            {
                idToCheckNext = currentPlayerID + 1;
            }

            if(!playersList[idToCheckNext].isCurrentlyJudge)
            {
                currentPlayerID = idToCheckNext;
            }
        }
    }

    private void JudgeCard(int playerChosenID)
    {
        playerCanvasParent.SetActive(true);
        playersList[playerChosenID].UpdateChain(0);
        //1 point + the chain you have + whatever your current points are = the new total.
        playersList[playerChosenID].UpdatePoints(playersList[playerChosenID].currentPoints + (1 + playersList[playerChosenID].currentChain));
        Debug.Log($"Player: {playerChosenID} now has: {playersList[playerChosenID].currentPoints} points!");

        foreach (Transform child in sentenceCardSpawnArea.transform)
        {
            Destroy(child.gameObject);
        }
        sentenceCardJudgeInstances.Clear();
        sentenceDisplayPanel.SetActive(true);


        SetPhase(Phase.playerStart);
    }


}
