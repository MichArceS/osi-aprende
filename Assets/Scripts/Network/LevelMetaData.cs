using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelMetaData : GameMetaData
{
    public string nombre_nivel;         //":"Lonchera Básica Activity 1",
    public string descripcion_nivel;    //":Captura alimentos saludable - Lento,
    public string correctas;            //Selecciones correctas
    public string incorrectas;          //

    public LevelMetaData(string id_registro, string nombre_nivel, string descripcion_nivel, string capitulo, string historia, string descripcion_cap) : base(id_registro)
    {
        tipo = "juego";
        this.nombre_nivel = nombre_nivel;
        this.descripcion_nivel = descripcion_nivel;
        this.descripcion_capitulo = descripcion_cap;
        this.nombre_capitulo = capitulo;
        this.nombre_historia = historia;
        correctas = "0";
        incorrectas = "0";
        //puntaje = "0";
    }
}