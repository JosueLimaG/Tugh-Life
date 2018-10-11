using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //Script utilizado para el movimiento de la camara, esta se ubica entre el jugador y la mira,
    //permitiendo ajustar el valor de la distancia y la altura, esto por si el jugador tiene varias vistas.

    private Transform aim;
    private Transform player;
    public float altura = 10;
    public float distancia = 3;

	void Start ()
    {
        aim = GameObject.Find("Aim").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void Update ()
    {
        transform.position = new Vector3((aim.position.x + player.position.x) / distancia, ( aim.position.y + player.position.y) / distancia, -altura);
	}
}
