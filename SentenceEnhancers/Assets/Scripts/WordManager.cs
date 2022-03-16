using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    public enum WordType 
    {
        noun, 
        verb, 
        adverb, 
        adjective 
    };

    //Subcategories for what words can have/be.
    public enum WordGroup
    {
        Funny,
        Cool,
        Serious,
        Action
    }

    public enum LengthSize
    {
        Short,
        Medium,
        Long
    }

}
