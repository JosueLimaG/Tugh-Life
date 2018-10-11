using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 dir;
    private MovementScript movement;
    private float ang;
    private bool mov;
    private Transform aim;

	void Start ()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (gameObject.tag == "Player")
            movement = GetComponent<MovementScript>();
	}
	
	void Update ()
    {
        dir = transform.InverseTransformDirection(rb.velocity);
        dir = rb.velocity;
        ang = Mathf.Atan(rb.velocity.y / rb.velocity.x) * Mathf.Rad2Deg;

        if (rb.velocity.x < 0)
            ang += 180;
        if (rb.velocity.x >= 0 && rb.velocity.y < 0)
            ang += 360;

        ang += movement.angulo;

        if (ang < 0)
            ang += 360;
        if (ang > 360)
            ang -= 306;

        if (dir.x != 0 || dir.y != 0)
        {
            anim.SetBool("Movimiento", true);
            anim.SetFloat("Angulo", ang);
        }
        else
        {
            anim.SetBool("Movimiento", false);
        }
	}
}
