using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [HideInInspector] public Image _01Arma;
    [HideInInspector] public Text _01Info;
    [HideInInspector] public Image _02Arma;
    [HideInInspector] public Text _02Info;
    [HideInInspector] public bool _antibalas;
    
    [Header("Sprites Armas")]
    public Sprite desarmado;
    public Sprite palo;
    public Sprite cuchillo;
    public Sprite pistola;
    public Sprite escopeta;
    public Sprite metralleta;
    public Sprite antibalas;
    public Sprite vacio;

    private Image red;
    private Sprite[] inventarioJugador = new Sprite[3];
    private int[] municionActual = new int[2];
    private int[] municionMaxima = new int[2];
    private int[] municionGuardada = new int[2];
    private Inventario jugador;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        jugador = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Inventario>();
    }

    private void Start()
    {
        //ActualizarInfomacion();
    }

    public void ActualizarInventario(string[] items)
    {
        for (int i = 0; i < 2; i++)
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
                    Debug.Log("Error asignando el sprite del objeto en inventario");
                    inventarioJugador[i] = desarmado;
                    break;
            }
        }

        _01Arma.sprite = inventarioJugador[1];
        _02Arma.sprite = inventarioJugador[0];
    }

    public void ActualizarInfomacion()
    {/*
        int[] info = jugador.InfoInventario();

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
        _01Info.text = municionActual[1].ToString() + "/" + municionMaxima[1].ToString() + "  -  " + municionGuardada[1].ToString();*/
    }
}

