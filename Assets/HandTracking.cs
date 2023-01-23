using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    public UDPReceive udpReceive;
    public GameObject[] handPoints;
    public Camera playerCamera;
    string data = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        data = udpReceive.data;

        if(data.Length != 0) {
            data = data.Remove(0, 1);
            data = data.Remove(data.Length - 1, 1);
            print(data);
            string[] points = data.Split(',');
            print(points[0]);

            for(int i = 0; i < 21; i++) {
                float x = float.Parse(points[i*3])/100;
                float y = float.Parse(points[i*3 + 1])/100;
                float z = float.Parse(points[i*3 + 2])/100;

                handPoints[i].transform.localPosition = new Vector3(x-2f,y-2f,z);

            }
        }

    }
}
