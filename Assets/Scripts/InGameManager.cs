// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class InGameManager : MonoBehaviour {


    public GameObject player;
    private LifeSystem playerScript;


    public GameObject pausePanel;
    public GameObject infoPanel;
    public Image healthBar;
    public Text healthDisplay;
    public Image magicaBar;
    public Text magicaDisplay;
    public Text roundDisplay;
    public Text endResult;

    private float maxHealth = 100;
    private float currentHealth = 100;
    private float maxMagica = 100;
    private float currentMagica = 100;
    private int currentWaveNumber;
    private int roundNumber;


    void Start()
    {
        //referencia al script de life system
        playerScript = (LifeSystem)player.GetComponent(typeof(LifeSystem));

        //esconde los paneles de pausa.
        pausePanel.SetActive(false);
        infoPanel.SetActive(false);
        updateBars();
    }

    private void FixedUpdate()
    {
        //siempre corre este metodo, sirve para poner la info de vida, energia y ronda en pantalla.
        updateBars();
    }


    public void updateBars()
    {
        //agarra el valor de vida y lo convierte para poder desplegarlo en pantalla.
        currentHealth = playerScript.currentLife;
        float healthRatio = currentHealth / maxHealth;
        healthBar.rectTransform.localScale = new Vector3(healthRatio, 1, 1);
        healthDisplay.text = (healthRatio * 100f).ToString("0")+"%";

        //Lo mismo pero con el valor de energia.
        currentMagica = playerScript.currentMagica;
        float magicaRatio = currentMagica / maxMagica;
        magicaBar.rectTransform.localScale = new Vector3(magicaRatio, 1, 1);
        magicaDisplay.text = (magicaRatio * 100f).ToString("0") + "%";
    }

    //pausa
    public void pauseOnClick()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
        
    }

    //quita la pausa
    public void unpauseOnClick()
    {
        pausePanel.SetActive(false);
        Time.timeScale=1.0f;
    }

    //regresa al menu principal
   public void exitOnClick()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void menuOnClick()
    {
        SceneManager.LoadScene("CampaignMenu");
    }

    //vuelve a cargar el nivel.
    public void RetryOnClick()
    {
        int Scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(Scene, LoadSceneMode.Single);
    }

    //Listeners para pasar datos entre scripts.
    void OnEnable()
    {
        EventManager.startListening("RoundDisplay", RoundDisplay);
        EventManager.startListening("EndGame", EndGame);
    }

    void OnDisable()
    {
        EventManager.stopListening("RoundDisplay", RoundDisplay);
        EventManager.stopListening("EndGame", EndGame);
    }

    //actualiza la info del round 
    void RoundDisplay(int info)
    {
        roundNumber++;
        roundDisplay.text = ("Round No." + roundNumber);
    }

    //finaliza el juego, si se gana muestra la pantalla de zona conquistada, si se pierde menciona que se perdio.
    void EndGame(int info)
    {
        infoPanel.SetActive(true);
        if (info == 1)
        {
            infoPanel.GetComponent<Image>().color = new Color32(255, 255, 255, 220);
            endResult.color = Color.black;
            endResult.text = ("Zone Conquered!");
        }
        if(info == 2)
        {
            infoPanel.GetComponent<Image>().color = new Color32(10, 10, 10, 220);
            endResult.color = Color.white;
            endResult.text = ("You Fail!");
        }
    }


}