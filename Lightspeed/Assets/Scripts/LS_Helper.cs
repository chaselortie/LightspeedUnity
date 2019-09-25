using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LS_Helper {
    public static List<ShipComponent> FilterByComponentType(List<ShipComponent> unfiltered, ShipComponent.ComponentType filterType) {
        List<ShipComponent> filtered = new List<ShipComponent>();
        foreach(ShipComponent component in unfiltered) {
            if(component.type == filterType) {
                filtered.Add(component);
            }
        }
        return filtered;
    }
    public static Color HexToColor(string hex) {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if(hex.Length == 8) {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

    public static ShipComponent.ComponentType ShipComponentNameFromString(string desiredShipComponentName) {
        foreach(ShipComponent.ComponentType currShipComponentName in System.Enum.GetValues(typeof(ShipComponent.ComponentType))) {
            if(currShipComponentName.ToString().Equals(desiredShipComponentName)) {
                return currShipComponentName;
            }
        }
        Debug.LogError("Could not find ShipComponentName: " + desiredShipComponentName);
        return ShipComponent.ComponentType.None;
    }

    public static void PrintIntArray(int[] x) {
        string outx = "[";
        foreach( var i in x) {
            outx += i +", ";
        }
        outx+= "]";
        Debug.Log(outx);
    }

    //public static List<ShipComponent> ConvertComponentIndiciesToComponents(int[] componentIndicies) {
    //    List<ShipComponent> components = new List<ShipComponent>();
    //    foreach (int componentIndex in componentIndicies)
    //        components.Add(LS_LocalGameController.instance.components)
    //}

    public static void KillAllChildren(UnityEngine.Transform parent) {
        for(int childIndex = parent.childCount - 1; childIndex >= 0; childIndex--) {
            UnityEngine.Object.Destroy(parent.GetChild(childIndex).gameObject);
        }
    }

    public static Sprite Texture2DToSprite(Texture2D t) {
        return Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
    }

    #region 404 Loaders
    public static Texture2D LoadDebugTexture2D() {
        return Resources.Load<Texture2D>("img404");
    }

    public static Sprite LoadDebugSprite() {
        var texture = LoadDebugTexture2D();
        return Texture2DToSprite(LoadDebugTexture2D());
    }

    public static AudioClip LoadDebugSound() {
        return Resources.Load<AudioClip>("audio404");
    }

    // TODO replace above with this one
    public static Object LoadDebugResource<T>() {
        if(typeof(T) == typeof(AudioClip)) {
            return Resources.Load<AudioClip>("audio404");
        }
        else if(typeof(T) == typeof(Sprite)) {
            var texture = Resources.Load<Texture2D>("img404");
            return Sprite.Create(LoadDebugTexture2D(), new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else if(typeof(T) == typeof(Texture2D)) {
            return Resources.Load<Texture2D>("img404");
        }
        else {
            Debug.LogError("Could not provide debug resource for type: " + typeof(T).ToString());
            return null;
        }
    }

    #endregion

    #region Resource & UGC Loaders
    public static Sprite LoadResourceSprite(string path) {
        Sprite resourceFromFile = Resources.Load<Sprite>(path);
        if(null == resourceFromFile) {
            Debug.LogError("Resource not found. Path: '" + path + "'");
            return LoadDebugSprite();
        }
        else {
            return resourceFromFile;
        }
    }

    public static AudioClip LoadResourceSound(string path) {
        AudioClip resourceFromFile = Resources.Load<AudioClip>(path);
        if(null == resourceFromFile) {
            Debug.LogError("Resource not found. Path: '" + path + "'");
            return LoadDebugSound();
        }
        else {
            return resourceFromFile;
        }
    }

    // TODO replace the other resource load methods with this generic one
    public static Object LoadResource<T>(string path) {
        Object resourceFromFile = Resources.Load<Object>(path);
        if(null == resourceFromFile) {
            Debug.LogError("Resource not found. Path: '" + path + "'");
            return LoadDebugResource<T>();
        }
        else {
            return resourceFromFile;
        }
    }
    #endregion
}
