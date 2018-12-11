using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Informacion del inventario acutal a ser mostrado en la interfaz
    [Header("Componentes de la IU")]
    public Image _01Arma;
    public Text _01Info;
    public Image _02Arma;
    public Text _02Info;
    private bool _antibalas;
    public Image armaPiso;
    public Image herido;
    
    //Sprites usados para mostrar las diferentes armas en el inventario
    [Header("Sprites Armas")]
    public Sprite desarmado;
    public Sprite palo;
    public Sprite cuchillo;
    public Sprite pistola;
    public Sprite escopeta;
    public Sprite metralleta;
    public Sprite antibalas;

    [Header("Sprites a recoger")]
    public Sprite desarmadoPiso;
    public Sprite paloPiso;
    public Sprite cuchilloPiso;
    public Sprite pistolaPiso;
    public Sprite escopetaPiso;
    public Sprite metralletaPiso;

    private Image red;              //Imagen que muestra el dano recibido por el jugador.
    private Inventario jugador;     //Script inventario
    private float heridoTime;
    private float min = 0;
    private float max = 1;
    private SpriteRenderer armaPisoW;

    void Awake()
    {
        //Se crea un singleton 
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        jugador = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventario>();  //Se obtiene el componente Inventario
    }

    private void Start()
    {
        armaPisoW = GameObject.Find("RecogerUI").GetComponent<SpriteRenderer>();
        if (GameManager.instance.windows == true)
            armaPisoW.gameObject.SetActive(false);
        Invoke("ActualizarInformacion", 0.5f);                  //Se llama al metodo de Actualizar Informacion con un tiempo de retraso para evitar errores.
    }                                                           //De otro modo aun no se armo la estructura solicitada y no hay informacion suficiente

    private void Update()
    {
        ActualizarArmaPiso();

        HP();
    }

    void HP()
    {
        heridoTime -= Time.deltaTime * 5;
        heridoTime = Mathf.Clamp(heridoTime, min, max);

        float value = heridoTime / max;
        herido.color = new Color(1, 1, 1, value);
    }

    public void RecibirDano(float hpActual, float hpMax)
    {
        min = hpActual;
        max = hpMax;

        heridoTime = max;
    }

    public void ActualizarInventario()
    {
        string[] items = jugador.ConsultarInventario();         //Se obtiene una coleccion con los nommbres de las armas en el inventario.
        Sprite[] inventarioJugador = new Sprite[3];             //Se crea una coleccion de sprites para almacenar temporalmente la infomacion de las armas actuales

        for (int i = 0; i < 2; i++)                             //Se obtiene la informacion de las armas acuales y se las asigna a la colleccion anterior
        {
            switch (items[i])                                   
            {
                case "Desarmado":
                    inventarioJugador[i] = desarmado;
                    break;
                case "Pistola":
                    inventarioJugador[i] = pistola;
                    break;
                case "Escopeta":
                    inventarioJugador[i] = escopeta;
                    break;
                case "Metralleta":
                    inventarioJugador[i] = metralleta;
                    break;
                case "Palo":
                    inventarioJugador[i] = palo;
                    break;
                case "Cuchillo":
                    inventarioJugador[i] = cuchillo;
                    break;
                default:
                    inventarioJugador[i] = desarmado;
                    break;
            }
        }

        _01Arma.sprite = inventarioJugador[1];                  //Se almacena los sprites obtenidos de ambas armas
        _02Arma.sprite = inventarioJugador[0];
    }

    public void ActualizarInformacion()
    {
        int[] info = jugador.InfoInventario();                  
        int[] municionActual = new int[2];
        int[] municionMaxima = new int[2];
        int[] municionGuardada = new int[2];

        municionActual[0] = info[0];
        municionMaxima[0] = info[1];
        municionGuardada[0] = info[2];
        municionActual[1] = info[3];
        municionMaxima[1] = info[4];
        municionGuardada[1] = info[5];

        if (info[6] == 0)
        {
            _01Arma.color = new Color(_01Arma.color.r, _01Arma.color.g, _01Arma.color.b, 0.25f);
            _02Arma.color = new Color(_02Arma.color.r, _02Arma.color.g, _02Arma.color.b, 1f);
        }
        else
        {
            _01Arma.color = new Color(_01Arma.color.r, _01Arma.color.g, _01Arma.color.b, 1f);
            _02Arma.color = new Color(_02Arma.color.r, _02Arma.color.g, _02Arma.color.b, 0.25f);
        }

        _02Info.text = municionActual[0].ToString() + "/" + municionMaxima[0].ToString() + "  -  " + municionGuardada[0].ToString();
        _01Info.text = municionActual[1].ToString() + "/" + municionMaxima[1].ToString() + "  -  " + municionGuardada[1].ToString();
    }

    void ActualizarArmaPiso()
    {
        try
        {
            switch (jugador.armaPiso.GetComponent<Armas>().Nombre())
            {
                case "Desarmado":
                    armaPisoW.sprite = null;
                    armaPiso.sprite = desarmadoPiso;
                    break;
                case "Palo":
                    armaPisoW.sprite = paloPiso;
                    armaPiso.sprite = paloPiso;
                    break;
                case "Cuchillo":
                    armaPisoW.sprite = cuchilloPiso;
                    armaPiso.sprite = cuchilloPiso;
                    break;
                case "Pistola":
                    armaPisoW.sprite = pistolaPiso;
                    armaPiso.sprite = pistolaPiso;
                    break;
                case "Metralleta":
                    armaPisoW.sprite = metralletaPiso;
                    armaPiso.sprite = metralletaPiso;
                    break;
                case "Escopeta":
                    armaPisoW.sprite = escopetaPiso;
                    armaPiso.sprite = escopetaPiso;
                    break;
                default:
                    armaPisoW.sprite = null;
                    armaPiso.sprite = desarmadoPiso;
                    break;
            }
        }
        catch
        {
            armaPisoW.sprite = null;
            armaPiso.sprite = desarmadoPiso;
        }
    }
}

