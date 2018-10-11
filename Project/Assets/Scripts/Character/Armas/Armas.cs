using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Armas : MonoBehaviour
{
    //Clase abstracta usada como base para todas las armas disponibles.
    //Se crean las propiedades basicas de todas las armas.

    public abstract string Nombre(); //Nombre del arma.
    public abstract int MaxAmmo(); //Tamano maximo del cargador.
    public abstract int Ammo(); //Municion actual dentro del cargador.
    public abstract float TiempoRecarga(); //Tiempo que demora la recarga del arma.
    public abstract float CadenciaDeTiro(); //Numero de disparos por segundo.
    public abstract float Precision(); //Rango de direccion de la bala, mmientras mayor sea la precision la bala ira cercana al punto de apuntado.
    public abstract float Retroceso(); //Impulso que recibe el jugador en direccion opuesta al disparo por cada bala.
    public abstract float Alcance(); //Tiempo de vida de la particula de la bala.
    public abstract Sprite Imagen(); //Icono del arma para el inventario

    private readonly string[] info = new string[8]; //Almacena toda la informacion del arma para ser consultada despues.

    private bool player = false;
    private bool disparo;
    private bool cargando = false;
    private float timer = 0f;

    public abstract void Disparo();
    public abstract void Descartar();
    public abstract void VarAmmo(bool recarga, int x);

    [HideInInspector] public Transform aim;
    [HideInInspector] public ParticleSystem disparoSystem;
    [HideInInspector] public Inventario inventario;
    [HideInInspector] public bool activo;
    [HideInInspector] public int savedAmmo;
    [HideInInspector] public UIManager ui;

    //Se obtiene las propiedades del sistema de particulas y sus modulos para modificarlos acorde a las caracteristicas del arma actual;
    private ParticleSystem.ShapeModule disparoShape;
    private ParticleSystem.MainModule disparoData;
    private ParticleSystem.SubEmittersModule disparoSubEmitters;

    private void Start()
    {
        //Se verificasi el arma pertenece a algun personaje o no
        try
        {
            if (transform.parent.parent != null)
            {
                if (transform.parent.parent.tag == "Player")
                {
                    player = true;
                    ui = GameObject.Find("UI").GetComponent<UIManager>();
                }
                else
                    player = false;

                activo = true;
                inventario = transform.parent.GetComponent<Inventario>();
            }
            else
                activo = false;
        }

        catch
        {
            activo = false;
        }


        if (activo)
        {
            Inicializar();
        }

        aim = GameObject.Find("Aim").transform;
        GetInfo();
    }

    private void FixedUpdate()
    {
        //Si el arma pertecene al jugador, se llama  a los diferentes metodos por el Input
        if (activo && transform.parent.parent.tag == "Player")
        {
            timer += Time.deltaTime;
            disparo = Input.GetButton("Disparo");

            if (disparo)
            {
                Disparar();
            }

            if (Input.GetButtonDown("Descartar"))
                Descartar();

            if (Input.GetButtonDown("Recargar"))
                StartCoroutine(Recargar(TiempoRecarga()));
        }
    }

    //Metodo para realizar el disparo cuando se tiene la municion necesaria, caso contrario se recarga el arma
    public void Disparar()
    {
        if (!cargando)
        {
            if (Ammo() > 0)
            {
                if (timer >= CadenciaDeTiro())
                {
                    Disparo();
                    timer = 0;
                }
            }
            else
            {
                StartCoroutine(Recargar(TiempoRecarga()));
            }
        }

        if (player)
        {
            ui.ActualizarInfomacion();
        }
    }


    //El metodo Recargar usa el bool 'cargando' para evitar llamar a este metodo mientras ya se esta cargando el arma, se realiza la carga despues de x segundos que corresponde al tiempo de recarga.
    public IEnumerator Recargar(float x)
    {
        if (!cargando)
        {
            cargando = true;
            yield return new WaitForSeconds(x);
            if (cargando)
            {
                VarAmmo(true, 0);

                if (player)
                {
                    ui.ActualizarInfomacion();
                }
            }
            cargando = false;
        }
    }

    public void Inicializar()
    {
        //Se obtiene la cantidad de municion disponible en el inventario del dueno.
        switch (Nombre())
        {
            case "Pistola":
                savedAmmo = transform.parent.GetComponent<Inventario>().balasPistola;
                break;
            case "Escopeta":
                savedAmmo = transform.parent.GetComponent<Inventario>().balasEscopeta;
                break;
            case "Metralleta":
                savedAmmo = transform.parent.GetComponent<Inventario>().balasMetralleta;
                break;
            default:
                Debug.Log("Error al obtener la municion almacenada en el tipo de arma.");
                savedAmmo = 0;
                break;
        }

        //Se obtiene el componente de particulas para la simulacion de balas. Desarmado no cuenta con este componente por lo que se prevee un error en caso de estar desarmado.
        try
        {
            disparoSystem = GetComponent<ParticleSystem>();
            disparoShape = disparoSystem.shape;
            disparoData = disparoSystem.main;
            disparoSubEmitters = disparoSystem.subEmitters;

            disparoShape.angle = (100 - Precision()) / 4f;
            disparoData.duration = Alcance();

            ParticleSystem[] subParticulas = new ParticleSystem[4];
            subParticulas = GameObject.Find("Particles").GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < 4; i++)
            {
                //Debug.Log(disparoSubEmitters.GetSubEmitterSystem(i));
                disparoSubEmitters.SetSubEmitterSystem(i, subParticulas[i]);
            }
        }
        catch
        {
            Debug.Log("El arma actual no cuenta con emisor de particulas");
        }

        if (player)
        {
           // ui.ActualizarInfomacion();
        }
    }

    //Obtener la informacion del arma seleccionada.
    public void GetInfo()
    {
        info[0] = "Nombre: " + Nombre();
        info[1] = "Tamano del cargador: " + MaxAmmo().ToString();
        info[2] = "Municion: " + Ammo().ToString();
        info[3] = "Municino en inventario: " + savedAmmo.ToString();
        info[4] = "Tiempo de recarga: " + TiempoRecarga().ToString();
        info[5] = "Cadencia de tiro: " + CadenciaDeTiro().ToString();
        info[6] = "Precision: " + Precision().ToString() + "%";
        info[7] = "Retroceso: " + Retroceso().ToString();

        foreach (string x in info)
        {
            Debug.Log(x);
        }
    }
}
