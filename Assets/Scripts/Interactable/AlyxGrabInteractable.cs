using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlyxGrabInteractable : MonoBehaviour
{
    public UnityEvent onInteract;

    public GameObject interactorObject;
    public bool isHitByRaycast = false;
    public float velocityThreshold = 2;
    public float jumpAngleInDegree = 2;

    private Rigidbody interactableRigidbody;
    private bool canJump = true;

    private Vector3 previousPos;

    public Transform attachTransform;
    private bool toggleOnce = false;

    private void Awake()
    {
        interactableRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        //attachTransform.SetParent(attachTransform, false);
    }

    void Update()
    {
        if (isHitByRaycast && canJump)
        {
            HandTracking handInteractor = interactorObject.GetComponent<HandTracking>();
            if (handInteractor.hand == HandTracking.Hand.left)
            { 
                //if (attachTransform)
                //{
                    //attachTransform.position = interactorObject.transform.position;
                    //attachTransform.rotation = interactorObject.transform.rotation;
                //}

                if (handInteractor.leftHandGesture == "grab")
                {
                    //interactorObject.GetComponent<HandTracking>().handSphere.GetComponent<Collider>().isTrigger = true;
                    if(!toggleOnce)
                    {
                        StartCoroutine(toggleIsTriggerTrue());
                        toggleOnce = true;
                    }
                    onInteract.Invoke();
                    //Vector3 handPos = interactorObject.transform.GetChild(0).transform.GetChild(9).transform.position;
                    //Quaternion handRot = interactorObject.transform.GetChild(0).transform.GetChild(9).transform.rotation;
                    //transform.position = new Vector3(handPos.x, handPos.y - 0.2f, handPos.z + 0.5f);
                    //transform.rotation = handRot;
                } else
                {
                    if (toggleOnce)
                    {
                        interactorObject = null;
                        StartCoroutine(toggleIsTriggerFalse());
                        toggleOnce = true;
                    }
                    
                    //interactorObject.GetComponent<HandTracking>().handSphere.GetComponent<Collider>().isTrigger = false;
                }
            }

            if (handInteractor.hand == HandTracking.Hand.right)
            {
                if (handInteractor.rightHandGesture == "grab")
                {
                    onInteract.Invoke();
                    //Vector3 handPos = interactorObject.transform.GetChild(0).transform.GetChild(9).transform.position;
                    //Quaternion handRot = interactorObject.transform.GetChild(0).transform.GetChild(9).transform.rotation;
                    //transform.position = new Vector3(handPos.x, handPos.y - 0.2f, handPos.z + 0.5f);
                    //transform.rotation = handRot;
                } else
                {
                    interactorObject = null;
                }
            }

            //AlyxInteractable

            //Vector3 velocity = (interactorObject.transform.position - previousPos) / Time.deltaTime;
            //previousPos = interactorObject.transform.position;

            //if (velocity.magnitude > velocityThreshold)
            //{
            //    //Drop();
            //    HandTracking handInteractor = interactorObject.GetComponent<HandTracking>();

            //    Debug.Log(handInteractor.hand);

            //    interactableRigidbody.velocity = ComputeVelocity();
            //    canJump = false;

            //    if (handInteractor.hand == HandTracking.Hand.left)
            //    {
            //        if (handInteractor.leftHandGesture == "grab")
            //        {
            //            interactableRigidbody.velocity = ComputeVelocity();
            //            canJump = false;
            //        }
            //    }

            //    if (handInteractor.hand == HandTracking.Hand.right)
            //    {
            //        if (handInteractor.rightHandGesture == "grab")
            //        {
            //            interactableRigidbody.velocity = ComputeVelocity();
            //            canJump = false;
            //        }
            //    }

            //    if (handInteractor.leftHandGesture == "grab" || handInteractor.rightHandGesture == "grab")
            //    {

            //    }
            //}
        }
    }

    public void Grab()
    {
        Vector3 handPos = interactorObject.transform.GetChild(0).transform.GetChild(9).transform.position;
        Quaternion handRot = interactorObject.transform.GetChild(0).transform.GetChild(9).transform.rotation;
        transform.position = new Vector3(handPos.x, handPos.y - 0.2f, handPos.z + 0.5f);
        transform.rotation = handRot;
    }

    public void forceGrab()
    {
        Vector3 handPos = interactorObject.transform.GetChild(0).transform.GetChild(9).transform.position;
        Quaternion handRot = interactorObject.transform.GetChild(0).transform.GetChild(9).transform.rotation;
        transform.position = new Vector3(handPos.x, handPos.y - 0.2f, handPos.z + 0.5f);
        transform.rotation = handRot;
    }

    public Vector3 ComputeVelocity()
    {
        Vector3 diff = interactorObject.transform.position - transform.position;
        Vector3 diffXZ = new Vector3(diff.x, 0, diff.z);
        float diffXZLength = diffXZ.magnitude;
        float diffYLength = diff.y;

        float angleInRadian = jumpAngleInDegree * Mathf.Deg2Rad;

        float jumpSpeed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(diffXZLength, 2) 
            / (2 * Mathf.Cos(angleInRadian) * Mathf.Cos(angleInRadian) * (diffXZ.magnitude * Mathf.Tan(angleInRadian) - diffYLength)));

        Vector3 jumpVelocityVector = diffXZ.normalized * Mathf.Cos(angleInRadian) * jumpSpeed + Vector3.up * Mathf.Sin(angleInRadian) * jumpSpeed;
        //jumpVelocityVector = new Vector3(jumpVelocityVector.x - 2.3f, jumpVelocityVector.y + 1.5f, jumpVelocityVector.z - 2f);
        jumpVelocityVector = new Vector3(jumpVelocityVector.x - 4f, jumpVelocityVector.y, jumpVelocityVector.z);
        return jumpVelocityVector;
    }

    public void OnSelectedEntered()
    {
        if (isHitByRaycast)
        {
            Debug.Log(interactorObject.transform.name);
            //previousPos = interactorObject.transform.position;
            previousPos = interactorObject.transform.GetChild(0).transform.GetChild(9).transform.localPosition;
            Debug.Log(interactorObject.transform.GetChild(0).transform.GetChild(9).transform.name);
            canJump = true;
        }
    }

    IEnumerator toggleIsTriggerTrue()
    {
        yield return new WaitForFixedUpdate();
        interactorObject.GetComponent<HandTracking>().handSphere.GetComponent<Collider>().isTrigger = true;
        int count = interactorObject.transform.GetChild(0).transform.childCount;
        if (interactorObject.GetComponent<HandTracking>().hand == HandTracking.Hand.left)
        {
            count -= 1;
        }
        for (int i = 0; i < count; i++)
        {
            interactorObject.transform.GetChild(0).transform.GetChild(i).GetComponent<Collider>().isTrigger = true;
        }
    }

    IEnumerator toggleIsTriggerFalse()
    {
        yield return new WaitForFixedUpdate();
        interactorObject.GetComponent<HandTracking>().handSphere.GetComponent<Collider>().isTrigger = false;
        int count = interactorObject.transform.GetChild(0).transform.childCount;
        if (interactorObject.GetComponent<HandTracking>().hand == HandTracking.Hand.left)
        {
            count -= 1;
        }
        for (int i = 0; i < count; i++)
        {
            interactorObject.transform.GetChild(0).transform.GetChild(i).GetComponent<Collider>().isTrigger = false;
        }
    }
}
