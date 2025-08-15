using UnityEngine;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using TMPro;
using System.ComponentModel;


#if UNITY_EDITOR
using UnityEditor;
#endif
public class CanvasUIHandler : MonoBehaviour
{
    public TMP_InputField inputField;

    public void StartGame()
    {
        
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void EnterName()
    {
        Debug.Log(inputField == null ? "Input field null" : "OK");
        PlayerNameController.instance.playerName = inputField.text;
        Debug.Log(PlayerNameController.instance.playerName);
    }
}
