using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager2 : MonoBehaviour
{
    [Header("Do Not Change")]
    public static bool GameOver;

    public GameObject GameOverUI;
    public GameObject PauseMenu;
    public Text WavesSurvived;

    [Header("Audio")]
    public AudioSource GameOverMusic;
    public AudioSource Intro;

    // Start is called before the first frame update
    void Start()
    {
        GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver) return;
        //If wallHP is less than 0, GGWP
        if (PlayerStats.Lives <= 0)
        {
            GGWP();
            GameOverMusic.Play();
            Intro.Play();
        }
    }
    
    //Game over function
    void GGWP()
    {
        PlayerStats.Interacting = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameOver = true;
        GameOverUI.SetActive(true);
        PauseMenu.SetActive(false);
        WavesSurvived.text = WaveSpawner.waveNumber.ToString();
    }

    //Restart game
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Send user back to main menu
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
