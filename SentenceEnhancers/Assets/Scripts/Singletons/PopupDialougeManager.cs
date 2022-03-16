using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDialougeManager : MonoBehaviour
{
    public static PopupDialougeManager instance;
    [Header("Regular popup")]
    public GameObject popupPrefab;
    [Header("Item useage")]
    public GameObject opponentItemPopupPrefab;
    public GameObject opponentButtonPrefab;

    public List<GameObject> activePopups;
    private void Awake()
    {
        //Singleton.
        if(instance is null)
        {
            instance = this;
            activePopups = new List<GameObject>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }

    public void AddActivePopupToList(GameObject popup)
    {
        activePopups.Add(popup);
    }

    public void RemoveActivePopupFromList(GameObject popup)
    {
        if(activePopups.Contains(popup))
        {
            activePopups.Remove(popup);
        }
    }
    public void CleanupAllPopups()
    {

        foreach(GameObject popup in activePopups)
        {
            Destroy(popup);
        }

        //Refresh the list.
        activePopups.Clear();
    }

}
