using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_ShipManager : MonoBehaviour {
    Transform root;
    Transform hotspot_Armor;
    Transform hotspot_Thruster;
    Transform hotspot_Gun;
    void Start() {
        root = transform;

        // Bogus
        GameObject startingHull = root.GetChild(0).gameObject;
        LS_Helper.KillAllChildren(root);
        GameObject newComponent = Instantiate(startingHull);
        AssignHull(newComponent);
        Debug.Log("--Started!");

        LoadComponentsFromFile();

        Debug.Log("Whats equipped on start:...");
        LS_Helper.PrintIntArray(LS_LocalGameController.instance.GetPlayer().equippedComponents);
    }

    // If you set the hull, it expects child hotspots for other components
    private void AssignHull(GameObject hull) {
        hull.transform.SetParent(root);
        hull.transform.localPosition = Vector3.zero;
        hotspot_Armor = hull.transform.Find("hotspot_Armor");
        hotspot_Thruster = hull.transform.Find("hotspot_Thruster");
        hotspot_Gun = hull.transform.Find("hotspot_Gun");
        if(hotspot_Armor == null || hotspot_Thruster == null || hotspot_Gun == null)
            Debug.LogError("Did not find proper hotspots as children of hull");
    }

    // TODO Too slow
    public void LoadComponentsFromFile() {
        int[] equippedComponentIndicies = LS_LocalGameController.instance.GetPlayer().equippedComponents;

        List<ShipComponent> allComponents = LS_LocalGameController.instance.GetUnlockableComponents();
        List<ShipComponent> equippedComponents = new List<ShipComponent>();
        foreach(var c in allComponents)
            if(equippedComponentIndicies.Contains(c.referenceID))
                equippedComponents.Add(c);


        // Ensure that some hull is equipped
        if(equippedComponentIndicies.Length == 0) {
            Debug.LogWarning("Found no components attached, so just loading default hull.");
            //LS_LocalGameController.instance.GetPlayer().UnlockComponent( LS_LocalGameController.DEFAULT_HULL_COMPONENT_INDEX);

            equippedComponentIndicies = new int[] { LS_LocalGameController.DEFAULT_HULL_COMPONENT_INDEX };

            foreach(var c in allComponents)
                if(c.referenceID == LS_LocalGameController.DEFAULT_HULL_COMPONENT_INDEX)
                    equippedComponents.Add(c);

            LS_LocalGameController.instance.GetPlayer().equippedComponents = equippedComponentIndicies;
            LS_LocalGameController.instance.SaveData();
        }


        // Add hull first
        List<ShipComponent> hulls = LS_Helper.FilterByComponentType(equippedComponents, ShipComponent.ComponentType.Hull);
        if(hulls.Count != 1) {
            Debug.LogError("Too  many hulls! We found:" + hulls.Count);
        }
        ShipComponent hull = hulls[0];
        AssignComponent(hull);
        // Add non-hull components
        equippedComponents.Remove(hull);
        foreach(var c in equippedComponents)
            AssignComponent(c);
    }

    public void RemoveAllComponentsOfACertainType() {

    }


    // If Should add is false then we alreayd have it equipped and must remove it
    public void AssignComponent(ShipComponent comp, bool shouldAdd = true) {

        if(shouldAdd) {
            //RemoveAllComponentsOfACertainType(comp.type);
        }


        if(comp.type != ShipComponent.ComponentType.Hull && (hotspot_Armor == null || hotspot_Thruster == null || hotspot_Gun == null))
            Debug.LogError("Did not find proper hotspots as children of hull");

        string lookupName = "ShipComponents/" + comp.type.ToString() + "/" + comp.prefabName;
        GameObject prefab_component = Resources.Load(lookupName) as GameObject;
        if(prefab_component == null)
            Debug.LogError("Could not Resources.Load component: " + lookupName);
        GameObject newComponent = Instantiate(prefab_component);

        switch(comp.type) {
            case ShipComponent.ComponentType.Hull:
                LS_Helper.KillAllChildren(root);
                AssignHull(newComponent);
                break;
            case ShipComponent.ComponentType.Armor:
                LS_Helper.KillAllChildren(hotspot_Armor);
                newComponent.transform.SetParent(hotspot_Armor);
                break;
            case ShipComponent.ComponentType.Gun:
                LS_Helper.KillAllChildren(hotspot_Gun);
                newComponent.transform.SetParent(hotspot_Gun);
                break;
            case ShipComponent.ComponentType.Thruster:
                LS_Helper.KillAllChildren(hotspot_Thruster);
                newComponent.transform.SetParent(hotspot_Thruster);
                break;
        }

        // "You can't unequip the hull!"
        if(!shouldAdd && comp.type != ShipComponent.ComponentType.Hull) {

            Destroy(newComponent);
            return;
        }

        // TODO i may not want this
        // Really shouldnt need this?
        newComponent.transform.localScale = prefab_component.transform.localScale;
        newComponent.transform.localRotation = prefab_component.transform.localRotation;
        newComponent.transform.localPosition = prefab_component.transform.localPosition;



        // Vector3.one;
        //newComponent.transform.localRotation = Quaternion.Euler(0, 0, 0);
        //newComponent.transform.position = Vector3.zero;
    }



}
