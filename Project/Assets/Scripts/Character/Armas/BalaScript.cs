using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaScript : MonoBehaviour
{
    private LineRenderer line;
    private Vector3[] positions = new Vector3[2];

    void Start()
    {
        line = GetComponent<LineRenderer>();
        positions[0] = transform.position;
        line = GetComponent<LineRenderer>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward) * Vector3.right);
        if (hit.collider != null)
        {
            positions[1] = hit.point;
            line.SetPositions(positions);
        }
    }
}
