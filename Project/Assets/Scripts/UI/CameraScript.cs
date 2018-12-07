using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //Script utilizado para el movimiento de la camara, esta se ubica entre el jugador y la mira,
    //permitiendo ajustar el valor de la distancia y la altura, esto por si el jugador tiene varias vistas.

    private Transform aim;
    private Transform player;
    private float dampX = 0.2f;
    private float dampZ = 0.2f;
    private float tiempo;
    private float altura = 10;
    private float velocityX = 0f;
    private float velocityZ = 0f;

    private bool apuntar = false;

    void Start()
    {
        aim = GameObject.Find("Aim").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Vector3 aimDir = (aim.position - player.position).normalized * 4; //*0.8f

        Vector3 targetOffset = player.position + player.forward; //*0.5f

        float posX = Mathf.SmoothDamp(transform.position.x, targetOffset.x + aimDir.x, ref velocityX, dampX);
        float posZ = Mathf.SmoothDamp(transform.position.z, targetOffset.z + aimDir.z, ref velocityZ, dampZ);

        transform.position = new Vector3(posX, altura, posZ);

        if (Input.GetButtonDown("R3"))
            apuntar = !apuntar;

        if (apuntar)
        {
            tiempo += Time.deltaTime * 5;
        }
        else
        {
            tiempo -= Time.deltaTime * 5;
        }

        tiempo = Mathf.Clamp01(tiempo);
        Camera.main.orthographicSize = Mathf.Lerp(8, 16, tiempo);
    }
}
