// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CampaignManager : MonoBehaviour {

    public RectTransform back;
    public CanvasGroup playButton;
    public Text infoDisplay;
    private int ID = 0;
    private string levelToLoad;
    public Button [] buttons;

    // Use this for initialization
    void Start () {
        playButton.alpha = 0.0f;
        
	}
	
    //regresa al menu principal.
    public void BackOnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void messageReceived(int levelID)
    {
        playButton.alpha = 1.0f;
        ID = levelID;
        informationDisplay();
        
        //Debug.Log(levelID);
    }

    //Lista increiblemente larga de los niveles disponibles (solo existe 1 por el momento). tiene la informacion del nivel en un string el cual actualiza la pantalla al presionar 
    //el boton del nivel deseado. 
    private void informationDisplay()
    {
        switch (ID)
        {
            case 0:
                //solo este nivel existe, cuando se presiona, cambia el texto en la pantalla con la siguiente informacion.
                infoDisplay.text = "Training Grounds, test your skills freely!";
                levelToLoad = "TrainingGrounds";
                break;
            case 1:
                infoDisplay.text = "";
                levelToLoad = "Area1_Tower";
                break;
            case 2:
                infoDisplay.text = "";
                levelToLoad = "Area1_Castle";
                break;
            case 3:
                infoDisplay.text = "";
                levelToLoad = "Area2_Tower";
                break;
            case 4:
                infoDisplay.text = "";
                levelToLoad = "Area2_Camp";
                break;
            case 5:
                infoDisplay.text = "";
                levelToLoad = "Area2_Castle";
                break;
            case 6:
                infoDisplay.text = "";
                levelToLoad = "Area3_Tower";
                break;
            case 7:
                infoDisplay.text = "";
                levelToLoad = "Area3_Camp";
                break;
            case 8:
                infoDisplay.text = "";
                levelToLoad = "Area3_Castle";
                break;
            case 9:
                infoDisplay.text = "";
                levelToLoad = "Area4_Tower";
                break;
            case 10:
                infoDisplay.text = "";
                levelToLoad = "Area4_Camp";
                break;
            case 11:
                infoDisplay.text = "";
                levelToLoad = "Area4_Castle";
                break;
            case 12:
                infoDisplay.text = "";
                levelToLoad = "Area5_Tower";
                break;
            case 13:
                infoDisplay.text = "";
                levelToLoad = "Area5_Camp";
                break;
            case 14:
                infoDisplay.text = "";
                levelToLoad = "Area5_Castle";
                break;
            case 15:
                infoDisplay.text = "";
                levelToLoad = "Area6_Tower";
                break;
            case 16:
                infoDisplay.text = "";
                levelToLoad = "Area6_Camp";
                break;
            case 17:
                infoDisplay.text = "";
                levelToLoad = "Area6_Castle";
                break;
            case 18:
                infoDisplay.text = "";
                levelToLoad = "Area7_Tower";
                break;
            case 19:
                infoDisplay.text = "";
                levelToLoad = "Area7_Camp";
                break;
            case 20:
                infoDisplay.text = "";
                levelToLoad = "Area7_Castle";
                break;
            case 21:
                infoDisplay.text = "";
                levelToLoad = "Area8_Tower";
                break;
            case 22:
                infoDisplay.text = "";
                levelToLoad = "Area8_Camp";
                break;
            case 23:
                infoDisplay.text = "";
                levelToLoad = "Area8_Castle";
                break;
            case 24:
                infoDisplay.text = "";
                levelToLoad = "Area9_Tower";
                break;
            case 25:
                infoDisplay.text = "";
                levelToLoad = "Area9_Camp";
                break;
            case 26:
                infoDisplay.text = "";
                levelToLoad = "Area9_Castle";
                break;
            case 27:
                infoDisplay.text = "";
                levelToLoad = "Area10_Tower";
                break;
            case 28:
                infoDisplay.text = "";
                levelToLoad = "Area10_Camp";
                break;
            case 29:
                infoDisplay.text = "";
                levelToLoad = "Area10_Castle";
                break;
            case 30:
                infoDisplay.text = "";
                levelToLoad = "Area11_Tower";
                break;
            case 31:
                infoDisplay.text = "";
                levelToLoad = "Area11_Camp";
                break;
            case 32:
                infoDisplay.text = "";
                levelToLoad = "Area11_Castle";
                break;
            default:
                break;
        }
    }

    //Carga el nivel seleccionado
    public void LoadOnClick()
    {
        SceneManager.LoadScene(levelToLoad);
    }


}
