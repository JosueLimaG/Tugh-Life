using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    [Header("Config")]
    public GameObject androidUI;

    [Header("Axes")]
    public string movX = "Horizontal";
    public string movY = "Vertical";
    public string aimX = "AimX";
    public string aimY = "AimY";

    [Header("Buttons")]
    public string disparo1 = "R2";
    public string disparo2 = "L2";
    public string recarga = "Square";
    public string cambiar = "Triangle";
    public string recoger = "X";
    public string zoom = "R3";
    public string aceptar = "X";
    public string cancelar = "Circle";
    public string pausa = "Options";
    public string verOpciones = "Square";
    public string siguiente = "R1";
    public string anterior = "L1";

    [HideInInspector] public bool vDisparo;
    [HideInInspector] public bool vRecarga;
    [HideInInspector] public bool vRecoger;
    [HideInInspector] public bool vCambio;
    [HideInInspector] public bool vZoom;
    [HideInInspector] public bool vPausa;
    [HideInInspector] public float vStickX;
    [HideInInspector] public float vStickY;


    private float _movX;
    private float _movY;
    private float _aimX;
    private float _aimY;
    private bool _disparo;
    private bool _recarga;
    private bool _cambiar;
    private bool _recoger;
    private bool _zoom;
    private bool _aceptar;
    private bool _cancelar;
    private bool _pausa;
    private bool _verOpciones;
    private bool _siguiente;
    private bool _anterior;

    private bool windows = false;

    private void Start()
    {
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
            windows = true;
    }

    public bool GetButtonDown(string input)
    {
        if (windows)
            return Input.GetButtonDown(input);
        else
            return false;
    }

    public bool GetButton(string input)
    {
        if (windows)
            return Input.GetButton(input);
        else
            return false;
    }

    public float GetAxis(string input)
    {
        if (windows)
            return Input.GetAxisRaw(input);
        else
            return 0;
    }
}
