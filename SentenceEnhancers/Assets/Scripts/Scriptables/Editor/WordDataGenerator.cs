using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WordDataGenerator : EditorWindow
{
    private string theWord;
    private WordManager.WordGroup wordGroup;
    private bool isEighteenPlus;

    private const string SLANGFOLDERPATH = "Assets/WordData/WordTypes/Slang/";
    private const string INTERNETACRONYMFOLDERPATH = "Assets/WordData/WordTypes/InternetAcronym/";
    [MenuItem("SE Tools/Generators/Word Data Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(WordDataGenerator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate a WordData scriptable", EditorStyles.boldLabel);
        theWord = EditorGUILayout.TextField("The word", theWord);

        wordGroup = (WordManager.WordGroup)EditorGUILayout.EnumPopup("Word type", wordGroup);

        isEighteenPlus = EditorGUILayout.Toggle("Is 18+", isEighteenPlus);

        string basePath = string.Empty;
        switch(wordGroup)
        {
            case WordManager.WordGroup.Slang:
                {
                    basePath = SLANGFOLDERPATH;
                    break;
                }
            case WordManager.WordGroup.InternetAcronym:
                {
                    basePath = INTERNETACRONYMFOLDERPATH;
                    break;
                }
        }

        if (GUILayout.Button("Generate word!") && theWord != null)
        {
            //This will be based on the WordType. We'll check if it exists and then create it if it is not a duplicate.
            string pathToCreateIn = basePath + theWord + ".asset";
            WordData newWordData = (WordData)CreateInstance(typeof(WordData));
            //Set the data to what the user has input.
            newWordData.Word = theWord;
            newWordData.IsEighteenPlus = isEighteenPlus;
            newWordData.WordGroup = wordGroup;

            AssetDatabase.CreateAsset(newWordData, pathToCreateIn);
            Debug.Log($"Created a new word! {newWordData.name}");
            EditorUtility.SetDirty(newWordData);
        }
    }
}
