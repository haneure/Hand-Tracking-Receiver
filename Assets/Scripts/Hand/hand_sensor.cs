using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand_sensor : MonoBehaviour
{
    HandTracking VRHand;

    public enum Hand // your custom enumeration
    {
        left,
        right
    };

    //Hand attribute
    public string gesture;
    public Hand hand;

    // Start is called before the first frame update
    void Start()
    {
        VRHand = this.transform.parent.gameObject.transform.parent.gameObject.GetComponent<HandTracking>();
    }

    // Update is called once per frame
    void Update()
    {
        string type = VRHand.hand.ToString();
        if (type == "left") {
            gesture = VRHand.leftHandGesture;
        } else if (type == "right") {
            gesture = VRHand.rightHandGesture;
        }
    }
}
