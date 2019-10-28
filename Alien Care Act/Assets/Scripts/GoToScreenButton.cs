using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GoToScreenButton : MonoBehaviour
{
    private Button goToScreenButton;

    public string whereToGoSceneName;

    void Start()
    {
        goToScreenButton = GetComponent<Button>();
        goToScreenButton.onClick.AddListener(goToScene);
    }

    private void goToScene()
    {
        if(whereToGoSceneName != null)
        {
            SceneManager.LoadScene(whereToGoSceneName, LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("Can't go to empty scene!");
        }
    }
}
