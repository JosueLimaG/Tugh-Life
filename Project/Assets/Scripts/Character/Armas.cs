using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Armas : MonoBehaviour
{
    //Clase abstracta usada como base para todas las armas disponibles.
    //Se crean las propiedades basicas de todas las armas.
    public abstract string Nombre();                        //Nombre del arma.
    public abstract int MaxAmmo();                          //Tamano maximo del cargador.
    public abstract int Ammo();                             //Municion actual dentro del cargador.
    public abstract int DistanciaDeTiro();                  //0 = Cuerpo a cuerpo, >1 La distancia a la que el enemigo se detendra frente al jugador
    public abstract float TiempoRecarga();                  //Tiempo que demora la recarga del arma.
    public abstract float CadenciaDeTiro();                 //Numero de disparos por segundo.
    public abstract float Precision();                      //Rango de direccion de la bala, mmientras mayor sea la precision la bala ira cercana al punto de apuntado.
    public abstract float Alcance();                        //Tiempo de vida de la particula de la bala.
    public abstract bool Silenciador();                     //El arma tiene silenciador?
    public abstract Sprite Imagen();                        //Icono del arma para el inventario
    public abstract Vector3 posFix();                       //Se corrige la posicion del arma
    public abstract Vector3 rotFix();                       //Se corrige la rotacion del arma
    public abstract Vector3 rotFixPlayer();

    private readonly string[] info = new string[8];         //Almacena toda la informacion del arma para ser consultada despues.

    private bool player = false;                            //El arma esta en el inventario del jugador?
    private bool disparo;                                   //Se esta presionando el boton disparo?
    private float timer = 0f;                               //Contador utilizado para limitar el disparo
    private Coroutine cargar;                               //Se asigna la corrutina a una variable para poder ser cancelada en caso de cambiar el arma 

    public abstract void Disparo();                         //Metodo abstracto usado para el disparo del arma
    public abstract void VarAmmo(bool recarga, int x);      //Metodo usado para la variacion de municion al disparar o al recargar el arma
    public abstract void CargarHabilidades(bool player);    //Carga de las habilidades en el uso de armas;


    [Header("Sonidos")]                                     //Sonidos del arma
    public AudioClip audioDisparo;
    public AudioClip audioRecarga;

    [HideInInspector] public Transform aim;                 //Posicion de la mira
    [HideInInspector] public ParticleSystem disparoSystem;  //Particle System del arma
    [HideInInspector] public Inventario inventario;         //Inventario del jugador
    [HideInInspector] public bool activo;                   //Esta el arma activa? 
    [HideInInspector] public bool cargando = false;         //Se esta cargando el arma?
    [HideInInspector] public int savedAmmo;                 //Municion guardada

    //Se obtiene las propiedades del sistema de particulas y sus modulos para modificarlos acorde a las caracteristicas del arma actual;
    private ParticleSystem.ShapeModule disparoShape;
    private ParticleSystem.MainModule disparoData;
    private ParticleSystem.SubEmittersModule disparoSubEmitters;
    private AudioSource audioS;
    private AudioSource recargaS;
    private AnimScript anim;
    private SpriteRenderer sr;
    private SphereCollider col;

    void Awake()
    {
        col = GetComponent<SphereCollider>();
        Iniciar();
    }

    private void FixedUpdate()
    {
        //Si el arma pertecene al jugador, se llama  a los diferentes metodos por el Input
        if (activo)
        {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0, CadenciaDeTiro() + 0.1f);
            if (transform.parent.parent.tag == "Player")
            {
                disparo = Input.GetButton("Disparo");

                if (disparo)
                {
                    Disparar();
                }

                // if (Input.GetButtonDown("Descartar"))
                //Descartar();

                if (Input.GetButtonDown("Recargar"))
                    RecargarArma();
            }
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
                    audioS.clip = audioDisparo;
                    audioS.Play();
                    Disparo();
                    anim.disparo = true;
                    if (!Silenciador())
                        GameManager.instance.Disparo(transform.position);
                    timer = 0;
                }
            }
            else
            {
                RecargarArma();
            }
        }

        if (player)
        {
            UIManager.instance.ActualizarInformacion();
        }
    }

    public void RecargarArma()
    {
        Debug.Log(savedAmmo);
        if (Ammo() < MaxAmmo() && savedAmmo > 0)
            cargar = StartCoroutine(Recargar(TiempoRecarga()));
    }

    private void OnDisable()
    {
        if (cargando)
        {
            if (cargar != null)
                StopCoroutine(cargar);
            cargando = false;
            anim.Cargando(cargando);
        }
    }

    //El metodo Recargar usa el bool 'cargando' para evitar llamar a este metodo mientras ya se esta cargando el arma, se realiza la carga despues de x segundos que corresponde al tiempo de recarga.
    public IEnumerator Recargar(float x)
    {
        if (!cargando)
        {
            cargando = true;
            anim.Cargando(cargando);
            recargaS.clip = audioRecarga;
            recargaS.Play();
            yield return new WaitForSeconds(x);
            if (cargando)
            {
                VarAmmo(true, 0);

                if (player)
                {
                    UIManager.instance.ActualizarInformacion();
                }
            }
            cargando = false;
            anim.Cargando(cargando);
        }
    }

    public void Iniciar()
    {
        //Se verifica si el arma pertenece a algun personaje o no
        try
        {
            if (transform.parent.parent != null)                            //Comprueba si el arma pertenece a un personaje
            {
                if (transform.parent.parent.tag == "Player")                //Ese personaje es el jugador?
                    player = true;                                          //Se indica que el jugador es el dueño del arma
                else
                    player = false;                                         //Se indica que el personaje que tiene el arma no es el jugador

                CargarHabilidades(player);
                activo = true;                                              //Si el arma pertenece a algun pesonaje, se indica que el arma esta activa
                inventario = transform.parent.GetComponent<Inventario>();   //Al ser activa, se obtiene el componente Inventario de quien tiene el arma
            }
            else
                activo = false;                                             //Si no es activa, se indica
        }

        catch
        {
            activo = false;                                                 //Si el arma no pertenece a nadie, no es activa
        }

        sr = GetComponent<SpriteRenderer>();                                //Se obtiene el sprite rendere para asignar un sprite al arma

        if (activo)
        {
            if (Nombre() != "Desarmado")
                col.enabled = false;

            ObtenerInfo();
            transform.localPosition = posFix();
            if (transform.parent.parent.tag == "Enemigo")
                transform.localRotation = Quaternion.Euler(rotFix());
            else
                transform.localRotation = Quaternion.Euler(rotFixPlayer());
        }
        else
        {
            if (Nombre() != "Desarmado")
                col.enabled = true;

            transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        aim = GameObject.Find("Aim").transform;
        audioS = GetComponent<AudioSource>();
        recargaS = gameObject.transform.GetChild(0).GetComponent<AudioSource>();
        GetInfo();
    }

    void ObtenerInfo()
    {
        print("obteniendo info de: " + name);
        anim = transform.parent.parent.GetComponent<AnimScript>();
        sr.enabled = false;
    
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
            case "Palo":
                savedAmmo = 999;
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

            if(player)
            {
                disparoShape.rotation = new Vector3(0, 90, 0);
            }
            else
            {
                disparoShape.rotation = new Vector3(0, 90, 90);
            }


            disparoShape.angle = (100 - Precision()) / 4f;
            disparoData.duration = Alcance();

            ParticleSystem[] subParticulas = new ParticleSystem[4];
            subParticulas = transform.parent.parent.parent.GetChild(1).gameObject.GetComponentsInChildren<ParticleSystem>(); 
            for (int i = 0; i < 4; i++)
            {
                disparoSubEmitters.SetSubEmitterSystem(i, subParticulas[i]);
            }
        }
        catch
        {
            Debug.Log("El arma actual no cuenta con emisor de particulas. " + name);
        }
    }

    public void Descartar()
    {
        if (Nombre() != "Desarmado")
        {
            sr.enabled = true;
            gameObject.transform.parent = null;
            activo = false;
            Iniciar();
            //transform.parent.GetComponent<Inventario>().ConsultarInventario();
        }
        else
        {
            gameObject.transform.parent = null;
            activo = false;
            Iniciar();
        }

        if (cargar != null)
            StopCoroutine(cargar);
        cargando = false;
        print(name);
        anim.Cargando(cargando);
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
    }
}
