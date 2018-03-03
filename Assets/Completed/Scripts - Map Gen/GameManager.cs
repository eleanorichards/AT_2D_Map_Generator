using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Dropdown loadFileDD;
    public string currentSelected = "";
    private List<string> levelNames = new List<string>();

    public enum GameState
    {
        MENU,
        PLAYPROC,
        PLAYCUST,
        EDITOR
    }

    public GameState _GS;

    // Use this for initialization
    private void Start()
    {
        ReturnSavedMaps();
    }

    // Update is called once per frame
    private void ReturnSavedMaps()
    {
        foreach (string file in System.IO.Directory.GetFiles(Application.persistentDataPath + "/"))
        {
            //Debug.Log(Path.GetFileName(file));
            loadFileDD.options.Add(new Dropdown.OptionData(Path.GetFileName(file)));
            levelNames.Add(file);
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void DropdownValueChanged(int levelNum)
    {
        currentSelected = levelNames[levelNum - 1];
        //currentSelected = levelName;
    }

    public string GetCurrentLevel()
    {
        return currentSelected;
    }
}