using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Word Data", menuName = "Data/Words/WordData", order = 1)]
public class WordData : ScriptableObject
{
    public string word;
    [Tooltip("This tells us what kind of word this is.")]
    public WordManager.WordGroup wordGroup;
   
}
