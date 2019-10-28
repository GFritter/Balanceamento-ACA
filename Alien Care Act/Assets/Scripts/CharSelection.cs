using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelection : MonoBehaviour
{
    public Button engineer_selection_button;
    public Button scrapper_selection_button;

    string selected_char;
    // Start is called before the first frame update
    void Start()
    {
        engineer_selection_button.onClick.AddListener(engineerSelected);
        scrapper_selection_button.onClick.AddListener(scrapperSelected);

    }

    public string getSelectedChar()
    {
        return selected_char;
    }

    void engineerSelected()
    {
        engineer_selection_button.transform.GetChild(0).gameObject.SetActive(true);
        scrapper_selection_button.transform.GetChild(0).gameObject.SetActive(false);

        selected_char = "Engineer";
    }

    void scrapperSelected()
    {
        scrapper_selection_button.transform.GetChild(0).gameObject.SetActive(true);
        engineer_selection_button.transform.GetChild(0).gameObject.SetActive(false);

        selected_char = "Scrapper";
    }

}
