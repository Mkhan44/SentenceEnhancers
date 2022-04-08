//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WordGeneratorList))]
[CanEditMultipleObjects]
public class WordDataGeneratorEditorWindow : Editor
{
    WordGeneratorList wordGeneratorList;

    private const string SLANGFOLDERPATH = "Assets/WordData/WordTypes/Slang/";
    private const string INTERNETACRONYMFOLDERPATH = "Assets/WordData/WordTypes/InternetAcronym/";
 

    private void OnEnable()
    {
        wordGeneratorList = (WordGeneratorList)FindObjectOfType(typeof(WordGeneratorList));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("GENERATE WORD"))
        {
            if(wordGeneratorList == null)
            {
                Debug.LogWarning("Couldn't find WordGeneratorList object in the scene! Please make sure there is 1 prefab in the scene.");
                return;
            }

            List<WordGeneratorList.WordGeneratorListWrapper> wordGeneratorListWrapper = wordGeneratorList.wordGeneratorListWrappers;
            string basePath = string.Empty;

            for (int i = 0; i < wordGeneratorListWrapper.Count; i++)
            {
                if (wordGeneratorListWrapper[i].theWord != string.Empty)
                {
                    basePath = string.Empty;
                    switch (wordGeneratorListWrapper[i].wordGroup)
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

                    //Make sure only the first letter is capitalized of the word.
                    string wordToUpper = char.ToUpper(wordGeneratorListWrapper[i].theWord[0]) + wordGeneratorListWrapper[i].theWord.Substring(1);

                    //This will be based on the WordType. We'll check if it exists and then create it if it is not a duplicate.
                    string pathToCreateIn = basePath + wordToUpper + ".asset";

                    //Check if the word already exists.
                    string[] guids;
                    bool alreadyExists = false;
                    guids = AssetDatabase.FindAssets("t:WordData");
                    foreach (string guid in guids)
                    {
                        if (AssetDatabase.GUIDToAssetPath(guid) == pathToCreateIn)
                        {
                            alreadyExists = true;
                            Debug.LogWarning($"{pathToCreateIn} already exists!");
                        }
                    }

                    if(alreadyExists)
                    {
                        continue;
                    }
                    WordData newWordData = (WordData)CreateInstance(typeof(WordData));

                    //Set the data to what the user has input.
                    newWordData.Word = wordToUpper;
                    newWordData.IsEighteenPlus = wordGeneratorListWrapper[i].isEighteenPlus;
                    newWordData.WordGroup = wordGeneratorListWrapper[i].wordGroup;


                    AssetDatabase.CreateAsset(newWordData, pathToCreateIn);
                    Debug.Log($"Created a new word! {newWordData.name}");
                    EditorUtility.SetDirty(newWordData);
                }
            }
        }

        GUILayout.Space(50);

        //Button to clear the list maybe?
        //if(GUILayout.Button("Clear list"))
        //{
        //    if (wordGeneratorList == null)
        //    {
        //        Debug.LogWarning("Couldn't find WordGeneratorList object in the scene! Please make sure there is 1 prefab in the scene.");
        //        return;
        //    }

        //    wordGeneratorList.wordGeneratorListWrappers.Clear();
        //}

    }

    
}

