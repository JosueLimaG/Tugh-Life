using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { mainMenu, inGame, pauseMenu }
public enum CharacterState { vivo, recargando, muerto };

public class GameManager : MonoBehaviour
{
    //Aca se almacenan los datos principales del juego para ser consultados en cualquier momento por cualquier otro objeto

    public static GameManager instance;
    public GameState gameState;
    public CharacterState characterState;
    //private UIManager ui;

    //Configuracion del jugador
    [Header("Settings")]
    public float sensibilidadDelMouse = 10f;
    public bool joystick = false;
    public bool invertirX = false;
    public bool invertitY = false;

    private List<GameObject> enemigos = new List<GameObject>();
    private int id = 0;

    [HideInInspector]
    public float radioJoystick = 8;

    void Awake()
    {
        //El script es un singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            instance.ResetGame();
            Destroy(gameObject);
        }

        //El GameManager no debe ser destruido al cambiar escena.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ChangeGameState(GameState.inGame);
    }

    public void ChangeGameState(GameState x)
    {
        switch (x)
        {
            case GameState.mainMenu:

                break;
            case GameState.inGame:
                //ui = GameObject.Find("UI").GetComponent<UIManager>();
                break;
            case GameState.pauseMenu:

                break;
            default:
                Debug.Log("Error en el estado del juego.");
                SceneManager.LoadScene(0);
                break;
        }

        gameState = x;
    }

    public void ChangeCharacterState(CharacterState x)
    {
        switch (x)
        {
            case CharacterState.vivo:

                break;
            case CharacterState.recargando:

                break;
            case CharacterState.muerto:

                break;
            default:
                Debug.Log("Error en el estado del personaje.");
                ChangeCharacterState(CharacterState.muerto);
                break;
        }

        characterState = x;
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
        Debug.Log(x);
    }

    public void Disparo(Vector3 position)
    {
        foreach(GameObject enemigo in enemigos)
        {
            enemigo.GetComponent<EnemyPatrolScript>().OirDisparo(position);
        }
    }

    private void ResetGame()
    {
        //En caso de volver a cargar la primera escena, se reestablecen los datos por defecto.
        ChangeGameState(GameState.mainMenu);
        ChangeCharacterState(CharacterState.vivo);
    }
}
