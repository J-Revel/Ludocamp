using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class PlaceDialogues
{
    public DialogueData[] dialogues;
}

[System.Serializable]
public class DialogueData
{
    public string uid;
    public char location_id;
    public int dialogue_index;
    public string topic;
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
    public string uid;
    public char location_id;
    public int dialogue_index;
    public int line_index;
}

public class DialogueDatabase : MonoBehaviour
{
    public static DialogueDatabase instance;
    public TableReference table;
    public Dictionary<DialogueEntryID, DialogueEntryData> entries = new Dictionary<DialogueEntryID, DialogueEntryData>();
    public Dictionary<char, List<DialogueData>> dialogue_per_location = new Dictionary<char, List<DialogueData>>();

    public void Awake()
    {
        instance = this;
    }
    
    public IEnumerator Start()
    {
        var table_request = LocalizationSettings.StringDatabase.GetTableAsync(table);
        while(!table_request.IsDone)
            yield return null;
        List<char> places = new List<char>();

        foreach (KeyValuePair<long, StringTableEntry> entry in table_request.Result)
        {
            if (!places.Contains(entry.Value.Key[3]))
                places.Add(entry.Value.Key[3]);
        }

        foreach (KeyValuePair<long, StringTableEntry> entry in table_request.Result)
        {
            string key = entry.Value.Key;
            char location_id = key[3];

            int chapter_index = (int)key[0] - (int)'A';
            int dialogue_index = chapter_index * 100 + int.Parse(key.Substring(1, 2));
            string uid = key.Substring(0, 4);
            if(key.EndsWith("Topic"))
            {
                if(!dialogue_per_location.ContainsKey(location_id))
                    dialogue_per_location[location_id] = new List<DialogueData>();
                dialogue_per_location[location_id].Add(new DialogueData {
                    uid = uid,
                    location_id = location_id,
                    dialogue_index = dialogue_index,
                    topic = entry.Value.Value,
                });
            }
            else
            {
                try
                {
                    int line_index = int.Parse(key.Substring(5, 3));
                    DialogueEntryID id = new DialogueEntryID
                    {
                        uid = uid,
                        dialogue_index = dialogue_index,
                        location_id = location_id,
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
                        data.character = key.Substring(9);
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
        lines.Sort((A, B) => { return A.line_index.CompareTo(B.line_index); });
        return lines.ToArray();
    }

    public DialogueData[] ListAvailableDialogues(char location_id)
    {
        if (!dialogue_per_location.ContainsKey(location_id))
            return new DialogueData[0];
        List<DialogueData> result = new List<DialogueData>();
        foreach(var dialogue in dialogue_per_location[location_id])
        {
            if(EvaluationReport.Instance.IsDialogueUnlocked(dialogue.uid))
            {
                result.Add(dialogue);
            }
        }
        return result.ToArray();
    }
}
