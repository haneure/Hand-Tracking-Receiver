using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTriggerToPublic : MonoBehaviour
{
    public Collider publicOther;
    public string eventName;
    public bool onTriggerEnter = false;
    public bool onTriggerExit = false;
    public bool onTriggerStay = false;


    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter = true;
        publicOther = other;
        eventName = "onTriggerEnter";
    }

    private void OnTriggerExit(Collider other)
    {
        publicOther = other;
        onTriggerExit = true;
        onTriggerEnter = false;
        onTriggerStay = false;
        eventName = "onTriggerExit";
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStay = true;
        publicOther = other;
        eventName = "onTriggerStay";
    }
}
