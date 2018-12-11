using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaScript : MonoBehaviour
{
    public bool indestructible = false;

    private int resistencia = 4;
    private ParticleSystem myPS;
    private List<ParticleCollisionEvent> particleCollisions = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        if (!indestructible)
        {
            if (other.tag == "Arma")
            {
                myPS = other.GetComponent<ParticleSystem>();
                int noOfCollisions = myPS.GetCollisionEvents(gameObject, particleCollisions);
                resistencia -= noOfCollisions;
            }

            if (resistencia <= 0)
                Destroy(transform.parent.gameObject);
        }
    }
}
