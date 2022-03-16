using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sentence Data", menuName = "Data/Sentences/SentenceData", order = 1)]
public class SentenceData : ScriptableObject
{
    [System.Serializable]
    public class WrappedSentenceList
    {
        [TextArea(5, 5)]
        public string theSentence;
    }
    [TextArea(4,4)]
    public string originalSentence;
    public List<WrappedSentenceList> ourBlankVariants;
    //public WordManager.WordType typeOfWord;
    //public string baseSentence;
}