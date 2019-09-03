using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update

    private string FileName = "";
    private string saveFileName = "";
    void Start()
    {
        GameObject[] index =  GameObject.FindGameObjectsWithTag("Obj1");
        if (PlayerPrefs.HasKey("FileName"))
        {
            saveFileName = PlayerPrefs.GetString("FileName");
            Debug.Log("saveFileName : " +saveFileName);
        }
        else {
            Debug.Log("Don't have FileName archives");
        }
    }
    public static UnityEngine.Object FindObjectFromInstanceID(int iid)
    {
        return (UnityEngine.Object)typeof(UnityEngine.Object)
                .GetMethod("FindObjectFromInstanceID", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .Invoke(null, new object[] { iid });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void saveAllTextureByTag(string tag,string fileName)
    {
        GameObject[] Objs = GameObject.FindGameObjectsWithTag(tag);
        
        for (int i = 0; i < Objs.Length; i++)
        {
            FindObjectChildren(Objs[i],FileName);


        }
    }
    void FindObjectChildren(GameObject  root,string fileName)
    {
        int ChildCount = root.transform.childCount;
        if (ChildCount == 0) return;
        else {
            for (int i = 0; i < ChildCount; i++)
            {

                GameObject childObj = root.transform.GetChild(i).gameObject;
                saveIDAndPath(childObj,fileName);
                FindObjectChildren(childObj,fileName);

            }

        }
    }
    void saveIDAndPath(GameObject obj,string fileName)
    {
        Renderer r = obj.GetComponent<Renderer>();
        if (r == null) return;
        Material m = obj.GetComponent<Renderer>().material;
        if (m == null) return;
        else {

            Texture2D t =TextureToTexture2D( m.mainTexture);
            byte [] texBuffer = t.EncodeToPNG();
            if (texBuffer.Length == 0)
            {
                Debug.Log("Texture Buffer is NULL");
                return;
            }
            File.WriteAllBytes("c:\\test\\" + fileName + obj.GetInstanceID() + "tex.png", texBuffer);



            string FilePath = "c:\\test\\" + fileName + obj.GetInstanceID() + "tex.png";
            PlayerPrefs.SetString(fileName + obj.GetInstanceID() + "tex",FilePath);

        }
        
    }



    void OnGUI()
    {
        GUIStyle EditTextStyle = new GUIStyle(GUI.skin.textField);
        EditTextStyle.fontSize = 40;
        GUIStyle ButtonStyle = new GUIStyle(GUI.skin.button);
        ButtonStyle.fontSize = 40;
        FileName = GUI.TextField(new Rect(10, 10, 200, 50), FileName,EditTextStyle);
        
        if (GUI.Button(new Rect(250,10,200,50),"Save File", ButtonStyle) ) 
        {
            PlayerPrefs.SetString("FileName", FileName);
            saveAllTextureByTag("Obj1", FileName);
            FileName = "";
            PlayerPrefs.Save();
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


}
