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
        Length,
        LetterPreference,
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
}
