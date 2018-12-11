using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class CreateStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    Joystick joy;

    float alpha;
    bool press;

    private void Start()
    {
        joy = transform.GetComponentInChildren<Joystick>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        joy.Touch(data.position);

        joy.OnPointerDown(data);

        press = true;
    }

    public void OnDrag(PointerEventData data)
    {
        joy.OnDrag(data);
    }

    private void Update()
    {
        Vector2 axis = new Vector2(CrossPlatformInputManager.GetAxisRaw("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));

        if (press)
            alpha += Time.deltaTime * 10;
        else
            alpha = 0;

        alpha = Mathf.Clamp01(alpha);

        print(alpha);
        joy.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
    }

    public void OnPointerUp(PointerEventData data)
    {
        press = false;
        joy.OnPointerUp(data);
    }
}
