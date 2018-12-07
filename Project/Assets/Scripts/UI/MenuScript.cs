using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [Header("Elementos UI")]
    public Image arma1;
    public Image arma2;
    public Image personaje;
    public Image armaSeleccionada;
    public Text TituloPrincipal;
    public Text[] titulos = new Text[5];
    public Text[] valores = new Text[5];
    public Text recursos;
    public Text mensaje;
    public Text armaSeleccionadaTxt;
    public EventSystem eventSystem;
    public Button scene;
    public GameObject informacionMejoras;
    public GameObject informacionArmas;

    [Header("Sprites de armas")]
    public Sprite desarmado;
    public Sprite palo;
    public Sprite cuchillo;
    public Sprite pistola;
    public Sprite metralleta;
    public Sprite escopeta;
    public Sprite antibalasSprite;

    private int dinero;
    private int municionPistola;
    private int municionMetralleta;
    private int municionEscopeta;
    private int experiencia;
    private bool antibalas;
    private Image temp;

    private int arma;
    private bool press = false;

    public int[,] mejorasDisp = new int[4, 5];
    public float[,] puntos = new float[4, 5];
    public string[,] textos = new string[4, 5]
    {
        {
            "Tamano del cargador",
            "Tiempo de recarga",
            "Cadencia de tiro",
            "Precision del disparo",
            "Silenciador"
        },
        {
            "Tamano del cargador",
            "Tiempo de recarga",
            "Cadencia de tiro",
            "Precision del disparo",
            "Silenciador"
        },
        {
            "Tamano del cargador",
            "Tiempo de recarga",
            "Cadencia de tiro",
            "Balas por disparo",
            "Silenciador"
        },
        {
            " ",
            "Municion escopeta",
            "Municion metralleta",
            "Municion pistola",
            "Chaleco antibalas"
        }
    };
    int mainSlot = 0;
    public int slotArma = 3;

    void Start()
    {
        dinero = GameManager.instance.ps.ObtenerDinero();
        experiencia = GameManager.instance.ps.ObtenerXP();
        mejorasDisp = GameManager.instance.ps.LoadInfo();

        for (int i = 1; i < 5; i++)
        {
            float[] temp = GameManager.instance.ps.ObtenerDatos(i, true);
            for (int j = 0; j < 5; j++)
            {
                puntos[i - 1, j] = temp[j];
            }
        }

        informacionArmas.SetActive(true);
        informacionMejoras.SetActive(false);
        CambiodeSlot(0, false);
        CambioArma(3, false);
        ActualizarRecursos(false, 0);
    }

    private void Update()
    {
        if (Input.GetButtonDown("R1"))
            CambioArma(1, true);
        if (Input.GetButtonDown("L1"))
            CambioArma(-1, true);

        if (Input.GetButtonDown("Triangle"))
            CambiodeSlot(1, true);
        if (Input.GetButtonDown("Circle") && !informacionMejoras.activeSelf)
            CambiodeSlot(-1, true);

        if (Input.GetButtonDown("X") && mainSlot == 2)
            StartGame();

        if (Input.GetButtonDown("X") && !informacionMejoras.activeSelf)
            CambiodeSlot(1, true);

        if (Input.GetButtonDown("Circle") && informacionMejoras.activeSelf)
            MostrarMejoras();

        if (Input.GetButtonDown("Square") && mainSlot != 2)
            MostrarMejoras();

        if (Input.GetButtonDown("Options") && mainSlot != 2)
            CambiodeSlot(1, true);
        else if (Input.GetButtonDown("Options"))
            StartGame();
    }

    public void CambioArma(int x)
    {
        CambioArma(x, true);
    }

    void CambioArma(int x, bool input)
    {
        if (input)
        {
            if (mainSlot != 2)
            {
                slotArma += x;

                if (slotArma > 4)
                    slotArma = 0;

                if (slotArma < 0)
                    slotArma = 4;

                if (x == 2)
                    slotArma = 5;
            }
        }
        else
            slotArma = x;

        MostrarInfo();
    }

    public void CambiodeSlot(int x)
    {
        CambiodeSlot(x, false);
    }

    void CambiodeSlot(int x, bool input)
    {
        Color active = new Color(1, 1, 1, 1);
        Color inactive = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        if (input)
            mainSlot += x;
        else
            mainSlot = x;

        if (mainSlot > 2)
            mainSlot = 2;
        else if (mainSlot < 0)
            mainSlot = 0;

        if (mainSlot == 2)
        {
            CambioArma(5, false);
            mensaje.gameObject.SetActive(false);
        }
        else if (mainSlot == 1)
        {
            if (arma2.sprite == pistola)
                CambioArma(0, false);
            else if (arma2.sprite == metralleta)
                CambioArma(1, false);
            else if (arma2.sprite == escopeta)
                CambioArma(2, false);
            else if (arma2.sprite == palo)
                CambioArma(3, false);
            else if (arma2.sprite == cuchillo)
                CambioArma(4, false);
        }
        else
        {
            if (arma1.sprite == pistola)
                CambioArma(0, false);
            else if (arma1.sprite == metralleta)
                CambioArma(1, false);
            else if (arma1.sprite == escopeta)
                CambioArma(2, false);
            else if (arma1.sprite == palo)
                CambioArma(3, false);
            else if (arma1.sprite == cuchillo)
                CambioArma(4, false);
        }

        switch (mainSlot)
        {
            case 0:
                informacionMejoras.SetActive(false);
                informacionArmas.SetActive(true);
                TituloPrincipal.text = "Arma principal";
                arma1.color = active;
                arma2.color = inactive;
                personaje.color = inactive;

                break;
            case 1:
                informacionMejoras.SetActive(false);
                informacionArmas.SetActive(true);
                TituloPrincipal.text = "Arma secundaria";
                arma1.color = active;
                arma2.color = active;
                personaje.color = inactive;
                break;
            case 2:
                informacionMejoras.SetActive(true);
                informacionArmas.SetActive(false);
                TituloPrincipal.text = "Inventario";
                arma1.color = active;
                arma2.color = active;
                personaje.color = active;
                break;
        }
    }

    public void AddPuntos(int columna)
    {
        CambioPuntos(columna, 1);
    }

    public void RestPuntos(int columna)
    {
        CambioPuntos(columna, -1);
    }

    void CambioPuntos(int columna, int positivo)
    {
        if (mainSlot == 0)
        {
            if (arma1.sprite == escopeta)
                arma = 2;
            else if (arma1.sprite == pistola)
                arma = 0;
            else if (arma1.sprite == metralleta)
                arma = 1;
        }
        else if (mainSlot == 1)
        {
            if (arma2.sprite == escopeta)
                arma = 2;
            else if (arma2.sprite == pistola)
                arma = 0;
            else if (arma2.sprite == metralleta)
                arma = 1;
        }
        else
            arma = 3;

        if (arma != 3)
        {
            switch (columna)
            {
                case 0:
                    if ((mejorasDisp[arma, columna] < 5 && positivo == 1) || (mejorasDisp[arma, columna] > 0 && positivo == -1))
                    {
                        if ((positivo == 1 && (mejorasDisp[arma, columna] < experiencia)) || positivo == -1)
                        {
                            switch (arma)
                            {
                                case 0:
                                    puntos[arma, columna] += 2 * positivo;
                                    break;
                                case 1:
                                    puntos[arma, columna] += 5 * positivo;
                                    break;
                                case 2:
                                    puntos[arma, columna] += 1 * positivo;
                                    break;
                            }

                            mejorasDisp[arma, columna] += positivo;

                            if (positivo < 0)
                                ActualizarRecursos(false, (mejorasDisp[arma, columna] + 1) * positivo);
                            else
                                ActualizarRecursos(false, mejorasDisp[arma, columna] * positivo);
                        }
                    }
                    break;
                case 1:
                    if ((mejorasDisp[arma, columna] < 5 && positivo == 1) || (mejorasDisp[arma, columna] > 0 && positivo == -1))
                    {
                        if ((positivo == 1 && (mejorasDisp[arma, columna] < experiencia)) || positivo == -1)
                        {
                            switch (arma)
                            {
                                case 0:
                                    puntos[arma, columna] -= 0.3f * positivo;
                                    break;
                                case 1:
                                    puntos[arma, columna] -= 0.4f * positivo;
                                    break;
                                case 2:
                                    puntos[arma, columna] -= 0.3f * positivo;
                                    break;
                            }

                            mejorasDisp[arma, columna] += positivo;

                            if (positivo < 0)
                                ActualizarRecursos(false, (mejorasDisp[arma, columna] + 1) * positivo);
                            else
                                ActualizarRecursos(false, mejorasDisp[arma, columna] * positivo);
                        }
                    }
                    break;
                case 2:
                    if ((mejorasDisp[arma, columna] < 5 && positivo == 1) || (mejorasDisp[arma, columna] > 0 && positivo == -1))
                    {
                        if ((positivo == 1 && (mejorasDisp[arma, columna] < experiencia)) || positivo == -1)
                        {
                            switch (arma)
                            {
                                case 0:
                                    puntos[arma, columna] -= 0.02f * positivo;
                                    break;
                                case 1:
                                    puntos[arma, columna] -= 0.01f * positivo;
                                    break;
                                case 2:
                                    puntos[arma, columna] -= 0.05f * positivo;
                                    break;
                            }

                            mejorasDisp[arma, columna] += positivo;

                            if (positivo < 0)
                                ActualizarRecursos(false, (mejorasDisp[arma, columna] + 1) * positivo);
                            else
                                ActualizarRecursos(false, mejorasDisp[arma, columna] * positivo);
                        }
                    }
                    break;
                case 3:
                    if ((mejorasDisp[arma, columna] < 5 && positivo == 1) || (mejorasDisp[arma, columna] > 0 && positivo == -1))
                    {
                        if ((positivo == 1 && (mejorasDisp[arma, columna] < experiencia)) || positivo == -1)
                        {
                            switch (arma)
                            {
                                case 0:
                                    puntos[arma, columna] += 7f * positivo;
                                    break;
                                case 1:
                                    puntos[arma, columna] += 5f * positivo;
                                    break;
                                case 2:
                                    puntos[arma, columna] += 1f * positivo;
                                    break;
                            }

                            mejorasDisp[arma, columna] += positivo;

                            if (positivo < 0)
                                ActualizarRecursos(false, (mejorasDisp[arma, columna] + 1) * positivo);
                            else
                                ActualizarRecursos(false, mejorasDisp[arma, columna] * positivo);
                        }
                    }
                    break;
                case 4:
                    if ((puntos[arma, columna] == 0 && dinero >= 1000) || puntos[arma,columna] == 1)
                    {
                        if (puntos[arma, columna] == 1)
                        {
                            puntos[arma, columna] = 0;
                            ActualizarRecursos(true, -1000);
                        }
                        else
                        {
                            puntos[arma, columna] = 1;
                            ActualizarRecursos(true, 1000);
                        }
                    }
                    break;
            }
        }
        else
        {
            if ((positivo == 1 && dinero >= 75) || (positivo == -1 && puntos[arma, columna] > 16))
            {
                switch (columna)
                {
                    case 1:
                        puntos[arma, columna] += 2 * positivo;
                        ActualizarRecursos(true, 75 * positivo);
                        break;
                    case 2:
                        puntos[arma, columna] += 16 * positivo;
                        ActualizarRecursos(true, 75 * positivo);
                        break;
                    case 3:
                        puntos[arma, columna] += 6 * positivo;
                        ActualizarRecursos(true, 75 * positivo);
                        break;
                    case 4:
                        if ((puntos[arma, columna] == 0 && dinero >= 400) || puntos[arma, columna] == 1)
                        {
                            if (puntos[arma, columna] == 1)
                            {
                                puntos[arma, columna] = 0;
                                ActualizarRecursos(true, -400);
                            }
                            else
                            {
                                puntos[arma, columna] = 1;
                                ActualizarRecursos(true, 400);
                            }
                        }
                        break;
                }
                mejorasDisp[arma, columna] += positivo;
            }
        }

        MostrarInfo();
    }

    void ActualizarRecursos(bool esDinero, int valor)
    {
        if (esDinero)
            dinero -= valor;
        else
            experiencia -= valor;

        recursos.text = string.Format("Dinero disponible: {0} bs.\nPuntos XP : {1}", dinero, experiencia);
    }

    public void MostrarMejoras()
    {
        informacionMejoras.SetActive(!informacionMejoras.activeSelf);
        informacionArmas.SetActive(!informacionArmas.activeSelf);

        if(informacionMejoras.activeSelf)
        {
            eventSystem.SetSelectedGameObject(GameObject.Find("SubMain"));
        }
    }

    void MostrarInfo()
    {
        for (int i = 0; i < 5; i++)
        {
            if (slotArma < 3)
            {
                titulos[i].gameObject.SetActive(true);
                mensaje.gameObject.SetActive(false);
                valores[i].text = puntos[slotArma, i].ToString();
                titulos[i].text = textos[slotArma, i].ToString();

                switch (i)
                {
                    case 0:
                        valores[i].text += " balas";
                        break;
                    case 1:
                        valores[i].text = (Mathf.Round(float.Parse(valores[i].text) * 100) / 100).ToString() + " seg";
                        break;
                    case 2:
                        valores[i].text = (Mathf.Round(float.Parse(valores[i].text) * 100) / 100).ToString() + " seg";
                        break;
                    case 3:
                        if (slotArma != 2)
                            valores[i].text += " %";
                        break;
                    case 4:
                        if (puntos[slotArma, i] == 1)
                            valores[i].text = "Si";
                        else
                            valores[i].text = "No";
                        break;
                }
            }

            else if (slotArma > 2 && slotArma < 5)
            {
                titulos[i].gameObject.SetActive(false);
                mensaje.gameObject.SetActive(true);
            }

            else if (slotArma == 5)
            {
                valores[i].text = puntos[3, i].ToString();
                titulos[i].text = textos[3, i].ToString();
                titulos[i].gameObject.SetActive(true);
                mensaje.gameObject.SetActive(false);

                switch (i)

                {
                    case 0:
                        valores[i].text = " ";
                        titulos[0].gameObject.SetActive(false);
                        break;
                    case 1:
                        valores[i].text += " balas";
                        break;
                    case 2:
                        valores[i].text += " balas";
                        break;
                    case 3:
                        valores[i].text += " balas";
                        break;
                    case 4:
                        if (valores[i].text == "1")
                            valores[i].text = "Si";
                        else
                            valores[i].text = "No";
                        break;
                }
            }


            if (mainSlot == 0)
                temp = arma1;
            else if (mainSlot == 1)
                temp = arma2;

            if (mainSlot != 2)
            {
                switch (slotArma)
                {
                    case 3:
                        temp.sprite = palo;
                        armaSeleccionadaTxt.text = "Palo";
                        break;
                    case 4:
                        temp.sprite = cuchillo;
                        armaSeleccionadaTxt.text = "Cuchillo";
                        break;
                    case 0:
                        temp.sprite = pistola;
                        armaSeleccionadaTxt.text = "Pistola";
                        break;
                    case 1:
                        temp.sprite = metralleta;
                        armaSeleccionadaTxt.text = "Metralleta";
                        break;
                    case 2:
                        temp.sprite = escopeta;
                        armaSeleccionadaTxt.text = "Escopeta";
                        break;
                }

                armaSeleccionada.sprite = temp.sprite;
            }
        }
    }

    public void ResetInfo()
    {
        GameManager.instance.ps.ResetearHabilidades();
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        GuardarInfo();
        GameManager.instance.ps.SaveInfo(mejorasDisp);
        GameManager.instance.gameState = GameState.inGame;
        SceneManager.LoadScene(int.Parse(scene.GetComponentInChildren<Text>().text));
    }

    public void ChangeScene()
    {
        int x = int.Parse(scene.GetComponentInChildren<Text>().text);
        x++;
        if (x > 3)
            x = 1;
        scene.GetComponentInChildren<Text>().text = x.ToString();
    }

    void GuardarInfo()
    {
        float[] nuevosDatos = new float[5];

        for (int i = 0; i < 5; i++)
        {
            nuevosDatos[i] = puntos[0, i];
        }

        GameManager.instance.ps.CambiarDatos("Pistola", nuevosDatos);

        for (int i = 0; i < 5; i++)
        {
            nuevosDatos[i] = puntos[1, i];
        }

        GameManager.instance.ps.CambiarDatos("Metralletea", nuevosDatos);

        for (int i = 0; i < 5; i++)
        {
            nuevosDatos[i] = puntos[2, i];
        }

        GameManager.instance.ps.CambiarDatos("Escopeta", nuevosDatos);

        for (int i = 0; i < 5; i++)
        {
            nuevosDatos[i] = puntos[3, i];
        }

        GameManager.instance.ps.CambiarDatos("Municion", nuevosDatos);

        string infArma2;
        if (arma2.sprite == pistola)
            infArma2 = "Pistola";
        else if (arma2.sprite == metralleta)
            infArma2 = "Metralleta";
        else if (arma2.sprite == escopeta)
            infArma2 = "Escopeta";
        else if (arma2.sprite == palo)
            infArma2 = "Palo";
        else
            infArma2 = "Cuchillo";

        string infArma1;
        if (arma1.sprite == pistola)
            infArma1 = "Pistola";
        else if (arma1.sprite == metralleta)
            infArma1 = "Metralleta";
        else if (arma1.sprite == escopeta)
            infArma1 = "Escopeta";
        else if (arma1.sprite == palo)
            infArma1 = "Palo";
        else
            infArma1 = "Cuchillo";

        GameManager.instance.ps.arma1 = infArma2;
        GameManager.instance.ps.arma2 = infArma1;

        GameManager.instance.ps.GuardarDinero(dinero);
        GameManager.instance.ps.GuardarXP(experiencia);
    }
}
