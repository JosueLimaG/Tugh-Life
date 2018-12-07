using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

public class RankingManager : MonoBehaviour {

    string rutaDB;
    string conexion;
    public GameObject puntosPrefab;
    public Transform puntosPadre;
    public int topRank;
    public int limiteRanking;
    
    IDbConnection conexionDB;
    IDbCommand comandosDB;
    IDataReader leerDatos;

    string nombreDB = "RankingDB.sqlite";
    private List<Ranking> rankings = new List<Ranking>();

    //Use this for initialization

   //void Start ()
   // {
   //    BorrarPuntosExtra();
   //    MostrarRanking();
   // }

    void AbrirDB()
    {

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            rutaDB = Application.dataPath + "/StreamingAssets/" + nombreDB;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            rutaDB = Application.dataPath + "/Raw/" + nombreDB;
        }
        // Si es android
        else if (Application.platform == RuntimePlatform.Android)
        {
            rutaDB = Application.persistentDataPath + "/" + nombreDB;
            // Comprobar si el archivo se encuantra almacenado en persistant data
            if (!File.Exists(rutaDB))
            {
                WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + nombreDB);
                while (!loadDB.isDone)
                { }
                File.WriteAllBytes(rutaDB, loadDB.bytes);
            }
        }
        conexion = "URI=file:" + rutaDB;
        conexionDB = new SqliteConnection(conexion);
        conexionDB.Open();
    }
    
    void ObtenerRanking()
    {
        rankings.Clear();
        AbrirDB();
        comandosDB = conexionDB.CreateCommand();
        //string sqlQuery = "select * from Ranking";

        string sqlQuery = "select * from Ranking join Puntos where Ranking.PlayerId = Puntos.PlayerID";

        comandosDB.CommandText = sqlQuery;

        leerDatos = comandosDB.ExecuteReader();
        while (leerDatos.Read())
        {
            rankings.Add(new Ranking(leerDatos.GetInt32(0), leerDatos.GetString(1), leerDatos.GetInt32(2), leerDatos.GetDateTime(3), leerDatos.GetInt32(4), leerDatos.GetInt32(5), leerDatos.GetInt32(6)));
        }
        leerDatos.Close();
        leerDatos = null;
        CerrarDB();
        rankings.Sort();
    }

    public void InsertarPuntos(string correo, string celular, int puntos, string diamantes, string nivel)
    {
        AbrirDB();
        comandosDB = conexionDB.CreateCommand();
        string sqlQuery = String.Format("insert into Ranking(eMail, Phone, Score) values(\"{0}\",\"{1}\",\"{2}\")", correo, celular, puntos);
        comandosDB.CommandText = sqlQuery;
        comandosDB.ExecuteScalar();
        string sqlQuery2 = string.Format("insert into Puntos(Diamantes, Puntos, Nivel) values(\"{0}\",\"{1}\",\"{2}\")", diamantes, puntos, nivel);
        comandosDB.CommandText = sqlQuery2;
        comandosDB.ExecuteScalar();
        CerrarDB();
    }

    void BorrarPuntos(int id)
    {
        AbrirDB();
        comandosDB = conexionDB.CreateCommand();
        string sqlQuery = "delete from Ranking where PlayerId = \"" +id+"\"";
        comandosDB.CommandText = sqlQuery;
        comandosDB.ExecuteScalar();
        CerrarDB();
    }

    public void MostrarRanking()
    {
        ObtenerRanking();
        for (int i = 0; i < topRank; i++)
        {
            if (i < rankings.Count)
            {
                GameObject tempPrefab = Instantiate(puntosPrefab);
                tempPrefab.transform.SetParent(puntosPadre);
                tempPrefab.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                Ranking rankTemp = rankings[i];
                tempPrefab.GetComponent<RankingScript>().PonerPuntos("#" + (i + 1).ToString(), rankTemp.eMail, rankTemp.Score.ToString(), rankTemp.Phone.ToString(), rankTemp.Diamantes.ToString(), rankTemp.Nivel.ToString());
            }
           
        }
    }

    public void BorrarPuntosExtra()
    {
        ObtenerRanking();
        // COmpruebo que el ranking sea mas grande que el límte
        if (limiteRanking <= rankings.Count)
        {
            // invierto el ranking
            // Obtengo diferencia entre el ranking y el limite
            rankings.Reverse();
            int diferencia = rankings.Count - limiteRanking;
            // Abro DB
            //Creo commando
            // Bucle con en la diferencia
            AbrirDB();
            comandosDB = conexionDB.CreateCommand();
            for (int i = 0; i < diferencia; i++)
            {
                // borro por ID en la posición del ranking
                string sqlQuery = "delete from Ranking where PlayerId = \"" + rankings[i].Id + "\"";
                comandosDB.CommandText = sqlQuery;
                comandosDB.ExecuteScalar();
            }
            // cierro DB
            CerrarDB();
        }
    }

    void CerrarDB()
    {
        comandosDB.Dispose();
        comandosDB = null;
        conexionDB.Close();
        conexionDB = null;
    }
}
