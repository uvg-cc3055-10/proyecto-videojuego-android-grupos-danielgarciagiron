using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    private Animator settingsMenu;
    private Animator exitMenu;

    public RectTransform settings;
    public RectTransform exit;


    // Use this for initialization
    void Start () {
		settingsMenu = settings.GetComponent<Animator>();
        exitMenu = exit.GetComponent<Animator>();
	}
	
    public void ConquestOnClick()
    {
        SceneManager.LoadScene("CampaignMenu");
    }

    public void CustomizetOnClick()
    {
        SceneManager.LoadScene("CustomizeMenu");
    }

    public void exitOnClick()
    {
        exitMenu.SetBool("state", true);
    }

    public void settingsOnClick()
    {
        settingsMenu.SetBool("state", true);
    }

    public void backOnClick()
    {
        settingsMenu.SetBool("state", false);
    }

    public void noOnClick()
    {
        exitMenu.SetBool("state", false);
    }

    public void yesOnClick()
    {
        Application.Quit();
    }
}
