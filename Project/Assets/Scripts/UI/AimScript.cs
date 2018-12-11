using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class AimScript : MonoBehaviour
{
    private Rigidbody rb;
    private float mouseX;
    private float mouseZ;
    private float radio;
    private Camera cam;
    private Rigidbody player;
    private bool aim;
    private bool joystick;
    private GameManager manager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        manager = GameManager.instance;
        radio = manager.radioJoystick;
        joystick = manager.joystick;
    }

    private void Update()
    {
        if (GameManager.instance.windows)
        {
            if (CrossPlatformInputManager.GetButtonDown("R3"))
                aim = !aim;

            float aimX = Mathf.Round(CrossPlatformInputManager.GetAxis("AimX") * 100) / 100;
            float aimY = Mathf.Round(CrossPlatformInputManager.GetAxis("AimY") * 100) / 100;

            if (joystick && !aim)
            {
                if (Mathf.Abs(aimX) > 0f && Mathf.Abs(aimY) > 0f)
                {
                    if (manager.invertirX)
                        mouseX = -aimX * radio;
                    else
                        mouseX = aimX * radio;

                    if (manager.invertitY)
                        mouseZ = -aimY * radio;
                    else
                        mouseZ = aimY * radio;
                }
                else if (Mathf.Abs(mouseX) == 0 && Mathf.Abs(mouseZ) == 0)
                {
                    mouseX = 1 * radio;
                    mouseZ = 1 * radio;
                }

                transform.position = new Vector3(mouseX, 0, mouseZ) + player.transform.position;
            }
            else
            {
                //Se asigna al objeto Aim la velocidad de el mouse, se comprueba si estan los ejes invertidos.
                if (manager.invertirX)
                    mouseX = -aimX * manager.sensibilidadDelMouse;
                else
                    mouseX = aimX * manager.sensibilidadDelMouse;

                if (manager.invertitY)
                    mouseZ = -aimY * manager.sensibilidadDelMouse;
                else
                    mouseZ = aimY * manager.sensibilidadDelMouse;

                rb.velocity = new Vector3(mouseX, 0, mouseZ);
            }

            //Se limita la posicion del objeto Aim dentro de la vision de la camara.
            var bottomLeft = cam.ScreenToWorldPoint(Vector3.zero);
            var topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight));

            var cameraRect = new Rect(bottomLeft.x, bottomLeft.z, topRight.x - bottomLeft.x, topRight.z - bottomLeft.z);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, cameraRect.xMin, cameraRect.xMax), 5f, Mathf.Clamp(transform.position.z, cameraRect.yMin, cameraRect.yMax));

            //El Aim se mueve un poco.
            rb.AddForce(new Vector3(Mathf.Sin(Time.time * 4) * 30, 0, Mathf.Cos(Time.time * 4) * 30));

            //El objeto Aim sigue al personaje a no ser que se presione el boton Apuntar

            if (!aim)
            {
                rb.velocity += player.velocity;
            }
        }
        else
            rb.isKinematic = true;
    }
}
