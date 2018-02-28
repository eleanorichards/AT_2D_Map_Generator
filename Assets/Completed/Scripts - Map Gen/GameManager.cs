using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    }

    // Update is called once per frame
    private void Update()
    {
        switch (_GS)
        {
            case GameState.MENU:
                break;

            case GameState.PLAYPROC:
                break;

            case GameState.PLAYCUST:
                break;

            case GameState.EDITOR:
                break;

            default:
                break;
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}