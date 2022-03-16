using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceManager : MonoBehaviour
{

    public List<SentenceListData> sentenceListDatas;
    // Start is called before the first frame update
    void Start()
    {
        //PopulateSentenceBank();
    }

    void PopulateSentenceBank()
    {
        // for(int i = 0; i < sentenceListDatas.Count; i++)
        //{
        ////    sentenceListDatas[i].sentenceList.Clear();
        //    switch (sentenceListDatas[i].typeOfList)
        //    {
        //        case WordManager.WordType.noun:
        //            {
        //                sentenceListDatas[i].sentenceList.Add("The Fox jumped over a very big ____.");
        //                break;
        //            }
        //        case WordManager.WordType.verb:
        //            {
        //                sentenceListDatas[i].sentenceList.Add("I really just want to ____ you.");
        //                break;
        //            }
        //        case WordManager.WordType.adverb:
        //            {
        //                sentenceListDatas[i].sentenceList.Add("Just give up or you might _____ lose.");
        //                break;
        //            }
        //        default:
        //            {
        //                Debug.Log("FAILED.");
        //                break;
        //            }
                
        //    }
        //}
    }
}
