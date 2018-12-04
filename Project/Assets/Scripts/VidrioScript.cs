using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidrioScript : MonoBehaviour
{
    private BoxCollider bc;
    private SpriteRenderer sr;
    public Sprite roto;

	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
	}

    private void OnParticleCollision(GameObject other)
    {
        RomperVidrio();
    }

    public void RomperVidrio()
    {
        sr.sprite = roto;
        bc.enabled = false;
    }
}
