using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPScript : MonoBehaviour
{
    private EnemyPatrolScript enemyScript;
    private MovementScript playerScript;
    private float initialMovement;
    private float tiempo;
    private float velocidad;
    private ParticleSystem myPS;
    private List<ParticleCollisionEvent> particleCollisions = new List<ParticleCollisionEvent>();

    [Header("Vida del personaje")]
    public int hp = 5;

    private void Start()
    {
        //Dependiendo si el personaje es enemigo o controlado por el jugador se obtiene la velocidad de movimiento por sus scripts correspondientes.
        if (gameObject.tag == "Enemigo")
        {
            enemyScript = GetComponent<EnemyPatrolScript>();
            initialMovement = enemyScript.velocidad;
        }
        else if (gameObject.tag == "Player")
        {
            playerScript = GetComponent<MovementScript>();
            initialMovement = playerScript.velocidad;
        }
    }

    private void FixedUpdate()
    {
        //Cuando el personaje reciba un disparo el float velocidad se baja a 0, aca se lo vuelve a subir a su valor maximo en un tiempo determinado.
        velocidad += Time.deltaTime * 3;
        velocidad = Mathf.Clamp(velocidad, 0, initialMovement);

        //Se aplica el valor de velocidad al personaje.
        if (gameObject.tag == "Enemigo")
        {
            enemyScript.velocidad = velocidad;
        }
        else if (gameObject.tag == "Player")
        {
            playerScript.velocidad = velocidad;
        }
    }

    public void RecibirDano(int x)
    {
        hp -= x; //Se le quita a la vida un punto por cada bala recibida.
        if (hp > 1)
        {
            initialMovement -= initialMovement / hp; //Se limita la velocidad del movimiento del personaje dependiendo de su vida restante.
        }
        velocidad = 0; //Se baja la velocidad del personaje temporalmente.

        if (hp <= 0)
        {
            if (gameObject.tag == "Player")
            {
                Time.timeScale = 0;
            }
            else
            {
                GameManager.instance.EnemigoEliminado(enemyScript.id);
                Destroy(gameObject); //Se comprueba la vida del personaje y si no tiene puntos disponibles es eliminado
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Arma") //Se comprueba que la particula que golpeo el objeto sea de el rival.
        {
            myPS = other.GetComponent<ParticleSystem>(); //Se obtiene el ParticleSystem de quien realizo el disparo.
            int noOfCollisions = myPS.GetCollisionEvents(gameObject, particleCollisions); //Se obtiene el numero de particulas que golpearon al objeto.
            RecibirDano(noOfCollisions);
        }
    }
}

