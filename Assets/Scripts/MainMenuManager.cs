// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos


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
	
    //Lanza el menu de seleccion de nivel
    public void ConquestOnClick()
    {
        SceneManager.LoadScene("CampaignMenu");
    }

    //Lanza el menu para personalizar al personaje. No lo implemente, solo lo deje para futuras actualizaciones, el boton esta desactivado en el editor.
    public void CustomizetOnClick()
    {
        SceneManager.LoadScene("CustomizeMenu");
    }

    //sale del juego.
    public void exitOnClick()
    {
        exitMenu.SetBool("state", true);
    }

    //igual que personalizar, no esta implementado.
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
