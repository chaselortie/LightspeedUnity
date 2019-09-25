using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO revisit these classes and determine scope restrictions (right now they're all public)

// The local player will have these things in memory
[SerializeField]
public class Player {
    public int xp;
    public int level;
    public int walletSC;
    public int walletHC;
    public int numTimesLoggedIn;
    public int[] equippedComponents;
    public int[] unlockedComponents;
    // Component health should be moved down to the component level
    public int health_hull;
    public int health_armor;
    public int health_weapon;
    public int health_thruster;

    public Player() {
        numTimesLoggedIn = 0;
        unlockedComponents = new int[0];
        equippedComponents = new int[0];
        health_hull = 100;
        health_armor = 100;
        health_weapon = 100;
        health_thruster = 100;
    }

    internal void UnlockComponent(int newComponentID) {
        if(unlockedComponents.Contains(newComponentID)) {
            Debug.LogError("Already unlocked!");
        }

        int[] newUnlockedComponents = new int[unlockedComponents.Length + 1];
        for(int i = 0; i < unlockedComponents.Length; i++) {
            newUnlockedComponents[i] = unlockedComponents[i];
        }
        newUnlockedComponents[unlockedComponents.Length] = newComponentID;
        LS_LocalGameController.instance.GetPlayer().unlockedComponents = newUnlockedComponents;
        LS_LocalGameController.instance.SaveData();
    }

    internal bool EquipComponent(int newComponentID) {
        int[] updatedComponents;
        bool shouldAddComponent = !equippedComponents.Contains(newComponentID);
        if(shouldAddComponent) {
            Debug.Log("Adding new component");
            updatedComponents = AddComponent(newComponentID);
        }
        else{
            Debug.Log("Already equipped.  Unequiping...");
            updatedComponents = RemoveComponent(newComponentID);
        }
        LS_LocalGameController.instance.GetPlayer().equippedComponents = updatedComponents;
        LS_LocalGameController.instance.SaveData();
        return shouldAddComponent;
    }

    int[] AddComponent(int newComponentID) {
        int[] newEquippedComponents = new int[equippedComponents.Length + 1];
        for(int i = 0; i < equippedComponents.Length; i++) {
            newEquippedComponents[i] = equippedComponents[i];
        }
        newEquippedComponents[equippedComponents.Length] = newComponentID;
        return newEquippedComponents;
    }

    int[] RemoveComponent(int newComponentID) {
        int[] newEquippedComponents = new int[equippedComponents.Length - 1];
        int componentsAdded = 0;
        for(int i = 0; i < equippedComponents.Length; i++) {
            // Skip the unwanted component
            if(equippedComponents[i] == newComponentID)
                continue;
            newEquippedComponents[componentsAdded] = equippedComponents[i];
            componentsAdded++;
        }
        return newEquippedComponents;
    }


    // Resets the health of all components
    internal void ResetHealth() {
        health_hull = 100;
        health_armor = 100;
        health_weapon = 100;
        health_thruster = 100;
    }

    // For healing: determine how much a component needs to heal
    static public int AmountHealthToAdd(int health, int gainAmount) {
        UnityEngine.Assertions.Assert.IsTrue(gainAmount > 0, "We specify a positive gain amount.");
        int healthNeeded = 100 - health;
        int amountWeCanProvide = Mathf.Min(healthNeeded, gainAmount);
        return amountWeCanProvide;
    }
    // For hurting: determine how much a component can be damaged
    static public int AmountHealthToSubtract(int health, int lossAmount) {
        UnityEngine.Assertions.Assert.IsTrue(lossAmount >= 0, "We specify a positive loss amount.");
        int healthAvailable = health;
        int amountToTake = healthAvailable < lossAmount ? healthAvailable : lossAmount;
        return amountToTake;
    }
}

public abstract class ShipComponent : ScriptableObject {
    public int referenceID;
    public string prefabName;
    public int cost;
    public bool isPremiumItem;
    public int health;
    public int levelRequirement;
    public int weight;
    public ComponentType type = ComponentType.None;
    public enum ComponentType {
        None,
        Armor,
        Gun,
        Hull,
        Thruster
    }
}


/*
//[CreateAssetMenu (fileName = "Ship_", menuName = "Ship")]
//public class Ship : ScriptableObject{
//    public int Health;
//    public float RespawnTime;
//    public int speed;
//    public int levelRequired;
//    public int maxWeight;
//    public int maneuverability;
//    public int cost;
//    public int[] attachedComponentsIDs;
//}

//[CreateAssetMenu (fileName = "Gun_", menuName = "Gun")]
//public class Gun : ShipComponent {
//    public int damage;
//    public float fireRate;
//}

//[CreateAssetMenu (fileName = "Armor_", menuName = "Armor")]
//public class Armor : ShipComponent {
//    public float rechargeRate;
//}

//[CreateAssetMenu (fileName = "Ship_", menuName = "Ship")]
//public class Thruster : ShipComponent {
//    public int speed;

//}
*/
