using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript
{
    [Header("Atributos Personaje")]
    public float velocidad = 8;
    public int experiencia = 0;

    [Header("Manejo de Escopeta")]
    public int e_maxAmmo = 2;
    public float e_tiempoRecarga = 8f;
    public float e_cadenciaDeTiro = 0.5f;
    public float e_precision = 60f;

    [Header("Manejo de Metralleta")]
    public int m_maxAmmo = 2;
    public float m_tiempoRecarga = 8f;
    public float m_cadenciaDeTiro = 0.5f;
    public float m_precision = 60f;

    [Header("Manejo de Pistola")]
    public int p_maxAmmo = 2;
    public float p_tiempoRecarga = 8f;
    public float p_cadenciaDeTiro = 0.5f;
    public float p_precision = 60f;

    [Header("Inventario")]
    public int dinero = 0;
    public int municionEscopeta = 12;
    public int municionMetralleta = 64;
    public int municionPistola = 24;
    public bool antibalas = false;
}
