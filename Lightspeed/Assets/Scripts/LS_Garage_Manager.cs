using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LS_Garage_Manager : MonoBehaviour {
    //[SerializeField] UnityEngine.UI.Button btnBack;
    [SerializeField] UnityEngine.UI.Text txtUserWallet;
    [SerializeField] UnityEngine.UI.Button btnThruster;
    [SerializeField] UnityEngine.UI.Button btnArmor;
    [SerializeField] UnityEngine.UI.Button btnGun;
    [SerializeField] UnityEngine.UI.Button btnHull;
    [SerializeField] UnityEngine.UI.Button btnBottomBar_EarnCredits;
    [SerializeField] UnityEngine.UI.Button btnBottomBar_Customize;

    [SerializeField] AudioSource sound;
    [SerializeField] AudioClip purchase;
    [SerializeField] AudioClip error;
    [SerializeField] AudioClip nuetral;

    [SerializeField] Transform parentOfComponentTypes;
    [SerializeField] LS_IronSourceAds ads;
    [SerializeField] LS_Garage_ComponentOptions ops;
    [SerializeField] LS_ShipManager ship;

    public Text gyroText;

    // This is redundant storage TODO
    List<ShipComponent> unlockables;
    List<ShipComponent> currentComponentsBeingDisplayed;
    ShipComponent.ComponentType currType = ShipComponent.ComponentType.None;

    void Start() {
        
        PlayerPrefs.SetInt("Gyro", 0); //set gryo to off at start
        //sound = GetComponent<AudioSource>();

        //UnityEngine.Assertions.Assert.IsNotNull(btnBack);
        UnityEngine.Assertions.Assert.IsNotNull(txtUserWallet);
        UnityEngine.Assertions.Assert.IsNotNull(btnThruster);
        UnityEngine.Assertions.Assert.IsNotNull(btnArmor);
        UnityEngine.Assertions.Assert.IsNotNull(btnGun);
        UnityEngine.Assertions.Assert.IsNotNull(btnHull);
        UnityEngine.Assertions.Assert.IsNotNull(btnBottomBar_EarnCredits);
        UnityEngine.Assertions.Assert.IsNotNull(btnBottomBar_Customize);

        UnityEngine.Assertions.Assert.IsNotNull(sound);
        UnityEngine.Assertions.Assert.IsNotNull(purchase);
        UnityEngine.Assertions.Assert.IsNotNull(error);
        UnityEngine.Assertions.Assert.IsNotNull(nuetral);


        UnityEngine.Assertions.Assert.IsNotNull(parentOfComponentTypes);
        UnityEngine.Assertions.Assert.IsNotNull(ads);
        UnityEngine.Assertions.Assert.IsNotNull(ops);
        UnityEngine.Assertions.Assert.IsNotNull(ship);

        //btnBack.onClick.AddListener(delegate{BackButton();});
        btnThruster.onClick.AddListener(delegate { OnClick_ComponentType(ShipComponent.ComponentType.Thruster); });
        btnArmor.onClick.AddListener(delegate { OnClick_ComponentType(ShipComponent.ComponentType.Armor); }); // TODO Also call paint panel button on MenuManager to get that nice tween in/out
        btnGun.onClick.AddListener(delegate { OnClick_ComponentType(ShipComponent.ComponentType.Gun); });
        btnHull.onClick.AddListener(delegate { OnClick_ComponentType(ShipComponent.ComponentType.Hull); });

        btnBottomBar_EarnCredits.onClick.AddListener(delegate { PlayAd(); });
        btnBottomBar_Customize.onClick.AddListener(delegate { OnClick_Customize(); });

        ops.OptionSelected += OnComponentOptionSelected;
        unlockables = LS_LocalGameController.instance.GetUnlockableComponents();
        parentOfComponentTypes.gameObject.SetActive(false);
        UpdateWalletDisplay();
    }


    public void OnClickStartRace() {
        LS_LocalGameController.instance.StartRace();
    }

    public void setGryo() {
        if (PlayerPrefs.GetInt("Gyro", 0) == 1) {
            PlayerPrefs.SetInt("Gyro", 0);
            Debug.Log(PlayerPrefs.GetInt("Gyro", 0));
            gyroText.text = "GYRO OFF";
        }
        else {
            PlayerPrefs.SetInt("Gyro", 1);
            Debug.Log(PlayerPrefs.GetInt("Gyro", 0));
            gyroText.text = "GYRO ON";
        }

    }

    // TODO the control flow here is wrong, need to confirm ad watch to earn the money
    private void PlayAd() {
        bool adStarted = ads.RequestPlayRewardedVideo();
        bool adCompleted = true; // TODO
        if(adCompleted) {
            LS_LocalGameController.instance.GetPlayer().walletSC += 1000;
            UpdateWalletDisplay();
        }
    }

    private void UpdateWalletDisplay() {
        txtUserWallet.text = "" + LS_LocalGameController.instance.GetPlayer().walletSC;
        LS_LocalGameController.instance.SaveData();
    }

    private void OnDestroy() {
        ops.OptionSelected -= OnComponentOptionSelected;
    }

    private void OnComponentOptionSelected(int index) {
        Debug.Log("index: " + index + " has been chosen!");
        ShipComponent item = currentComponentsBeingDisplayed[index];
        // Handle monetary aspect of purchase
        bool owned = LS_LocalGameController.instance.GetPlayer().unlockedComponents.Contains(item.referenceID); // TODO the whole reference id system needs a replacement, maybe just hash the string name?
        if(!owned) {
            // Can't afford...
            if(item.cost > LS_LocalGameController.instance.GetPlayer().walletSC) {
                Debug.Log("Insufficient Funds");
                // Flash player wallet display red or something TODO
                sound.clip = error;
                sound.Play();
                return;
            }
            // Purchase...
            else {
                LS_LocalGameController.instance.GetPlayer().walletSC -= item.cost;
                LS_LocalGameController.instance.GetPlayer().UnlockComponent(item.referenceID);
                UpdateWalletDisplay();
                currType = ShipComponent.ComponentType.None;
                ops.gameObject.SetActive(false);
                sound.clip = purchase;
                sound.Play();
            }
        }
        // Attach the item or unequip if it's currently equipped (and not a hull)
        bool shouldAdd = LS_LocalGameController.instance.GetPlayer().EquipComponent(item.referenceID);
        ship.AssignComponent(item, shouldAdd);

        Debug.Log("Whats equipped after buy:...");
        LS_Helper.PrintIntArray(LS_LocalGameController.instance.GetPlayer().equippedComponents);
    }

    private void OnClick_ComponentType(ShipComponent.ComponentType type) {
        sound.clip = nuetral;
        sound.Play();
        if(currType == type) {
            BackButton();
            return;
        }
        currType = type;
        currentComponentsBeingDisplayed = LS_Helper.FilterByComponentType(unlockables, type);
        ops.DisplayComponentOptions(currentComponentsBeingDisplayed);
    }

    private void OnClick_Customize() {
        sound.clip = nuetral;
        sound.Play();
        if(parentOfComponentTypes.gameObject.activeInHierarchy) {
            BackButton();
        }
        else {
            //btnBack.gameObject.SetActive(true);
            parentOfComponentTypes.gameObject.SetActive(true);
        }
    }

    private void BackButton() {
        currType = ShipComponent.ComponentType.None;
        parentOfComponentTypes.gameObject.SetActive(false);
        ops.gameObject.SetActive(false);
        //btnBack.gameObject.SetActive(false);
    }
}
