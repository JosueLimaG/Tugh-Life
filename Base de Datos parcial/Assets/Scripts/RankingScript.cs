using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingScript : MonoBehaviour {

    public GameObject Posicion;
    public GameObject eMail;
    public GameObject Puntos;
    public GameObject Phone;
    public GameObject Diamantes;
    public GameObject Nivel;

    public void PonerPuntos(string pos, string eMail, string puntos, string phone, string diamantes, string nivel)
    {
        this.Posicion.GetComponent<Text>().text = pos;
        this.eMail.GetComponent<Text>().text = eMail;
        this.Puntos.GetComponent<Text>().text = puntos;
        this.Phone.GetComponent<Text>().text = phone;
    }
}
