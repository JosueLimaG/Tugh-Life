using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { mainMenu, inGame, pauseMenu }

public class GameManager : MonoBehaviour
{
    //Aca se almacenan los datos principales del juego para ser consultados en cualquier momento por cualquier otro objeto

    public static GameManager instance;
    public PlayerScript ps;
    public GameState gameState;
    //private UIManager ui;

    //Configuracion del jugador
    [Header("Settings")]
    public float sensibilidadDelMouse = 15f;
    public bool joystick = false;
    public bool invertirX = false;
    public bool invertitY = false;
    public MissionScript ms;

    public AudioClip[] clips;

    private AudioSource source;
    private List<GameObject> enemigos = new List<GameObject>();
    private int id = 0;
    private int musicInt;
    [HideInInspector] public float radioJoystick = 8;
    [HideInInspector] public bool windows = false;

    void Awake()
    {
        //El script es un singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            //instance.ResetGame();
            //Destroy(gameObject);
        }

        ps = GetComponent<PlayerScript>();
        //El GameManager no debe ser destruido al cambiar escena.
        DontDestroyOnLoad(gameObject);

        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
            windows = true;
    }

    private void Start()
    {
        Reset();
        source = GetComponent<AudioSource>();
        source.clip = clips[0];
        source.Play();
    }

    private void Update()
    { 
        if (!source.isPlaying)
        {
            musicInt++;

            if (musicInt >= clips.Length)
                musicInt = 0;

            source.clip = clips[musicInt];
            source.Play();
        } 
    }

    public void Reset()
    {
        gameState = GameState.mainMenu;
        enemigos.Clear();
    }

    public int NuevoEnemigo(GameObject enemigo)
    {
        enemigos.Add(enemigo);
        id++;
        return id;
    }

    public void EnemigoEliminado(int id)
    {
        int target = 0;
        int x = 0;
        foreach(GameObject enemigo in enemigos)
        {
            if (enemigo.GetComponent<EnemyPatrolScript>().id == id)
            {
                target = x;
            }
            x++;
        }
        enemigos.RemoveAt(target);
        if (x == 1 && ms.eliminarATodos)
        {
            ms.Victoria();
        }
    }

    public void Disparo(Vector3 position)
    {
        foreach (GameObject enemigo in enemigos)
        {
            enemigo.GetComponent<EnemyPatrolScript>().OirDisparo(position);
        }
    }
}
