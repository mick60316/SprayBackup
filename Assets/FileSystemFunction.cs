using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class FileSystemFunction 
{
    private const string ArchivesNameCode = "ArchivesName";
    private const string ObjIDCode = "ObjID";
    private const string ArchivesPath = "C:\\VR_Spray\\";
    private const int DefaultTextureWidth = 256;
    private const int DefaultTextureHeight = 256;

    private List<string> ObjIDList = new List<string>();



    public FileSystemFunction()
    {
        /*
         Initlization class

        確認 C槽是否有指定的資料夾 (ArchivesPath)

         */


        CheckAndCreateDirectory(ArchivesPath);
    }

    public string getArchivesNames()
    {
        /*
         回傳所有已存的檔案名稱
         
         */
        return PlayerPrefs.GetString(ArchivesNameCode);
    }
    public void DeleteAll()
    {
        /*
        
        刪除目錄內的所有資料夾以及檔案並且刪除儲存好的檔案名稱
         
         */
        foreach (string folder in Directory.GetDirectories(ArchivesPath))
        {

            Directory.Delete(folder, true);
        }


        PlayerPrefs.DeleteAll();
    }
    public void SaveTexturesByTag(string Tag, string ArchivesName)
    {
        /*
         利用Tag將場景內的物件Texture存下來

        Argv:
            Tag : 欲指定的Tag
            ArchivesName : 欲存檔的檔案名稱
         */

        GameObject[] Objs = GameObject.FindGameObjectsWithTag(Tag);
        CheckArchivesRepeat(ArchivesName);
        ObjIDList.Clear();
        for (int i = 0; i < Objs.Length; i++)
        {
            FindObjectChildren(Objs[i], ArchivesName);

        }
        string SaveObjIDStr = "";
        for (int i = 0; i < ObjIDList.Count; i++)
        {
            SaveObjIDStr += ObjIDList[i] + ","; 

        }

        PlayerPrefs.SetString(ArchivesName+ObjIDCode, SaveObjIDStr);
        PlayerPrefs.Save();
    }
    public void LoadArchives(string ArchivesName)
    {
        string []  ObjIDs = PlayerPrefs.GetString(ArchivesName + ObjIDCode).Split(',');
        for (int IDIndex = 0; IDIndex < ObjIDs.Length-1; IDIndex++)
        {
            string TexturePath = ArchivesPath + ArchivesName + "\\" + ObjIDs[IDIndex] + "tex.png";
            Debug.Log(TexturePath);
            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(TexturePath))
            {
                fileData = File.ReadAllBytes(TexturePath);
                tex = new Texture2D(256, 256);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                tex.Apply();
            }
            GameObject obj = (GameObject)FindObjectFromInstanceID(Convert.ToInt32(ObjIDs[IDIndex]));
            obj.GetComponent<Renderer>().material.mainTexture = tex;
            Debug.Log(ObjIDs[IDIndex] +" "+obj.name);



        }
        
    }
    private void CheckArchivesRepeat(string NewArchivesName)
    {

        string ArchivesNameFile = PlayerPrefs.GetString(ArchivesNameCode);
        string[] ArchivesNameSplit = ArchivesNameFile.Split(',');
        bool HaveFolder = false;
        for (int ArchivesNameIndex = 0; ArchivesNameIndex < ArchivesNameSplit.Length; ArchivesNameIndex++)
        {
            if (ArchivesNameSplit[ArchivesNameIndex].Equals(NewArchivesName))
            {
                HaveFolder = true;
            }
            else
            {
                
                
            }
        }
        if (HaveFolder == false)
        {

            CheckAndCreateDirectory(ArchivesPath+"\\"+NewArchivesName);
            ArchivesNameFile += "," + NewArchivesName;
        }

        PlayerPrefs.SetString(ArchivesNameCode, ArchivesNameFile);
    }
    private void FindObjectChildren(GameObject root, string fileName)
    {
        int ChildCount = root.transform.childCount;
        if (ChildCount == 0) return;
        else
        {
            for (int i = 0; i < ChildCount; i++)
            {

                GameObject childObj = root.transform.GetChild(i).gameObject;
                SaveIDAndPath(childObj, fileName);
                FindObjectChildren(childObj, fileName);

            }

        }
    }

    private void SaveIDAndPath(GameObject obj, string fileName)
    {
        Renderer r = obj.GetComponent<Renderer>();
        if (r == null) return;
        Material m = obj.GetComponent<Renderer>().material;
        if (m == null) return;
        else
        {

            if (m.mainTexture == null)
            {
                Texture2D newTexture = new Texture2D(DefaultTextureWidth, DefaultTextureHeight, TextureFormat.ARGB32, true);
                m.mainTexture = newTexture;
            }

            Texture2D objTexture = TextureToTexture2D(m.mainTexture);
            
            byte[] texBuffer = objTexture.EncodeToPNG();
            if (texBuffer.Length == 0)
            {
                Debug.Log("Texture Buffer is NULL");
                return;
            }

            File.WriteAllBytes(ArchivesPath + "\\"+fileName + "\\" + obj.GetInstanceID() + "tex.png", texBuffer);



            string FilePath = ArchivesPath +"\\"+ fileName+"\\" + obj.GetInstanceID() + "tex.png";
            ObjIDList.Add(obj.GetInstanceID().ToString());
               // PlayerPrefs.SetString(fileName +ObjIDCode,);
            
        }

    }

    private Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }

    private static UnityEngine.Object FindObjectFromInstanceID(int iid)
    {
        return (UnityEngine.Object)typeof(UnityEngine.Object)
                .GetMethod("FindObjectFromInstanceID", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .Invoke(null, new object[] { iid });

    }
    private void CheckAndCreateDirectory(string DirectoryPath)
    {
        if (Directory.Exists(DirectoryPath))
        {
            Debug.Log("Get Folder " + ArchivesPath);
            //資料夾存在
        }
        else
        {
            //新增資料夾

            Directory.CreateDirectory(DirectoryPath);
            Debug.Log("Add Folder " + ArchivesPath);
        }
    }


}
