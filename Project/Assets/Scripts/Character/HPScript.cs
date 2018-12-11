using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPScript : MonoBehaviour
{
    private EnemyPatrolScript enemyScript;
    private MovementScript playerScript;
    private ParticleSystem myPS;
    private List<ParticleCollisionEvent> particleCollisions = new List<ParticleCollisionEvent>();
    public bool invulnerable = false;

    [Header("Vida del personaje")]
    public int hp = 5;

    private int maxHp;

    private void Start()
    {
        //Dependiendo si el personaje es enemigo o controlado por el jugador se obtiene la velocidad de movimiento por sus scripts correspondientes.
        if (gameObject.tag == "Enemigo")
        {
            enemyScript = GetComponent<EnemyPatrolScript>();
        }
        else if (gameObject.tag == "Player")
        {
            playerScript = GetComponent<MovementScript>();

            if (GameManager.instance.ps.ObtenerDatos(4, true)[4] == 1)
            {
                hp *= 2;
            }

            maxHp = hp;
        }
    }

    public void RecibirDano(int x)
    {
        if (!invulnerable)
        {
            hp -= x; //Se le quita a la vida un punto por cada bala recibida.


            if (hp <= 0)
            {
                if (gameObject.tag == "Player")
                {
                    GameManager.instance.ms.Derrota();
                }
                else
                {
                    GameManager.instance.EnemigoEliminado(enemyScript.id);
                    GetComponentInChildren<Inventario>().activa.GetComponent<Armas>().Descartar();
                    Destroy(gameObject); //Se comprueba la vida del personaje y si no tiene puntos disponibles es eliminado
                }
            }

            if (tag == "Player")
                UIManager.instance.RecibirDano(hp, maxHp);

            //if (gameObject.tag == "Enemigo")
            //{
            //  GetComponent<EnemyPatrolScript>().OirDisparo(GameObject.FindWithTag("Player").transform.position);
            //}
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

