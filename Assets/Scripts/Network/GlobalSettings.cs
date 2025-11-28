using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

[Serializable]
public class GlobalSettings
{
    public string school;
    public string room;

    public GlobalSettings()
    {
        school = "";
        room = "";
    }

    public GlobalSettings(string school, string room)
    {
        this.school = school;
        this.room = room;
    }

}

