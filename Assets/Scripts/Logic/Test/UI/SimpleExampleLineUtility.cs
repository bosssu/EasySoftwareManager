using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This example shows how to apply line simplification to a line that already contains points.
[RequireComponent(typeof(LineRenderer))]
public class SimpleExampleLineUtility : MonoBehaviour
{
    public float tolerance = 1;
    void Start()
    {
        // Get the points.
        var lineRenderer = GetComponent<LineRenderer>();

        //lineRenderer.positionCount = 100;
        //for (int i = 0; i < 100; i++)
        //{
        //    lineRenderer.SetPosition(i, new Vector3(0, Random.Range(0, 5f), transform.position.z + i));
        //}


        int pointsBefore = lineRenderer.positionCount;
        var points = new Vector3[pointsBefore];
        lineRenderer.GetPositions(points);



        // Simplify.
        var simplifiedPoints = new List<Vector3>();
        LineUtility.Simplify(points.ToList(), tolerance, simplifiedPoints);

        // Assign back to the line renderer.
        lineRenderer.positionCount = simplifiedPoints.Count;
        lineRenderer.SetPositions(simplifiedPoints.ToArray());

        Debug.Log("Line reduced from " + pointsBefore + " to " + lineRenderer.positionCount);
    }
}