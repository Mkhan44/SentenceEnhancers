//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupDialouge : MonoBehaviour
{
    public TextMeshProUGUI dialougeText;
    public TextMeshProUGUI noticeText;
    public TextMeshProUGUI buttonText;
    public ItemManager itemManagerRef;
    public Button closeButton;

    private void Awake()
    {
        itemManagerRef = PopupDialougeManager.instance.itemManagerRef;
        PopupDialougeManager.instance.AddActivePopupToList(gameObject);
    }
    public void SetupPopup(string dialougeMessage, string noticeMessage = "Notice!")
    {
        dialougeText.text = dialougeMessage;
        noticeText.text = noticeMessage;
    }

    public void SetupOpponentPopup(string dialougeMessage, GameObject opponentButtonPrefab, int numPlayers, Player_Gameplay currentPlayer, List<Player_Gameplay> playersList, ItemData itemData, string noticeMessage = "Choose opponent", string parentToFind = "OpponentsButtonGroup")
    {
        RectTransform buttonPanel = GameObject.Find(parentToFind).GetComponent<RectTransform>();
        dialougeText.text = dialougeMessage;
        noticeText.text = noticeMessage;

        foreach(Player_Gameplay player_Gameplay in playersList)
        {
            if (currentPlayer.playerID != player_Gameplay.playerID && !player_Gameplay.isCurrentlyJudge)
            {
                GameObject tempObj = Instantiate(opponentButtonPrefab, buttonPanel, false);
                tempObj.GetComponentInChildren<TextMeshProUGUI>().text = "Player: " + player_Gameplay.playerID;
                Button itemButton = tempObj.GetComponent<Button>();

                //NEED TO CHANGE THIS LINE SO THAT IT'S MORE DYNAMIC.
                switch(itemData.itemType)
                {
                    case ItemData.ItemType.Freeze:
                        {
                            itemButton.onClick.AddListener(() => itemManagerRef.FreezeWord(player_Gameplay, itemData, currentPlayer));
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                
                itemButton.onClick.AddListener(() => PopupDialougeManager.instance.CleanupAllPopups());
            }
        }
    }

    public void CleanupPopup()
    {
        PopupDialougeManager.instance.RemoveActivePopupFromList(gameObject);
        Destroy(gameObject);
    }
}
