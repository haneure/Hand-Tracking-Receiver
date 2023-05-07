using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreOriginRotation : MonoBehaviour
{
    private Quaternion lastParentRotation;

    // Start is called before the first frame update
    void Start()
    {
        lastParentRotation = transform.parent.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Inverse(transform.parent.localRotation) * lastParentRotation * transform.localRotation;

        lastParentRotation = transform.parent.localRotation;
    }
}
