using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LS_IronSourceAds : MonoBehaviour {
    public bool rewardedVideoReady = false;
    private const string YOUR_APP_KEY = "6d72649d";
    private const string debugAdPrefabLocation = "prefab_DebugAd";

    void Start() {
        // Init only if not in editor
        #if UNITY_EDITOR
            Debug.Log("In unity editor, not initializing ad platform.");
            return;
        #endif
        IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
        IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
        IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
        IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;

        IronSource.Agent.init(YOUR_APP_KEY, IronSourceAdUnits.REWARDED_VIDEO);
    }

    public bool RequestPlayRewardedVideo() {
         // Play ad only if not in editor
        #if UNITY_EDITOR
            GameObject debugAd = Instantiate(Resources.Load(debugAdPrefabLocation, typeof(GameObject))) as GameObject;
            RewardedVideoAdRewardedEvent(new IronSourcePlacement("RewardedVideo", "Stardust", 100));
            return true;
        #endif
        bool outcome = PlayAdIfReady();
        Debug.Log("\nRewarded Video Successful: " + (outcome ? "T" : "F"));
        return outcome;
    }

    void OnApplicationPause(bool isPaused) {
        IronSource.Agent.onApplicationPause(isPaused);
    }

    private bool PlayAdIfReady() {
        if(rewardedVideoReady) {
            Debug.Log("Rewarded Video ready...showing");
            IronSource.Agent.showRewardedVideo();
            return true;
        }
        else {
            Debug.Log("Rewarded Video was not ready.");
            return false;
        }
    }

    //Invoked when the RewardedVideo ad view has opened.
    //Your Activity will lose focus. Please avoid performing heavy 
    //tasks till the video ad will be closed.
    void RewardedVideoAdOpenedEvent() {
    }
    //Invoked when the RewardedVideo ad view is about to be closed.
    //Your activity will now regain its focus.
    void RewardedVideoAdClosedEvent() {
    }
    //Invoked when there is a change in the ad availability status.
    //@param - available - value will change to true when rewarded videos are available. 
    //You can then show the video by calling showRewardedVideo().
    //Value will change to false when no videos are available.
    void RewardedVideoAvailabilityChangedEvent(bool available) {
        //Change the in-app 'Traffic Driver' state according to availability.
        rewardedVideoReady = available;
    }
    //Invoked when the video ad starts playing.
    void RewardedVideoAdStartedEvent() {
        Debug.Log("the video ad starts playing");
    }
    //Invoked when the video ad finishes playing.
    void RewardedVideoAdEndedEvent() {
    }
    //Invoked when the user completed the video and should be rewarded. 
    //If using server-to-server callbacks you may ignore this events and wait for 
    //the callback from the ironSource server.
    //@param - placement - placement object which contains the reward data
    void RewardedVideoAdRewardedEvent(IronSourcePlacement placement) {
        Debug.Log("the user completed the video and should be rewarded.");
        //TODO - here you can reward the user according to the reward name and amount
        placement.getPlacementName();
        placement.getRewardName();
        placement.getRewardAmount();
    }
    //Invoked when the Rewarded Video failed to show
    //@param description - string - contains information about the failure.
    void RewardedVideoAdShowFailedEvent(IronSourceError error) {
        Debug.Log("Rewarded video failed:");
    }
}
