using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Linq;

// Writes and reads data from file
public static class LS_DataManager {
    //  Save information describing player progress
    // TODO we would ideally want to move this to be saved in a encrypted XML file or online
    // Note: we save not the components but corresponding refernceIDS
    public static void SavePlayerData(Player player) {
        PlayerPrefs.SetString("player_", Serialize(player));
    }

    // Read in information describing player progress
    // TODO we would ideally want to move this to be saved in a encrypted XML file or online
    public static Player LoadPlayerData() {
        string pastData = PlayerPrefs.GetString("player_", "");
        if(pastData.Length > 0) {
            Debug.Log("Loading previous player data.");
            return Deserialize<Player>(pastData);
        } 
        else {
            Debug.Log("Could not find previous player to load, creating a new player.");
            return new Player();
        }
            
    }
    // Read in information describing all the stuff a player can buy
    public static List<ShipComponent> LoadAllUnlockables() {
        List<ShipComponent> unlockables = new List<ShipComponent>();
        var componentData = Resources.LoadAll("ShipComponents", typeof(ShipComponent));
        foreach(var component in componentData) {
            unlockables.Add(component as ShipComponent);
        }

        // TODO change the ref number to a hash of the name so we dont need this
        Dictionary<int, string> checkNoRepeatRefNumbers = new Dictionary<int, string>();
        foreach(var u in unlockables) {
            if(checkNoRepeatRefNumbers.ContainsKey(u.referenceID)) {
                Debug.Log("Duplicate ref Number between: " + u.name + "  and " + checkNoRepeatRefNumbers[u.referenceID]);
            }
            else {
                checkNoRepeatRefNumbers.Add(u.referenceID, u.name);
            }
        }

        return unlockables;
    }

    public static T Deserialize<T>(this string toDeserialize) {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        StringReader textReader = new StringReader(toDeserialize);
        return (T)xmlSerializer.Deserialize(textReader);
    }

    public static string Serialize<T>(this T toSerialize) {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        StringWriter textWriter = new StringWriter();
        xmlSerializer.Serialize(textWriter, toSerialize);
        return textWriter.ToString();
    }

}
