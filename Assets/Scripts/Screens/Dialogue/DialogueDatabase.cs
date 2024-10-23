using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class PlaceDialogues
{
    public DialogueData[] dialogues;
}

public class DialogueData
{
    public int dialogue_index;
    public DialogueEntryData[] entries;
}

public class DialogueEntryData
{
    public int line_index;
    public string character;
    public string[] unlocks;
    public string text;
}

public struct DialogueEntryID
{
    public char location_id;
    public int dialogue_index;
    public int line_index;
}

public class DialogueDatabase : MonoBehaviour
{
    public static DialogueDatabase instance;
    public TableReference table;
    public Dictionary<DialogueEntryID, DialogueEntryData> entries = new Dictionary<DialogueEntryID, DialogueEntryData>();

    public void Awake()
    {
        instance = this;

        List<char> places = new List<char>();

        foreach (KeyValuePair<long, StringTableEntry> entry in LocalizationSettings.StringDatabase.GetTable(table))
        {
            if (!places.Contains(entry.Value.Key[0]))
                places.Add(entry.Value.Key[0]);
        }

        foreach (char place in places)
        {
            foreach (KeyValuePair<long, StringTableEntry> entry in LocalizationSettings.StringDatabase.GetTable(table))
            {
                string key = entry.Value.Key;
                try
                {
                    int line_index = int.Parse(key.Substring(4, 3));
                    DialogueEntryID id = new DialogueEntryID
                    {
                        dialogue_index = int.Parse(key.Substring(1, 2)),
                        location_id = key[0],
                        line_index = line_index,
                    };
                    DialogueEntryData data = new DialogueEntryData();
                    if (entries.ContainsKey(id))
                        data = entries[id];
                    if(key.EndsWith("Unlock"))
                    {
                        data.unlocks = entry.Value.Value.Split(" ");
                        data.line_index = line_index;
                    }
                    else
                    {
                        data.character = key.Substring(8);
                        data.text = entry.Value.Value;
                        data.line_index = line_index;
                    }
                    entries[id] = data;
                }
                catch (Exception e)
                {
                    Debug.Log("Wrong dialogue key : " + key);
                }
            }
        }
    }

    public DialogueEntryData[] GetLines(char location_id, int dialogue_index)
    {
        List<DialogueEntryData> lines = new List<DialogueEntryData>();
        foreach(KeyValuePair<DialogueEntryID, DialogueEntryData> entry in entries)
        {
            if(entry.Key.location_id == location_id && entry.Key.dialogue_index == dialogue_index)
            {
                lines.Add(entry.Value);
            }
        }
        return lines.ToArray();
    }

}
