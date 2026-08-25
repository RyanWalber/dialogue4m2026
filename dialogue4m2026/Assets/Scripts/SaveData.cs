using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int faseAtualIndex = 1;
    public bool checkpointAlcancado = false;
    public float checkpointPosX;
    public float checkpointPosY;
    public float checkpointPosZ;
    public int moedasNoCheckpoint = 0;
    public List<string> idsMoedasColetadasNoCheckpoint = new List<string>();
}