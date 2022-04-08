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
        [SerializeField] private string blankVariant;

        public string BlankVariant { get => blankVariant; set => blankVariant = value; }
    }
    [TextArea(4, 4)]
    [SerializeField] private string originalSentence;
    [SerializeField] private List<WrappedSentenceList> ourBlankVariants;
    [SerializeField] private WordManager.ChainCategory categoryPreference;
    [SerializeField] private bool isEighteenPlus = false;

    public string OriginalSentence { get => originalSentence; set => originalSentence = value; }
    public List<WrappedSentenceList> OurBlankVariants { get => ourBlankVariants; set => ourBlankVariants = value; }
    public WordManager.ChainCategory CategoryPreference { get => categoryPreference; set => categoryPreference = value; }
    public bool IsEighteenPlus { get => isEighteenPlus; set => isEighteenPlus = value; }
    //public WordManager.WordType typeOfWord;
    //public string baseSentence;
}