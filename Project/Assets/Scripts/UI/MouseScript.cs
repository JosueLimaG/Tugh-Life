using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    Vector3 mousePos;
    Camera cam;
	// Use this for initialization
	void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
    {
        mousePos = cam.WorldToScreenPoint(transform.position);
        Debug.Log(mousePos);
	}
}
