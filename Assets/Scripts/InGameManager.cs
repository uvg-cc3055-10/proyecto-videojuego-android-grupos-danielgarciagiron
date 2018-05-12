// Universidad del Valle de Guatemala
// Daniel Garcia, 14152
// Programacion de plataformas moviles y juegos

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class InGameManager : MonoBehaviour {

    //creates the reference to the main character life script
    public GameObject player;
    private LifeSystem playerScript;

    //Variables to hold the UI elements
    public GameObject pausePanel;
    public GameObject infoPanel;
    public Image healthBar;
    public Text healthDisplay;
    public Image magicaBar;
    public Text magicaDisplay;
    public Text roundDisplay;
    public Text endResult;

    //variables to display in the HUD about health and magicka
    private float maxHealth = 100;
    private float currentHealth = 100;
    private float maxMagica = 100;
    private float currentMagica = 100;
    private int currentWaveNumber;
    private int roundNumber;

    // Use this for initialization
    void Start()
    {
        //makes the link to the life system script
        playerScript = (LifeSystem)player.GetComponent(typeof(LifeSystem));

        //removes the pause panel from the screen
        pausePanel.SetActive(false);
        infoPanel.SetActive(false);
        updateBars();
    }

    private void FixedUpdate()
    {
        //always runs the update bars method
        updateBars();
    }

    //displays the correct value in the HUD
    public void updateBars()
    {
        //gives the value from the linked script into this local variable
        currentHealth = playerScript.currentLife;
        float healthRatio = currentHealth / maxHealth;
        healthBar.rectTransform.localScale = new Vector3(healthRatio, 1, 1);
        healthDisplay.text = (healthRatio * 100f).ToString("0")+"%";

        //gives the value from the linked script into this local variable
        currentMagica = playerScript.currentMagica;
        float magicaRatio = currentMagica / maxMagica;
        magicaBar.rectTransform.localScale = new Vector3(magicaRatio, 1, 1);
        magicaDisplay.text = (magicaRatio * 100f).ToString("0") + "%";
    }

    //pauses the game
    public void pauseOnClick()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
        
    }

    //resumes the game
    public void unpauseOnClick()
    {
        pausePanel.SetActive(false);
        Time.timeScale=1.0f;
    }

    //exits to main menu, sets the time to normal again
   public void exitOnClick()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void menuOnClick()
    {
        SceneManager.LoadScene("CampaignMenu");
    }

    public void RetryOnClick()
    {
        int Scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(Scene, LoadSceneMode.Single);
    }

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

    void RoundDisplay(int info)
    {
        roundNumber++;
        roundDisplay.text = ("Round No." + roundNumber);
    }

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