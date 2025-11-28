using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

[System.Serializable]
public class SessionManager : UnitySingleton<SessionManager>
{

    public string tipo = "jugador";
    public string avatar;
    public string nombre_jugador;
    public string nombre_juego = "Osi Aprende";

    public string fecha_inicio_saludo;
    public string fecha_inicio_nombre;
    public string fecha_fin_nombre;
    public string fecha_inicio_creditos = null;
    public string fecha_fin_creditos = null;

    private string V;

    public void SetPlayerInfo(string avatar, string nombre_jugador, string nombre_juego)
    {
        string school = GameStateManager.Instance.GetGlobalSettings().school;
        string room = GameStateManager.Instance.GetGlobalSettings().room;

        if (school == "" || room == "")
        {
            V = "";
        }
        else
        {
            V = "-";
        }
        this.avatar = avatar;
        this.nombre_jugador = school + V + room + V + nombre_jugador;
        this.nombre_juego = nombre_juego;
        GameStateManager.Instance.AddJsonToList(JsonUtility.ToJson(this));
    }
}

