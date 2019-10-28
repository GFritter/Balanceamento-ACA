using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuitGame : MonoBehaviour
{
    private Button quitGameButton;
    
    void Start()
    {
        quitGameButton = GetComponent<Button>();
        quitGameButton.onClick.AddListener(QuitTheGame);
    }

    private void QuitTheGame()
    {
        Application.Quit();
    }
}
