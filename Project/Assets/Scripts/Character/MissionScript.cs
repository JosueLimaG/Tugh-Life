using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

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
    public bool sigilo;

    [Header("Recompensas de Mision")]
    public int dinero;
    public int experiencia;

    [Header("Elementos UI")]
    public GameObject panel;
    public GameObject hud;
    public GameObject pausa;
    public GameObject pausa_Button;
    public GameObject reintentar;
    public GameObject reintentar_button;
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
    private EventSystem eventSystem;

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        GameManager.instance.ms = this;
        player = GameObject.FindWithTag("Player").transform;
        Mision();
    }

    private void Update()
    {
        switch(estado)
        {
            case State._00Pausa:
                if (CrossPlatformInputManager.GetButtonDown("Options"))
                    Empezar();
                if (CrossPlatformInputManager.GetButtonDown("X"))
                    Menu();
                break;

            case State._01Mision:
                if(Input.anyKeyDown || Input.touchCount > 0)
                //if (CrossPlatformInputManager.GetButtonDown("X"))
                {
                    Empezar();
                    objetivo.gameObject.SetActive(true);
                }

                break;

            case State._02EnJuego:
                if (CrossPlatformInputManager.GetButtonDown("Options"))
                    Pausa();
                break;

            case State._03Completo:
                //if (CrossPlatformInputManager.GetButtonDown("X"))
                    //SceneManager.LoadScene(0);
                break;

            case State._04Recompensas:
                if (Input.anyKeyDown || Input.touchCount > 0)
                //if (CrossPlatformInputManager.GetButtonDown("X"))
                {
                    try
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                    catch
                    {
                        Menu();
                    }
                }
                break;

            case State._05Derrota:
                break;
        }


        if (sobrevivirTiempo && estado == State._02EnJuego)
        {
            tiempo += Time.deltaTime;

            objetivo.text = "Sobrevivir " + (Mathf.Round((tiempoASobrevivir - tiempo) * 10) / 10).ToString() + " segundos.";
            if (tiempo > tiempoASobrevivir)
                Victoria();
        }
    }

    public void Reintentar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.instance.Reset();
    }

    void SpawnEnemigo()
    {
        int x = Random.Range(0, puntoDeSpawnEnemigo.Length);
        int y = Random.Range(0, enemigos.Length);

        GameObject inst = Instantiate(enemigos[y], puntoDeSpawnEnemigo[x].position, Quaternion.identity);
        inst.transform.GetChild(2).GetChild(0).position = player.transform.position;
    }

    public void Pausa()
    {
        estado = State._00Pausa;
        Time.timeScale = 0;
        pausa.SetActive(true);
        reintentar.SetActive(false);
        eventSystem.SetSelectedGameObject(GameObject.Find("APausa"));
    }

    public void ArmaButton(int x)
    {
        player.GetComponent<MovementScript>().CambiarArma(x);
        print(x);
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

        if (sigilo) 
        {
            misionTxt += x + ": Evita ser detectado.\n"; x++;
        }

        textoMision.text = misionTxt;
        objetivo.text = misionTxt;
        objetivo.gameObject.SetActive(false);
        panel.SetActive(true);
        pausa.SetActive(false);
        hud.SetActive(false);
        meta.SetActive(false);
        reintentar.SetActive(false);
    }

    public void Empezar()
    {
        estado = State._02EnJuego;
        panel.SetActive(false);
        hud.SetActive(true);
        pausa.SetActive(false);
        reintentar.SetActive(false);
        Time.timeScale = 1;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }


    public void Victoria()
    {
        estado = State._03Completo;
        meta.SetActive(true);
        reintentar.SetActive(false);
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
        reintentar.SetActive(false);
        Time.timeScale = 0;
    }

    public void Derrota()
    {
        estado = State._05Derrota;
        panel.SetActive(false);
        hud.SetActive(false);
        reintentar.SetActive(true);
        eventSystem.SetSelectedGameObject(reintentar_button);
        Time.timeScale = 0;
        eventSystem.SetSelectedGameObject(GameObject.Find("ADerrota"));
    }

}
