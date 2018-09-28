using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform aim;
    private Transform player;
    public float altura = 10;
	void Start ()
    {
        aim = GameObject.Find("Aim").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void Update ()
    {
        transform.position = new Vector3((aim.position.x + player.position.x) / 3,( aim.position.y + player.position.y) / 3, -altura);
	}
}
