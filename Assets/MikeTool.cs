using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikeTool 
{

    private string saveFolderPath = Application.dataPath;



    public void setFolderPath(string saveFilePath)
    {
        this.saveFolderPath = saveFilePath;
    }
    public string getSaveFolderPath() { return saveFolderPath;}

    void Start()
    {
        Debug.Log( Application.dataPath);
    }
    
}
