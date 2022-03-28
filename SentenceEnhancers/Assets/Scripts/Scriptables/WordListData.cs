//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordListData", menuName = "Data/Lists/WordList", order = 1)]
public class WordListData : ScriptableObject
{
    public List<WordData> wordList;
}
