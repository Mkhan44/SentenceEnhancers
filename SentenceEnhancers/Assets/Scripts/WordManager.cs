//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this game developed by BukuGames.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class WordManager
{
    public enum ChainCategory
    {
        None,
        Length,
        LetterPreference,
        WordGroup,
    }
    public enum WordGroup
    {
        Slang,
        InternetAcronym,
    }

    public enum LengthSize
    {
        Short,
        Medium,
        Long
    }

    //Category checkers.

    #region LetterPreference

    /// <summary>
    /// Checks if the word played is comprised mostly of the letterPreference letter.
    /// </summary>
    /// <param name="wordToCheck">The word that the player played.</param>
    /// <param name="letterPreference">The letter that should appear the most in the word for the chain bonus to be given.</param>
    /// <returns></returns>
    public static bool CheckLetterPreference(string wordToCheck, char letterPreference)
    {
        int highestCurrentCharacterOccurrence = 0;
        char currentHighestChar = '-';

        string stringToCheck = wordToCheck.ToLower();
        char letterToCheck = char.ToLower(letterPreference);

        for(int i = 0; i < wordToCheck.Length; i++)
        {
            char charToCompare = stringToCheck[i];
            int numOfOccurrences = stringToCheck.Count(theChar => (theChar == charToCompare));
            Debug.Log($"Num of occurrences of {charToCompare} is: {numOfOccurrences} the current highest occurred char is: {currentHighestChar}");

            if(numOfOccurrences > highestCurrentCharacterOccurrence)
            {
                highestCurrentCharacterOccurrence += 1;
                currentHighestChar = charToCompare;
            }
        }

        if(currentHighestChar == letterPreference)
        {
            Debug.Log($"The LetterPreference is: {letterPreference} and the word {wordToCheck} fulfills the requirement!");
            return true;
        }

        Debug.Log($"The LetterPreference is: {letterPreference} and the word {wordToCheck} does not fulfill the requirement!");
        return false;
    }
    #endregion

    #region Length
    /// <summary>
    /// Check the length of the word based on the determined LengthSize chain bonus.
    /// If short words need to be between 1-3 letters, medium words between 4-7 letters, and long words are 8+ letters.
    /// </summary>
    /// <param name="wordToCheck">The word that was played by the player.</param>
    /// <param name="lengthToMatch">The LengthSize that is needed to get the chain bonus.</param>
    /// <returns></returns>
    public static bool CheckWordLength(string wordToCheck, LengthSize lengthToMatch)
    {
        bool isChainBonusSuccessful = false;
        switch(lengthToMatch)
        {
            case LengthSize.Short:
                {
                    if(wordToCheck.Length > 1 && wordToCheck.Length <= 3)
                    {
                        isChainBonusSuccessful = true;
                    }
                    break;
                }
            case LengthSize.Medium:
                {
                    if (wordToCheck.Length > 3 && wordToCheck.Length <= 7)
                    {
                        isChainBonusSuccessful = true;
                    }
                    break;
                }
            case LengthSize.Long:
                {
                    if (wordToCheck.Length > 7)
                    {
                        isChainBonusSuccessful = true;
                    }
                    break;
                }
        }

        Debug.Log($"The word played was: {wordToCheck} and the length size was: {lengthToMatch} , so chain succession = {isChainBonusSuccessful}");
        return isChainBonusSuccessful;
    }

    #endregion

    #region WordGroup
    public static bool CheckWordGroup(WordData wordPlayed, WordGroup wordGroupToMatch)
    {
        if(wordPlayed.WordGroup == wordGroupToMatch)
        {
            return true;
        }

        return false;
    }

    #endregion
}
