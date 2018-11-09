using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolScript : MonoBehaviour
{
    [Header("Puntos de patrullaje")]
    public Transform[] puntos;
    [Header("Configuracion")]
    public float maxRange = 50;
    public float tiempoEspera = 4f;
    public float velocidad = 7;

    private int destino = 0;
    private bool esperando = false;

    private NavMeshAgent agent;
    private Inventario inventario;

    private Transform player;
    private Transform playerTemp;

    private RaycastHit hit;
    private Ray ray;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        inventario = transform.GetChild(0).GetComponent<Inventario>();
        SiguientePunto();
    }

    void SiguientePunto()
    {
        esperando = false;
        agent.destination = puntos[destino].position;
        destino = (destino + 1) % puntos.Length;
    }

    void Update()
    {
        agent.speed = velocidad;
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!esperando)
            {
                Invoke("SiguientePunto", tiempoEspera);
                esperando = true;
            }
        }

        Debug.DrawRay(transform.position, (player.position - transform.position), Color.blue);

        Physics.Raycast(transform.position, (player.position - transform.position), out hit, maxRange);

        if (hit.transform == player)
        {
            playerTemp = player;
            agent.destination = playerTemp.position;
            esperando = false;
        }

        transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
