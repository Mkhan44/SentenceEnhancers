using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sentence List Data", menuName = "Data/Lists/SentenceList", order = 1)]
public class SentenceListData : ScriptableObject
{
    public List<SentenceData> sentenceList;
    
}
