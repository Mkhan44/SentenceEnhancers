//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SentenceGeneratorList))]
[CanEditMultipleObjects]
public class SentenceDataGenerator : Editor
{
    SentenceGeneratorList sentenceGeneratorList;

    private const string BASEFOLDERPATH = "Assets/SentenceData/";
    private const string TESTPATH = "TEST/";
    public static string filePath;


    private void OnEnable()
    {
        sentenceGeneratorList = (SentenceGeneratorList)FindObjectOfType(typeof(SentenceGeneratorList));
        filePath = "Assets/SentenceData/";
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        SpaceOutGUIVertically(50);

        filePath = FolderField(new GUIContent("Save to", "Where to save..."), filePath);

        //if (GUILayout.Button("Select file path..."))
        //{
        //    filePath = EditorUtility.OpenFolderPanel("TEST", filePath, string.Empty);
        //    filePath = filePath.Substring(filePath.IndexOf("Assets"));
        //    Debug.Log($"File path: {filePath}");
        //}

        SpaceOutGUIVertically(50);

        base.OnInspectorGUI();

        if (GUILayout.Button("GENERATE SENTENCE"))
        {
            if (sentenceGeneratorList == null)
            {
                Debug.LogWarning("Couldn't find SentenceGeneratorList object in the scene! Please make sure there is 1 prefab in the scene.");
                return;
            }

            List<SentenceGeneratorList.SentenceGeneratorListWrapper> sentenceGeneratorListWrapper = sentenceGeneratorList.wordGeneratorListWrappers;
            string basePath = filePath;

            for (int i = 0; i < sentenceGeneratorListWrapper.Count; i++)
            {
                if (sentenceGeneratorListWrapper[i].dataName != string.Empty && sentenceGeneratorListWrapper[i].originalSentence != string.Empty)
                {
                    basePath = filePath + "/";

                    //basePath = basePath + TESTPATH;
                    //switch (sentenceGeneratorListWrapper[i].wordGroup)
                    //{
                    //    case WordManager.WordGroup.Slang:
                    //        {
                    //            basePath = basePath + TESTPATH;
                    //            break;
                    //        }
                    //    default:
                    //        {
                    //            basePath = basePath + TESTPATH;
                    //            break;
                    //        }
                    //}

                    //Make sure only the first letter is capitalized of the word.
                    string wordToUpper = char.ToUpper(sentenceGeneratorListWrapper[i].dataName[0]) + sentenceGeneratorListWrapper[i].dataName.Substring(1);

                    //This will be the actual scriptable name. We'll check if it exists and then create it if it is not a duplicate.
                    string pathToCreateIn = basePath + wordToUpper + ".asset";

                    //Check if the word already exists.
                    string[] guids;
                    bool alreadyExists = false;
                    guids = AssetDatabase.FindAssets("t:SentenceData");
                    foreach (string guid in guids)
                    {
                        if (AssetDatabase.GUIDToAssetPath(guid) == pathToCreateIn)
                        {
                            alreadyExists = true;
                            Debug.LogWarning($"{pathToCreateIn} already exists!");
                        }
                    }

                    if (alreadyExists)
                    {
                        continue;
                    }
                    SentenceData newSentenceData = (SentenceData)CreateInstance(typeof(SentenceData));

                    //Set the data to what the user has input.
                    newSentenceData.OriginalSentence = sentenceGeneratorListWrapper[i].originalSentence;
                    newSentenceData.IsEighteenPlus = sentenceGeneratorListWrapper[i].isEighteenPlus;
                    newSentenceData.CategoryPreference = sentenceGeneratorListWrapper[i].categoryPreference;
                    newSentenceData.OurBlankVariants = new List<SentenceData.WrappedSentenceList>(sentenceGeneratorListWrapper[i].ourBlankVariants.Count);

                    for (int j = 0; j < sentenceGeneratorListWrapper[i].ourBlankVariants.Count; j++)
                    {
                        newSentenceData.OurBlankVariants.Add(sentenceGeneratorListWrapper[i].ourBlankVariants[j]);
                    }

                    AssetDatabase.CreateAsset(newSentenceData, pathToCreateIn);
                    Debug.Log($"Created a new sentence! {newSentenceData.name}");
                    EditorUtility.SetDirty(newSentenceData);
                }
            }
        }

        SpaceOutGUIVertically(50);

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

        EditorGUI.EndChangeCheck();

    }

    string FolderField(GUIContent label, string localPath)
    {
        using (new GUILayout.HorizontalScope())
        {
            string tempPath = EditorGUILayout.TextField(label, localPath);
            if(string.IsNullOrEmpty(tempPath.Trim()))
            {
                return localPath;
            }

            string fullPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), filePath));
            if(!Directory.Exists(fullPath))
            {
                return localPath;
            }

            if(GUILayout.Button("Browse...", EditorStyles.miniButton, GUILayout.Width(100)))
            {
                string newPath = EditorUtility.OpenFolderPanel(label.text, fullPath, "");
                if(!string.IsNullOrEmpty(newPath.Trim()))
                {
                    int index = newPath.IndexOf("Assets/", System.StringComparison.Ordinal);
                    if(index < 0)
                    {
                        Debug.LogError("Invalid path selected.");
                    }
                    else
                    {
                        localPath = newPath.Substring(index);
                    }
                }
            }
        }

        return localPath;
    }

    private static void SpaceOutGUIVertically(int space)
    {
        GUILayout.Space(space);
    }
}


