using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSystemBehaveior : MonoBehaviour
{

    private FileSystemFunction FileSystem;
    private string InputFileName = "";
    private string ArchivesName = "";
    private GUIStyle EditTextStyle, ButtonStyle;
    private const int SAVE_FILE_BUTTON = 0;
    private const int LOAD_FILE_BUTTON = 1;
    private const int DELETE_ALL_BUTTON = 2;
    
 
    
 
    // Start is called before the first frame update
    void Start()
    {

        FileSystem = new FileSystemFunction();
        if (PlayerPrefs.HasKey("ArchivesName"))
        {
            ArchivesName = PlayerPrefs.GetString("ArchivesName");
            Debug.Log("ArchivesName : " + ArchivesName);
        }
        else {

            Debug.Log("NULL");
        }

 
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        EditTextStyle = new GUIStyle(GUI.skin.textField);
        ButtonStyle = new GUIStyle(GUI.skin.button);
        GUIStyle LabelStyle = new GUIStyle(GUI.skin.label);
        EditTextStyle.fontSize = 40;
        ButtonStyle.fontSize = 40;
        LabelStyle.fontSize = 40;


        InputFileName = GUI.TextField(new Rect(10, 10, 200, 50), InputFileName, EditTextStyle);

        if (GUI.Button(new Rect(250, 10, 200, 50), "Save File", ButtonStyle))
        {
            ButtonClickEvent(SAVE_FILE_BUTTON);
            
        }
        if (GUI.Button(new Rect(250, 70, 200, 50), "Load File", ButtonStyle))
        {
            ButtonClickEvent(LOAD_FILE_BUTTON);
        }
        if (GUI.Button(new Rect(250, 130, 200, 50), "Delete All", ButtonStyle))
        {
            ButtonClickEvent(DELETE_ALL_BUTTON);
        }

        string  index =  FileSystem.getArchivesNames();
        
        GUI.Label(new Rect(10, 100, 200, 50), index, LabelStyle);
        
        
    }
    

    private  void ButtonClickEvent (int ButtonID)
    {

        /*
         Button  Click Event

         Argv :
            Button ID: Which Button Click
         */
        Debug.Log("Click ButtonID = " + ButtonID);
        switch (ButtonID)
        {
            case SAVE_FILE_BUTTON:

                FileSystem.SaveTexturesByTag("Obj1", InputFileName);
                InputFileName = "";

                break;
            case DELETE_ALL_BUTTON:

                FileSystem.DeleteAll();

                break;
            case LOAD_FILE_BUTTON:
                FileSystem.LoadArchives(InputFileName);
                InputFileName = "";
                break;
            default:
                break;


        }



    }




}
