using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    private void Awake()
    {
        entryContainer = transform.Find("EntryContainer");
        entryTemplate = transform.Find("HisghscoreEntryTemplate");
    }
}
