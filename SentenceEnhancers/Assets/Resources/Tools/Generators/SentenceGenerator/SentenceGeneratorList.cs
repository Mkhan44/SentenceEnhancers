//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentenceGeneratorList : MonoBehaviour
{
    [System.Serializable]
    public class SentenceGeneratorListWrapper
    {
        public string dataName;
        [TextArea(4, 4)]
        public string originalSentence;
        public List<SentenceData.WrappedSentenceList> ourBlankVariants;
        public WordManager.ChainCategory categoryPreference;
        public bool isEighteenPlus = false;
    }

    public List<SentenceGeneratorListWrapper> wordGeneratorListWrappers;
}
