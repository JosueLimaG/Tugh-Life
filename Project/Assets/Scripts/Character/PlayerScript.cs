using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public PlayerScript ps;

    [Header("Atributos Personaje")]
    public float velocidad = 8;
    public int experiencia = 20;

    private static float def_velocidad = 8;
    private static int def_experiencia = 20;

    [Header("Manejo de Escopeta")]
    public int e_maxAmmo = 2;
    public float e_tiempoRecarga = 3.5f;
    public float e_cadenciaDeTiro = 0.5f;
    public int e_numeroDeBalas = 4;
    public int e_silenciador = 0;

    private static int eDef_maxAmmo = 2;
    private static float eDef_tiempoRecarga = 3.5f;
    private static float eDef_cadenciaDeTiro = 0.5f;
    private static int eDef_numeroDeBalas = 4;
    private static int eDef_silenciador = 0;

    [Header("Manejo de Metralleta")]
    public int m_maxAmmo = 32;
    public float m_tiempoRecarga = 4f;
    public float m_cadenciaDeTiro = 0.1f;
    public float m_precision = 75f;
    public int m_silenciador = 0;

    private static int mDef_maxAmmo = 32;
    private static float mDef_tiempoRecarga = 4f;
    private static float mDef_cadenciaDeTiro = 0.1f;
    private static float mDef_precision = 75f;
    private static int mDef_silenciador = 0;

    [Header("Manejo de Pistola")]
    public int p_maxAmmo = 6;
    public float p_tiempoRecarga = 2.5f;
    public float p_cadenciaDeTiro = 0.2f;
    public float p_precision = 65f;
    public int p_silenciador = 0;

    private static int pDef_maxAmmo = 6;
    private static float pDef_tiempoRecarga = 2.5f;
    private static float pDef_cadenciaDeTiro = 0.2f;
    private static float pDef_precision = 65f;
    private static int pDef_silenciador = 0;

    [Header("Inventario")]
    public int dinero = 650;
    public int municionEscopeta = 2;
    public int municionMetralleta = 16;
    public int municionPistola = 6;
    public int antibalas = 0;

    private static int def_dinero = 650;
    private static int def_munEscopeta = 2;
    private static int def_munMetralleta = 16;
    private static int def_munPistola = 6;
    private static int def_antibalas = 0;

    public string arma1;
    public string arma2;

    private void Start()
    {
        if (ps == null)
        {
            ps = this;
        }
        else
        {
            Destroy(this);
        }

        if(PlayerPrefs.GetFloat("velocidad") != 0)
            LoadInfo();
    }

    public int ObtenerDinero()
    {
        if (PlayerPrefs.GetFloat("velocidad") != 0)
            return PlayerPrefs.GetInt("dinero");
        else
            return dinero;
    }

    public void GuardarDinero(int x)
    {
        dinero = x;
    }

    public int ObtenerXP()
    {
        if (PlayerPrefs.GetFloat("velocidad") != 0)
            return PlayerPrefs.GetInt("experiencia");
        else
            return experiencia;
    }

    public void GuardarXP(int x)
    {
        experiencia = x;
    }

    public float[] ObtenerDatos(int id, bool player)
    {
        float[] info = new float[5];
        if (player)
        {
            switch (id)
            {
                case 1:
                    info[0] = p_maxAmmo;
                    info[1] = p_tiempoRecarga;
                    info[2] = p_cadenciaDeTiro;
                    info[3] = p_precision;
                    info[4] = p_silenciador;
                    return info;
                case 2:
                    info[0] = m_maxAmmo;
                    info[1] = m_tiempoRecarga;
                    info[2] = m_cadenciaDeTiro;
                    info[3] = m_precision;
                    info[4] = m_silenciador;
                    return info;
                case 3:
                    info[0] = e_maxAmmo;
                    info[1] = e_tiempoRecarga;
                    info[2] = e_cadenciaDeTiro;
                    info[3] = e_numeroDeBalas;
                    info[4] = e_silenciador;
                    return info;
                case 4:
                    info[0] = dinero;
                    info[1] = municionEscopeta;
                    info[2] = municionMetralleta;
                    info[3] = municionPistola;
                    info[4] = antibalas;
                    return info;
                default:
                    return null;
            }
        }
        else
        {
            switch (id)
            {
                case 1:
                    info[0] = pDef_maxAmmo;
                    info[1] = pDef_tiempoRecarga;
                    info[2] = pDef_cadenciaDeTiro;
                    info[3] = pDef_precision;
                    info[4] = pDef_silenciador;
                    return info;
                case 2:
                    info[0] = mDef_maxAmmo;
                    info[1] = mDef_tiempoRecarga;
                    info[2] = mDef_cadenciaDeTiro;
                    info[3] = mDef_precision;
                    info[4] = mDef_silenciador;
                    return info;
                case 3:
                    info[0] = eDef_maxAmmo;
                    info[1] = eDef_tiempoRecarga;
                    info[2] = eDef_cadenciaDeTiro;
                    info[3] = eDef_numeroDeBalas;
                    info[4] = eDef_silenciador;
                    return info;
                case 4:
                    info[0] = def_dinero;
                    info[1] = def_munEscopeta;
                    info[2] = def_munMetralleta;
                    info[3] = def_munPistola;
                    info[4] = def_antibalas;
                    return info;
                default:
                    return null;
            }
        }
    }

    public void CambiarDatos(string nombre, float[] info)
    {
        switch (nombre)
        {
            case "Pistola":
                p_maxAmmo = (int)info[0];
                p_tiempoRecarga = info[1];
                p_cadenciaDeTiro = info[2];
                p_precision = info[3];
                p_silenciador = (int)info[4];
                break;
            case "Metralleta":
                m_maxAmmo = (int)info[0];
                m_tiempoRecarga = info[1];
                m_cadenciaDeTiro = info[2];
                m_precision = info[3];
                m_silenciador = (int)info[4];
                break;
            case "Escopeta":
                e_maxAmmo = (int)info[0];
                e_tiempoRecarga = info[1];
                e_cadenciaDeTiro = info[2];
                e_numeroDeBalas = (int)info[3];
                e_silenciador = (int)info[4];
                break;
            case "Municion":
                dinero = (int)info[0];
                municionEscopeta = (int)info[1];
                municionMetralleta = (int)info[2];
                municionPistola = (int)info[3];
                antibalas = (int)info[4];
                break;
        }
    }

    public void ResetearHabilidades()
    {
        p_maxAmmo = pDef_maxAmmo;
        p_tiempoRecarga = pDef_tiempoRecarga;
        p_cadenciaDeTiro = pDef_cadenciaDeTiro;
        p_precision = pDef_precision;
        p_silenciador = pDef_silenciador;
        m_maxAmmo = mDef_maxAmmo;
        m_tiempoRecarga = mDef_tiempoRecarga;
        m_cadenciaDeTiro = mDef_cadenciaDeTiro;
        m_precision = mDef_precision;
        m_silenciador = mDef_silenciador;
        e_maxAmmo = eDef_maxAmmo;
        e_tiempoRecarga = eDef_tiempoRecarga;
        e_cadenciaDeTiro = eDef_cadenciaDeTiro;
        e_numeroDeBalas = eDef_numeroDeBalas;
        e_silenciador = eDef_silenciador;
        dinero = def_dinero;
        municionEscopeta = def_munEscopeta;
        municionMetralleta = def_munMetralleta;
        municionPistola = def_munPistola;
        antibalas = def_antibalas;
        velocidad = def_velocidad;
        experiencia = def_experiencia;

        int[,] puntos = new int[4, 5];
        PlayerPrefs.DeleteAll();
        SaveInfo(puntos);
    }

    public void SaveMissionResults(int newdinero, int newxp, int newmunpistola, int newmunmetralleta, int newmunescopeta)
    {
        dinero += newdinero;
        experiencia += newxp;
        municionPistola = newmunpistola;
        municionMetralleta = newmunmetralleta;
        municionEscopeta = newmunescopeta;
        antibalas = 0;
        PlayerPrefs.SetInt("dinero", dinero);
        PlayerPrefs.SetInt("experiencia", experiencia);
        PlayerPrefs.SetInt("municionEscopeta", municionEscopeta);
        PlayerPrefs.SetInt("municionMetralleta", municionMetralleta);
        PlayerPrefs.SetInt("municionPistola", municionPistola);
        PlayerPrefs.SetInt("antibalas", antibalas);
    }

    public void SaveInfo(int[,] puntos)
    {
        PlayerPrefs.SetFloat("velocidad", velocidad);
        PlayerPrefs.SetInt("experiencia", experiencia);
        PlayerPrefs.SetInt("e_maxAmmo", e_maxAmmo);
        PlayerPrefs.SetFloat("e_tiempoRecarga", e_tiempoRecarga);
        PlayerPrefs.SetFloat("e_cadenciaDeTiro", e_cadenciaDeTiro);
        PlayerPrefs.SetInt("e_numeroDeBalas", e_numeroDeBalas);
        PlayerPrefs.SetInt("e_sileniador", e_silenciador);
        PlayerPrefs.SetInt("m_maxAmmo", m_maxAmmo);
        PlayerPrefs.SetFloat("m_tiempoRecarga", m_tiempoRecarga);
        PlayerPrefs.SetFloat("m_cadenciaDeTiro", m_cadenciaDeTiro);
        PlayerPrefs.SetFloat("m_precision", m_precision);
        PlayerPrefs.SetInt("m_silenciador", m_silenciador);
        PlayerPrefs.SetInt("p_maxAmmo", p_maxAmmo);
        PlayerPrefs.SetFloat("p_tiempoRecarga", p_tiempoRecarga);
        PlayerPrefs.SetFloat("p_cadenciaDeTiro", p_cadenciaDeTiro);
        PlayerPrefs.SetFloat("p_precision", p_precision);
        PlayerPrefs.SetInt("p_silenciador", p_silenciador);
        PlayerPrefs.SetInt("dinero", dinero);
        PlayerPrefs.SetInt("municionEscopeta", municionEscopeta);
        PlayerPrefs.SetInt("municionMetralleta", municionMetralleta);
        PlayerPrefs.SetInt("municionPistola", municionPistola);
        PlayerPrefs.SetInt("antibalas", antibalas);
        int data = 0;
        foreach (int x in puntos)
        {
            PlayerPrefs.SetInt("pref" + data.ToString(), x);
            data++;
        }

        PlayerPrefs.Save();
    }

    public int[,] LoadInfo()
    {
        int[,] datos = new int[4, 5];

        if (PlayerPrefs.GetFloat("velocidad") != 0)
        {
            velocidad = PlayerPrefs.GetFloat("velocidad");
            experiencia = PlayerPrefs.GetInt("experiencia");
            e_maxAmmo = PlayerPrefs.GetInt("e_maxAmmo");
            e_tiempoRecarga = PlayerPrefs.GetFloat("e_tiempoRecarga");
            e_cadenciaDeTiro = PlayerPrefs.GetFloat("e_cadenciaDeTiro");
            e_numeroDeBalas = PlayerPrefs.GetInt("e_numeroDeBalas");
            e_silenciador = PlayerPrefs.GetInt("e_silenciador");
            m_maxAmmo = PlayerPrefs.GetInt("m_maxAmmo");
            m_tiempoRecarga = PlayerPrefs.GetFloat("m_tiempoRecarga");
            m_cadenciaDeTiro = PlayerPrefs.GetFloat("m_cadenciaDeTiro");
            m_precision = PlayerPrefs.GetFloat("m_precision");
            m_silenciador = PlayerPrefs.GetInt("m_silenciador");
            p_maxAmmo = PlayerPrefs.GetInt("p_maxAmmo");
            p_tiempoRecarga = PlayerPrefs.GetFloat("p_tiempoRecarga");
            p_cadenciaDeTiro = PlayerPrefs.GetFloat("p_cadenciaDeTiro");
            p_precision = PlayerPrefs.GetFloat("p_precision");
            p_silenciador = PlayerPrefs.GetInt("p_silenciador");
            dinero = PlayerPrefs.GetInt("dinero");
            municionEscopeta = PlayerPrefs.GetInt("municionEscopeta");
            municionMetralleta = PlayerPrefs.GetInt("municionMetralleta");
            municionPistola = PlayerPrefs.GetInt("municionPistola");
            antibalas = PlayerPrefs.GetInt("antibalas");

            int data = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    datos[i, j] = PlayerPrefs.GetInt("pref" + data);
                    data++;
                }
            }
        }

        return datos;
    }

}
