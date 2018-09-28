using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenColliders : MonoBehaviour
{
    private float grosor = 10f;
    private Vector2 screenSize;
    private Transform top, bot, left, right;

    void Start()
    {
        top = new GameObject().transform;
        bot = new GameObject().transform;
        right = new GameObject().transform;
        left = new GameObject().transform;

        top.name = "Top";
        bot.name = "Bot";
        right.name = "Right";
        left.name = "Left";

        top.gameObject.layer = 8;
        bot.gameObject.layer = 8;
        left.gameObject.layer = 8;
        right.gameObject.layer = 8;

        top.gameObject.AddComponent<BoxCollider2D>();
        bot.gameObject.AddComponent<BoxCollider2D>();
        right.gameObject.AddComponent<BoxCollider2D>();
        left.gameObject.AddComponent<BoxCollider2D>();

        top.parent = transform;
        bot.parent = transform;
        right.parent = transform;
        left.parent = transform;

        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;

        right.localScale = new Vector3(grosor, screenSize.y * grosor * 0.5f);
        right.position = new Vector3(transform.position.x + screenSize.x + (right.localScale.x * 0.5f), 0, 0);
        left.localScale = new Vector3(grosor, screenSize.y * grosor * 0.5f);
        left.position = new Vector3(transform.position.x - screenSize.x - (left.localScale.x * 0.5f), 0, 0);
        top.localScale = new Vector3(screenSize.x * grosor * 0.5f, grosor);
        top.position = new Vector3(0, screenSize.y + (top.localScale.y * 0.5f), 0);
        bot.localScale = new Vector3(screenSize.x * grosor * 0.5f, grosor);
        bot.position = new Vector3(0, -screenSize.y - (bot.localScale.y * 0.5f), 0);
    }
}
