using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [Header("Do Not Change")]
    public static bool isPaused;
    public GameObject PauseUI;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) && !GameManager2.GameOver)
        {
            Toggle();        

        }
    }

    //If the pause menu is enabled, slows the enemies down to 0, Also sets ispaused to true which is referenced in other objects as an if statement
    public void Toggle()
    {
        if (!isPaused)
        {
            isPaused = true;
            PlayerStats.Interacting = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            PauseUI.SetActive(true);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                EnemyScript e = enemy.GetComponent<EnemyScript>();
                e.speed = 0;
                Animator a = enemy.GetComponent<Animator>();
                a.speed = 0;
            }
        }
        else
        {
            isPaused = false;
            PlayerStats.Interacting = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            PauseUI.SetActive(false);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                EnemyScript e = enemy.GetComponent<EnemyScript>();
                e.speed = 5;
                Animator a = enemy.GetComponent<Animator>();
                a.speed = 1;
            }
        }
    }
}
