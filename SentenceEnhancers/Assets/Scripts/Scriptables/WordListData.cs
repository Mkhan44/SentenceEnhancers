using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordListData", menuName = "Data/Lists/WordList", order = 1)]
public class WordListData : ScriptableObject
{
    public List<WordData> wordList;
    //
}
