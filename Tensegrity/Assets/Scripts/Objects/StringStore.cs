using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StringStore
{
    public List<GameObject> SavedStrings;

    public StringStore()
    {
        SavedStrings = new List<GameObject>();
    }


}