using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Word Data", menuName = "Data/Words/WordData", order = 1)]
public class WordData : ScriptableObject
{
    [SerializeField] private string word;
    [Tooltip("This tells us what kind of word this is.")]
    [SerializeField] private WordManager.WordGroup wordGroup;
    [SerializeField] private bool isEighteenPlus;

    //Properties.
    public bool IsEighteenPlus { get => isEighteenPlus; set => isEighteenPlus = value; }
    public WordManager.WordGroup WordGroup { get => wordGroup; set => wordGroup = value; }
    public string Word { get => word; set => word = value; }
}
