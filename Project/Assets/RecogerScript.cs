using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogerScript : MonoBehaviour
{
    Transform player;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update ()
    {
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z + 2);
    }
}
