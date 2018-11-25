using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    Transform player;
    public float anguloDeVision = 70;
    public bool enVista = false;
    public float timepoDeVision = 5;
    float tiempo;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        if (enVista)
        {
            tiempo += Time.deltaTime;
            if(tiempo >= timepoDeVision)
            {
                Debug.Log("Visto");
            }
        }
        else
        {
            tiempo -= Time.deltaTime;
        }

        tiempo = Mathf.Clamp(tiempo, 0, timepoDeVision + 1);

        float x = player.position.x - transform.position.x;
        float y = player.position.z - transform.position.z;

        float angulo = Mathf.Atan(y / x) * Mathf.Rad2Deg;

        if (x < 0 && y < 0)
            angulo = ((90 - angulo) * -1) + -90;
        else if (x < 0 && y > 0)
            angulo = 180  + angulo;

        tiempo += Time.deltaTime;

        float anguloFinal = angulo + transform.eulerAngles.y + 90;
        if (anguloFinal > 180)
            anguloFinal-= 360;
        else if (anguloFinal < -180)
            anguloFinal += 360;

        if (Mathf.Abs(anguloFinal) < anguloDeVision)
            enVista = true;
        else
            enVista = false;

       // Debug.Log(anguloFinal + "\t" + x + "\t" + y + "\t\t" + tiempo);

        
        /*
        Vector3 pos1 = new Vector3(-4, 1, -4);
        Vector3 pos2 = player.position;
        float angulo = Vector3.SignedAngle(pos1, pos2, Vector3.forward);
        float anguloLocal = 360 - transform.eulerAngles.y;
        if (anguloLocal > 180) anguloLocal -= 360;
        //Debug.Log(pos1 + " \t " + pos2 + " \t " + angulo + "\t\t" + tiempo);

        if (angulo > 180 - anguloDeVision)
            enVista = true;
        else
            enVista = false;
        //Debug.Log(Mathf.RoundToInt(angulo * 10) / 10 + "\t" + Mathf.Round(anguloLocal) + "\t" + enVista + "\t\t" + tiempo);

        transform.eulerAngles += new Vector3(0, 0, 1f);*/
    }
}
