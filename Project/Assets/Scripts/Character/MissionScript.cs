using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Text textoMision;

    [Header("Tipos de Enemigos")]
    public GameObject[] enemigos;

    private GameObject player;

    private float tiempo;
    private bool inicio = true;
    private bool final = false;
    private bool derrota = false;

    void Start()
    {
        GameManager.instance.ms = this;
        player = GameObject.FindWithTag("Player");
        int x = 1;
        string temp = "";
        if (sobrevivirTiempo)
        {
            temp += x + ": Sobrevivir " + tiempoASobrevivir + " segundos.\n"; x++;
            InvokeRepeating("SpawnEnemigo", 5, tiempoEntreSpawnEnemigo);
        }

        if (eliminarATodos)
            temp += x + ": Eliminar a todos los enemigos.\n"; x++;

        if (robo)
            temp += x + ": Robar " + nombreDelItemARobar + ".\n"; x++;

        if (escolta)
        {
            temp += x + ": Evitar que muera tu protegido.\n"; x++;
            temp += x + ": Llega al punto objetivo con el.\n";
        }

        temp += "\nPresiona DISPARO para continuar.";

        textoMision.text = temp;
        panel.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Disparo") && inicio)
            Empezar();

        if (Input.GetButtonDown("Disparo") && final)
            SceneManager.LoadScene(0);

        if(derrota)
        {
            if (Input.GetButtonDown("Disparo")) 
                SceneManager.LoadScene(1);
            if (Input.GetButtonDown("Recargar"))
                SceneManager.LoadScene(0);
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

    public void Victoria()
    {
        textoMision.text = "Felicidades!\nObtuviste:\nDinero: " + dinero + "\nPuntos de experiencia: " + experiencia;
        int pistola = player.transform.GetChild(0).GetComponent<Inventario>().balasPistola;
        int metralleta = player.transform.GetChild(0).GetComponent<Inventario>().balasMetralleta;
        int escopeta = player.transform.GetChild(0).GetComponent<Inventario>().balasEscopeta;
        GameManager.instance.ps.SaveMissionResults(dinero, experiencia,pistola, metralleta, escopeta);
        panel.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0;
    }

    public void Derrota()
    {
        derrota = true;
        textoMision.text = "Fallaste\n\nPresiona DISPARO para reintentar\n\nPresiona RECARGAR para ir al menu.";
        panel.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0;
    }

    public void Empezar()
    {
        panel.SetActive(false);
        hud.SetActive(true);
        inicio = false;
        Time.timeScale = 1;
    }
}
