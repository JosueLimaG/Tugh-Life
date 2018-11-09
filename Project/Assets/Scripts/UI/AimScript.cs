using UnityEngine;
using System.Collections;

public class AimScript : MonoBehaviour
{
    private Rigidbody rb;
    private float mouseX;
    private float mouseZ;
    private Camera cam;
    private Rigidbody player;
    private bool aim;
    private GameManager manager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        manager = GameManager.instance;

    }

    private void Update()
    {
        //Se asigna al objeto Aim la velocidad de el mouse, se comprueba si estan los ejes invertidos.
        if (manager.invertirX)
            mouseX = -Input.GetAxis("AimX") * manager.sensibilidadDelMouse;
        else
            mouseX = Input.GetAxis("AimX") * manager.sensibilidadDelMouse;

        if (manager.invertitY)
            mouseZ = -Input.GetAxis("AimY") * manager.sensibilidadDelMouse;
        else
            mouseZ = Input.GetAxis("AimY") * manager.sensibilidadDelMouse;

        rb.velocity = new Vector3(mouseX, 0, mouseZ);

        //Se limita la posicion del objeto Aim dentro de la vision de la camara.
        var bottomLeft = cam.ScreenToWorldPoint(Vector3.zero);
        var topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight));

        var cameraRect = new Rect(bottomLeft.x, bottomLeft.z, topRight.x - bottomLeft.x, topRight.z - bottomLeft.z);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, cameraRect.xMin, cameraRect.xMax), 5f, Mathf.Clamp(transform.position.z, cameraRect.yMin, cameraRect.yMax));

        //El Aim se mueve un poco.
        rb.AddForce(new Vector3(Mathf.Sin(Time.time * 4) * 30, 0, Mathf.Cos(Time.time * 4) * 30));

        //El objeto Aim sigue al personaje a no ser que se presione el boton Apuntar
        aim = Input.GetButton("Apuntar");

        if (!aim)
        {
            rb.velocity += player.velocity;
        }
    }
}
