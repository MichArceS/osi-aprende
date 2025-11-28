using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryMetaData : GameMetaData
{

    public string descripcion_historia;     //"bla bla"
    public string duracion;                 //"180"



    public StoryMetaData(string id_registro, string duracion, string descripcion_historia, string capitulo, string descripcion_cap, string historia) : base(id_registro)
    {
        tipo = "historia";
        this.descripcion_historia = descripcion_historia;
        this.duracion = duracion;
        this.nombre_capitulo = capitulo;
        this.descripcion_capitulo = descripcion_cap;
        this.nombre_historia = historia;

    }
}
