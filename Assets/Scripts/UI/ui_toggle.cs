using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ui_toggle : MonoBehaviour
{
    Toggle inGameToggle;
    public bool toggleState = true;

    // Start is called before the first frame update
    void Start()
    {
        inGameToggle = this.GetComponent<Toggle>();
        inGameToggle.isOn = toggleState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleForceGrab()
    {
        HandTracking leftHand = GameObject.Find("LeftHand").GetComponent<HandTracking>();
        HandTracking rightHand = GameObject.Find("RightHand").GetComponent<HandTracking>();
        leftHand.useRaycast = !leftHand.useRaycast;
        rightHand.useRaycast = !rightHand.useRaycast;
    }

    public void toggleShowAdvancedUI()
    {
        HandTracking leftHand = GameObject.Find("LeftHand").GetComponent<HandTracking>();
        HandTracking rightHand = GameObject.Find("RightHand").GetComponent<HandTracking>();

    }

    public void toggleValue()
    {
        inGameToggle.isOn = !inGameToggle.isOn;
    }
}
