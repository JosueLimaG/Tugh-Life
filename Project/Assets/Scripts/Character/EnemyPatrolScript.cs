using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Conductas { agresivo, precavido, tranquilo };

public class EnemyPatrolScript : MonoBehaviour
{
    [Header("Puntos de patrullaje")]
    public Transform[] puntos;              //Coleccion de puntos para realizar la patrulla del NavMesh Agent

    [Header("Configuracion")]
    public float maxRange = 50;             //Rango de vision del enemigo
    public float tiempoEspera = 4f;         //Segundos que esperara el enemigo antes de dirigirse al siguiente punto de patrullaje
    public float velocidad = 7;             //Velocidad de movimiento del enemigo
    public float anguloDeVision = 100f;     //Angulo de vision conica en la que podra ver al jugador
    public float tiempoDeVision = 1f;       //Segundos que transcurren viendo al jugador antes de detectarlo

    public int id;                          //ID del enemigo. Se usa para darle alertas de disparo y mantener una lista de enemigos con vida restantes

    private int destino = 0;                //Se usa para saber a cual punto[] debe ir a continuacion
    private int distanciaDeTiro;            //Se usa para indicar al enemigo a que distancia debe usar su arma actual
    private bool esperando = false;         //El enemigo esta esperando a que se le asigne el siguiente punto de patrullaje?
    private bool siguiendoJugador = false;  //El enemigo vio al jugador y lo esta siguiendo?
    private bool aLaVista = false;          //El jugador esta a la vista del enemigo?
    private NavMeshAgent agent;             //Componente del NavMesh para darle instrucciones de movimiento
    private Inventario inventario;          //Usado para disparar/recargar
    private Transform player;               //Ubicacion del jugador
    private Transform playerTemp;           //Cuando el enemigo ve al jugador, crea una ubicacion temporal con su ultima posicion detectada. 
    private RaycastHit hit;                 //El hit del raycast usado para comprobar si hay algun objeto entre el jugador y el enemigo
    private Ray ray;                        //Rayo del raycast
    private Armas arma;                     //Usado para obtener el arma actual

    private Conductas conducta = Conductas.precavido;       //El enemigo empieza con una conducta precavida.             

    [HideInInspector] public float tiempo;                  //Usado para medir el tiempo en el que el jugador permanece a la vista del enemigo

    void Start()
    {
        id = GameManager.instance.NuevoEnemigo(gameObject);             //Se obtiene un ID unico de forma automatica
        player = GameObject.FindGameObjectWithTag("Player").transform;  //Ubicacion del jugador
        agent = GetComponent<NavMeshAgent>();  
        inventario = transform.GetChild(0).GetComponent<Inventario>();
        Invoke("ComprobaArma", 0.1f);                                   //Se comprueba el arma activa
        SiguientePunto();                                               //Se le asigna el primer punto de patrullaje
    }


    void Update()
    {
        NavMesh();

        CrearRaycast();

        FijarAngulo();

        if (aLaVista)
            arma.Disparar();
    }

    void NavMesh()
    {
        agent.speed = velocidad;                                            //Se le asigna una velocidad de movimiento

        if (!agent.pathPending && agent.remainingDistance < 0.5f)           //Se comprueba si el enemigo llego al punto de destino asignado
        {
            if (!esperando)                                                 //Se comprueba si esta esperando al siguiente punto o no
            {
                Invoke("SiguientePunto", tiempoEspera);                     //Se le asigna un nuevo punto de destino tras el tiempo de espera

                if (siguiendoJugador)                                       //Se comprueba si el enemigo estaba siguiendo al jugador
                    arma.RecargarArma();                                    //Si estaba siguiento al jugador, el enemigo recargara su arma

                agent.stoppingDistance = 0;                                 //Se le indica al jugador que no debe detenerse antes de llegar al siguiente punto. Este valor varia cuando si sigue al jugador
                siguiendoJugador = false;                                   //Se indica que no esta siguiendo al jugador
                esperando = true;                                           //Se indica que esta esperando al siguiente punto para no repetir este segmento innecesariamente
            }
        }
    }

    void CrearRaycast()
    {
        Physics.Raycast(transform.position, (player.position - transform.position), out hit, maxRange); //Se crea un raycast desde enemigo y en direccion al jugador con un rango maximo

        if (hit.transform == player)                                //Se comprueba si el raycast golpeo al jugador, en tal caso no hay obstaculos entre los dos
        {
            if (CalcularTiempo(JugadorEnAngulo()))
            {
                playerTemp = player;                                //Se guarda la posicion del jugador mientras es visible
                agent.destination = playerTemp.position;            //Se asigna como destino del enemigo a la ultima posicion conocida
                agent.stoppingDistance = distanciaDeTiro;           //Se le indica al enemigo que se detenga al estar a cierta distancia del jugador
                siguiendoJugador = true;                            //El enemigo esta siguiendo al jugador
                esperando = false;                                  //El enemigo no esta esperando a que se le asigne un punto de patrullaje
            }
        }
        else
        {
            agent.stoppingDistance = 0;                             //Si el raycast golpeo un obstaculo, el enemigo debe moverse hasta el centro del siguiente punto de patrullaje   
            CalcularTiempo(false);
        }
    }

    void FijarAngulo()
    {
        transform.LookAt(agent.destination);                        //El enemigo mira a su destino

        if (siguiendoJugador)                                       //Se comprieba si el enemigo esta siguiendo al jugador o realizando su patrulla
        {
            aLaVista = true;
            
            //arma.Disparar();                                        //Se dispara el arma del enemigo mientras lo tenga a la vista
/*
            if (arma.Ammo() == 0 && arma.MaxAmmo() == 0)            //Se comprueba si el arma tiene municion disponible
                CambiarArma();                                      //En caso de no tener mas municion, se cambia de arma
            else
                arma.Disparar();                                    //Se dispara el arma del enemigo mientras lo tenga a la vista
   */     }
        else
        {
            aLaVista = false;
        }

        transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    bool CalcularTiempo(bool x)                                     //Recibe como un bool si el jugador esta dentro del rango de visibilidad
    {
        if (x)                                                      //Si el bool es verdadero se cuenta el tiempo transcurrido
            tiempo += Time.deltaTime;
        else
            tiempo -= Time.deltaTime;                               //Si el bool es falso se devuelve el temporizador a 0

        tiempo = Mathf.Clamp(tiempo, 0, tiempoDeVision + 0.1f);     //Se limita el valor entre 0 y el tiempo maximo de visibilidad
        if (tiempo >= tiempoDeVision)                               //Si el tiempo pasa el valor maximo se devuelve verdadero
            return true;
        else
            return false;                                           //Caso contrario se devuelve falso
    }

    bool JugadorEnAngulo()
    {
        float x = player.position.x - transform.position.x;         //Se forma un vector a base de la posicion del enemigo y la del jugador
        float y = player.position.z - transform.position.z;         //para poder determinar el anguloque existe entre los dos objetos

        float angulo = Mathf.Atan(y / x) * Mathf.Rad2Deg;           //Se obtiene el angulo a base de la tangente en Radianes y se lo convierte en Grados

        if (x < 0 && y < 0)                                         //Se corrige el angulo a base del valor del vector, al sacar el angulo
            angulo = ((90 - angulo) * -1) + -90;                    //a base de la tangente, solo los valores de -90 a 90 son correctos
        else if (x < 0 && y > 0)
            angulo = 180 + angulo;

        float anguloFinal = angulo + transform.eulerAngles.y + 90;  //Al angulo obtenido se le suma un valor par alinear el angulo 0 con el frente del enemigo

        if (anguloFinal > 180)                                      //Se limita en angulo a un valor entre -180 y 180
            anguloFinal -= 360;
        else if (anguloFinal < -180)
            anguloFinal += 360;

        if (Mathf.Abs(anguloFinal) < anguloDeVision)                //Devuelve un valor booleano dependiendo si el angulo entre el jugador y en enemigo
            return false;                                            //esta dentro del valor del angulo de vision del enemigo.
        else
            return true;
    }

    void SiguientePunto()
    {
        esperando = false;                                          //Cuando se le asigna al enemigo un nuevo punto, deja de estar en espera
        agent.destination = puntos[destino].position;               //Se le asigna como destino el siguiente punto
        destino = (destino + 1) % puntos.Length;                    //El siguiente punto se calcula a partir de una suma +1 al indice del anterior valor
    }

    public void OirDisparo(Vector3 posicion)                        //Metodo publico que se llama cuando se ejecuta un disparo en la escena
    {
        if (!siguiendoJugador)                                      //Si el jugador no esta siguiendo al jugador
            agent.destination = posicion;                           //Se le indica el origen del disparo como nuevo destino
    }

    public void CambiarArma()
    {
        inventario.CambiarArma();
        ComprobaArma();
    }

    void ComprobaArma()
    {
        arma = inventario.activa.GetComponent<Armas>();
        distanciaDeTiro = arma.DistanciaDeTiro() * 4;
    }
}
