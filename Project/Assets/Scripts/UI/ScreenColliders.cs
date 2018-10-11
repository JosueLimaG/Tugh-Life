using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenColliders : MonoBehaviour
{
    //Script utilizado para la creacion de los limites del cursor del apuntado dentro de la vision de la camara, sin estos el cursor se saldria de la pantalla

    private float grosor = 10f;
    private Vector2 screenSize;
    private Transform top, bot, left, right;

    void Start()
    {
        //Creacion de 4 GameObjects
        top = new GameObject().transform;
        bot = new GameObject().transform;
        right = new GameObject().transform;
        left = new GameObject().transform;

        //Se los emparenta a la camara, para que siempre lo sigan
        top.parent = transform;
        bot.parent = transform;
        right.parent = transform;
        left.parent = transform;

        //Se les asigna nombres para referencia en escena
        top.name = "Top";
        bot.name = "Bot";
        right.name = "Right";
        left.name = "Left";

        //Se asigna a los objetos el layer 8, que corresponde al Aim
        top.gameObject.layer = 2;
        bot.gameObject.layer = 2;
        left.gameObject.layer = 2;
        right.gameObject.layer = 2;

        //Se anade a los GameObjects los componentes BoxCollider2D
        top.gameObject.AddComponent<BoxCollider2D>();
        bot.gameObject.AddComponent<BoxCollider2D>();
        right.gameObject.AddComponent<BoxCollider2D>();
        left.gameObject.AddComponent<BoxCollider2D>();

        //Se determina el tamano de la pantalla
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;

        //Se modifica la escala de cada collider para que cubra el area correspondiente de la pantalla
        right.localScale = new Vector3(grosor, screenSize.y * grosor * 0.5f);
        left.localScale = new Vector3(grosor, screenSize.y * grosor * 0.5f);
        top.localScale = new Vector3(screenSize.x * grosor * 0.5f, grosor);
        bot.localScale = new Vector3(screenSize.x * grosor * 0.5f, grosor);

        //Se posiciona a los GameObjects en los bordes de la pantalla
        right.position = new Vector3(transform.position.x + screenSize.x + (right.localScale.x * 0.5f), 0, 0);
        left.position = new Vector3(transform.position.x - screenSize.x - (left.localScale.x * 0.5f), 0, 0);
        top.position = new Vector3(0, screenSize.y + (top.localScale.y * 0.5f), 0);
        bot.position = new Vector3(0, -screenSize.y - (bot.localScale.y * 0.5f), 0);
    }
}
