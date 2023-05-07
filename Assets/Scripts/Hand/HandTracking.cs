using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using MathNet.Numerics;
using System;

public class HandTracking : MonoBehaviour
{
    public UDPReceive udpReceive;
    public GameObject[] handPoints;
    public Camera playerCamera;
    public GameObject leftHandUI;

    [SerializeField]
    private Color tintColor = Color.green;
    [SerializeField]
    private Color tintColorForRaycastAll = Color.yellow;
    [SerializeField]
    private bool multiple;
    [SerializeField]
    private GameObject detectedObject;
    [SerializeField]
    private bool detectOnce = false;
    [SerializeField]
    private Color detectedColor;

    // Hands
    public enum Hand // your custom enumeration
    {
        left,
        right
    };
    
    public Hand hand = Hand.left;
    public GameObject handSphere;

    // Raycast
    GameObject rayLine;

    // z axis
    public float positionConstant;
    public float positionFactor;
    public float positionDistanceFactor;

    // scale
    public float scaleFactor;
    public float distanceFactor;

    // Data
    string data;
    string leftHand;
    public string leftHandGesture;
    string rightHand;
    public string rightHandGesture;

    // UI
    GameObject leftDetectedGestureUI;
    GameObject rightDetectedGestureUI;

    void Start()
    {
        leftDetectedGestureUI = GameObject.Find("LeftGesture");
        rightDetectedGestureUI = GameObject.Find("RightGesture");
        handSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        handSphere.transform.localScale = new Vector3(0, 0, 0);
        handSphere.transform.parent = this.transform;
        handSphere.AddComponent<Rigidbody>();
        handSphere.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        handSphere.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rayLine = new GameObject("RayLine");
        rayLine.transform.SetParent(handSphere.transform, false);
        rayLine.AddComponent<LineRenderer>();
        rayLine.GetComponent<LineRenderer>().enabled = false;
    }

    void Update()
    {
        data = udpReceive.data;
        JSONNode node = JSON.Parse(data);
        JSONObject handLm = node.AsObject;

        if (hand == Hand.left)
        {
            if (handLm != null)
            {
                leftHand = handLm["left"]["lmList"].Value;
                leftHandGesture = handLm["left"]["gesture"].Value.ToLower();

                if (leftHand.Length != 0)
                {
                    // Detected gesture
                    leftDetectedGestureUI.GetComponent<Text>().text = "Left: " + leftHandGesture;

                    leftHand = leftHand.Remove(0, 1);
                    leftHand = leftHand.Remove(leftHand.Length - 1, 1);
                    string[] points = leftHand.Split(',');

                    showLeftHandUI(points);
                    calculatePosition(points, this);
                    if (multiple)
                        RaycastMultiple();
                    else
                        RaycastSingle();
                }
                else
                {
                    this.transform.localScale = new Vector3(0, 0, 0);
                }
            }

        } else if (hand == Hand.right)
        {
            if (handLm != null)
            {
                rightHand = handLm["right"]["lmList"].Value;
                rightHandGesture = handLm["right"]["gesture"].Value.ToLower();

                if (rightHand.Length != 0)
                {
                    // Detected gesture
                    rightDetectedGestureUI.GetComponent<Text>().text = "Right: " + rightHandGesture;
                    rightHand = rightHand.Remove(0, 1);
                    rightHand = rightHand.Remove(rightHand.Length - 1, 1);
                    string[] points = rightHand.Split(',');

                    calculatePosition(points, this);
                    if (multiple)
                        RaycastMultiple();
                    else
                        RaycastSingle();
                }
                else
                {
                    this.transform.localScale = new Vector3(0, 0, 0);
                }
            }
        }
    }

    private void RaycastSingle()
    {
        if (handSphere)
        {
            LineRenderer rayLineRenderer = rayLine.GetComponent<LineRenderer>();
            rayLineRenderer.startWidth = 0.05f;
            rayLineRenderer.endWidth = 0.05f;
            Vector3 origin = handSphere.transform.position;
            Vector3 direction = handSphere.transform.forward;

            float maxDistance = 5f;

            Debug.DrawRay(origin, direction * maxDistance, Color.red);
            Ray ray = new Ray(origin, direction);

            bool result = Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance);
            rayLineRenderer.SetPosition(0, origin);
            rayLine.GetComponent<LineRenderer>().enabled = true;
            if (result)
            {
                if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                {
                    rayLineRenderer.SetPosition(1, raycastHit.point);
                    detectedObject = raycastHit.collider.gameObject;
                    if (!detectOnce)
                    {
                        detectedColor = detectedObject.GetComponent<Renderer>().material.color;
                        detectedObject.GetComponent<AlyxGrabInteractable>().interactorObject = this.gameObject;
                        // untuk AlyxGrabInteractable
                        //detectedObject.GetComponent<AlyxGrabInteractable>().attachTransform.position = this.transform.position;
                        //detectedObject.GetComponent<AlyxGrabInteractable>().attachTransform.rotation = this.transform.rotation;
                        detectedObject.GetComponent<AlyxGrabInteractable>().isHitByRaycast = true;
                        detectedObject.GetComponent<AlyxGrabInteractable>().OnSelectedEntered();
                        Debug.Log("Ray hit " + raycastHit.collider.name);
                        detectOnce = true;
                    } 
                    
                    raycastHit.collider.GetComponent<Renderer>().material.color = tintColor;
                }
            } else
            {
                rayLineRenderer.SetPosition(1, ray.GetPoint(100));
                if (detectedObject)
                {
                    detectedObject.GetComponent<Renderer>().material.color = detectedColor;
                    detectedObject.GetComponent<AlyxGrabInteractable>().interactorObject = null;
                    detectedObject.GetComponent<AlyxGrabInteractable>().isHitByRaycast = false;
                    detectOnce = false;
                }
            }
        }
    }

    private void RaycastMultiple()
    {
        throw new NotImplementedException();
    }

    public void showLeftHandUI(string[] points)
    {
        if (leftHandGesture == "backhand")
        {
            leftHandUI.SetActive(true);
        }
        else
        {
            leftHandUI.SetActive(false);
        }

        // Left Hand UI
        if (leftHandUI.activeSelf)
        {
            leftHandUI.transform.localPosition = new Vector3(
            float.Parse(points[17 * 3]) / 100 - 0.789f,
            float.Parse(points[17 * 3 + 1]) / 100 - 0f,
            float.Parse(points[17 * 3 + 2]) / 100 - 0.2f
            );
        }
    }

    private void calculatePosition(string[] points, HandTracking handTracking)
    {
        Vector3 point0 = new Vector3(float.Parse(points[0]), float.Parse(points[1]), float.Parse(points[2]));
        Vector3 point5 = new Vector3(float.Parse(points[15]), float.Parse(points[16]), float.Parse(points[17]));
        Vector3 point9 = new Vector3(float.Parse(points[27]), float.Parse(points[28]), float.Parse(points[29]));
        //Vector3 point12 = new Vector3(float.Parse(points[36]), float.Parse(points[37]), float.Parse(points[38]));
        float distance = Mathf.Floor(Vector3.Distance(point0, point5));

        //handSphere.transform.localPosition = new Vector3(Math.Abs((point12.x / 100 - point0.x / 100)) / 2, Math.Abs((point12.y / 100 - point0.y / 100)) / 2, Math.Abs((point12.z / 100 - point0.z / 100)) / 2);


        for (int i = 0; i < 21; i++)
        {
            float x = float.Parse(points[i * 3]) / 100;
            float y = float.Parse(points[i * 3 + 1]) / 100;
            float z = float.Parse(points[i * 3 + 2]) / 100;

            handPoints[i].transform.localPosition = new Vector3(x, y, z);
        }

        // position
        //var positionConstant = -1f;
        var newPosition = positionConstant + (positionFactor * (distance * positionDistanceFactor));
        this.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y, newPosition);

        // 240an = 0.5f scale
        // 330 paling dekat camera / depan
        // 100 paling jauh dari camera / belakang
        //print("Distance: " + Mathf.Floor(distance));

        var diff = distance - 200;

        // scale
        var scaleConstant = 0.5f;

        var newScale = scaleConstant - (scaleFactor * (distance * distanceFactor));
        //var newScale = 0.5f;


        handSphere.transform.localPosition = new Vector3((point9.x / 100), (point9.y / 100) - 0.5f, (point9.z / 100));


        var sphereScale = 0.7f + (distance / 1000) * scaleFactor;
        //var sphereScale = 0.7f + (scaleFactor * (distance * distanceFactor));

        handSphere.transform.localScale = new Vector3(sphereScale, sphereScale, sphereScale);
        this.transform.localScale = new Vector3(newScale, newScale, newScale); // original 0.5f
    }
}


//if (data.Length != 0)
//{
//    data = data.Remove(0, 1);
//    data = data.Remove(data.Length - 1, 1);
//    //print(data);
//    string[] points = data.Split(',');
//    //print(points[0]);

//    for (int i = 0; i < 21; i++)
//    {
//        float x = float.Parse(points[i * 3]) / 100;
//        float y = float.Parse(points[i * 3 + 1]) / 100;
//        float z = float.Parse(points[i * 3 + 2]) / 100;

//        handPoints[i].transform.localPosition = new Vector3(x - 2f, y - 2f, z);
//    }
//}