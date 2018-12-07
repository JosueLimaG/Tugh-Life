using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum State { _00Pausa, _01Mision, _02EnJuego, _03Completo, _04Recompensas, _05Derrota }

public class MissionScript : MonoBehaviour
{
    [Header("Tipo de Mision")]
    public bool sobrevivirTiempo;
    public float tiempoASobrevivir;
    public Transform[] puntoDeSpawnEnemigo;
    public float tiempoEntreSpawnEnemigo;
    public bool eliminarATodos;
    public bool robo;
    public string nombreDelItemARobar;
    public bool escolta;

    [Header("Recompensas de Mision")]
    public int dinero;
    public int experiencia;

    [Header("Elementos UI")]
    public GameObject panel;
    public GameObject hud;
    public GameObject pausa;
    public Text textoMision;
    public Text objetivo;

    [Header("Tipos de Enemigos")]
    public GameObject[] enemigos;

    [Header("Meta")]
    public GameObject meta;

    private Transform player;
    private State estado = State._01Mision;
    private float tiempo;
    private string misionTxt;

    void Start()
    {
        GameManager.instance.ms = this;
        player = GameObject.FindWithTag("Player").transform;
        Mision();
    }

    private void Update()
    {
        switch(estado)
        {
            case State._00Pausa:
                if (Input.GetButtonDown("Options"))
                    Empezar();
                break;

            case State._01Mision:

                if (Input.GetButtonDown("X"))
                {
                    Empezar();
                    objetivo.gameObject.SetActive(true);
                }

                break;

            case State._02EnJuego:
                if (Input.GetButtonDown("Options"))
                    Pausa();
                break;

            case State._03Completo:
                if (Input.GetButtonDown("X"))
                    SceneManager.LoadScene(0);
                break;

            case State._04Recompensas:
                if (Input.GetButtonDown("X"))
                {
                    try
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                    catch
                    {
                        SceneManager.LoadScene(0);
                    }
                }
                break;

            case State._05Derrota:
                if (Input.GetButtonDown("X"))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    GameManager.instance.Reset();
                }

                if (Input.GetButtonDown("Square"))
                    SceneManager.LoadScene(0);
                break;
        }


        if (sobrevivirTiempo)
        {
            tiempo += Time.deltaTime;
            if (tiempo > tiempoASobrevivir)
                Victoria();
        }
    }

    void SpawnEnemigo()
    {
        int x = Random.Range(0, puntoDeSpawnEnemigo.Length - 1);
        int y = Random.Range(0, enemigos.Length - 1);

        GameObject inst = Instantiate(enemigos[y], puntoDeSpawnEnemigo[x].position, Quaternion.identity);
        inst.transform.GetChild(2).GetChild(0).position = player.transform.position;
    }

    public void Pausa()
    {
        estado = State._00Pausa;
        Time.timeScale = 0;
        pausa.SetActive(true);
    }

    void Mision()
    {
        estado = State._01Mision;
        Time.timeScale = 0;

        int x = 1;
        misionTxt = "";

        if (sobrevivirTiempo)
        {
            misionTxt += x + ": Sobrevivir " + tiempoASobrevivir + " segundos.\n"; x++;
            InvokeRepeating("SpawnEnemigo", 5, tiempoEntreSpawnEnemigo);
        }

        if (eliminarATodos)
            misionTxt += x + ": Eliminar a todos los enemigos.\n"; x++;

        if (robo)
            misionTxt += x + ": Robar " + nombreDelItemARobar + ".\n"; x++;

        if (escolta)
        {
            misionTxt += x + ": Evitar que muera tu protegido.\n"; x++;
            misionTxt += x + ": Llega al punto objetivo con el.\n";
        }

        objetivo.text = misionTxt;
        objetivo.gameObject.SetActive(false);
        textoMision.text = misionTxt + "\nPresiona X para continuar.";
        panel.SetActive(true);
        pausa.SetActive(false);
        hud.SetActive(false);
        meta.SetActive(false);
    }

    public void Empezar()
    {
        estado = State._02EnJuego;
        panel.SetActive(false);
        hud.SetActive(true);
        pausa.SetActive(false);
        Time.timeScale = 1;
    }


    public void Victoria()
    {
        estado = State._03Completo;
        meta.SetActive(true);
    }

    public void Recompensa()
    {
        estado = State._04Recompensas;
        textoMision.text = "Felicidades!\nObtuviste:\nDinero: " + dinero + "\nPuntos de experiencia: " + experiencia;
        int pistola = player.transform.GetChild(0).GetComponent<Inventario>().balasPistola;
        int metralleta = player.transform.GetChild(0).GetComponent<Inventario>().balasMetralleta;
        int escopeta = player.transform.GetChild(0).GetComponent<Inventario>().balasEscopeta;
        GameManager.instance.ps.SaveMissionResults(dinero, experiencia, pistola, metralleta, escopeta);
        panel.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0;
    }

    public void Derrota()
    {
        estado = State._05Derrota;
        textoMision.text = "Fallaste\n\nPresiona X para reintentar\n\nPresiona CUADRADO para ir al menu.";
        panel.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0;
    }

}
