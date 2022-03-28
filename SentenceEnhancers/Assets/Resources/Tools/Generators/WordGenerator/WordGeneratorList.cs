using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGeneratorList : MonoBehaviour
{
    [System.Serializable]
    public class WordGeneratorListWrapper
    {
        public string theWord;
        public WordManager.WordGroup wordGroup;
        public bool isEighteenPlus;
    }

    public List<WordGeneratorListWrapper> wordGeneratorListWrappers;
    
}
