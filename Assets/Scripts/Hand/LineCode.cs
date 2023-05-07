using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCode : MonoBehaviour
{
    LineRenderer lineRenderer;
    MeshCollider meshCollider;
    Mesh mesh;
    public Transform origin;
    public Transform destination;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        if (meshCollider == null)
        {
            //meshCollider = gameObject.AddComponent<MeshCollider>();
        }

        lineRenderer.startWidth = 0.07f;
        lineRenderer.endWidth = 0.07f;

        //var points = new List<Vector3>() { origin.position, destination.position };
        //_edgeCollider.points = points.Select(x =>
        //{
        //    var pos = _edgeCollider.transform.InverseTransformPoint(x);
        //    return new Vector2(pos.x, pos.y);
        //}).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPos = origin.position;
        Vector3 endPos = destination.position;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}
