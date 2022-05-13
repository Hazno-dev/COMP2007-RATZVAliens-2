using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [Header("Do Not Change")]
    public static bool InTutorial;

    public Text NextWaveText;
    public Text waveCountdownText;

    public GameObject TutorialPlane;

    [Header("UI References")]
    public Text WASDMove;
    public Text CAMMove;
    public Text SHOOTgun;
    public Text collectE;
    public Text upgradeE;
    public Text JUMPmove;

    private bool WASD;
    private bool CAM;
    private bool SHOOT;
    private bool COLLECT;
    private bool UPGRADE;
    private bool JUMPED;

    // Start is called before the first frame update
    void Start()
    {
        InTutorial = true;
        NextWaveText.enabled = false;
        waveCountdownText.enabled = false;
        TutorialPlane.SetActive(true);
    }

    //Switch called from other objects when an action is taken that the tutorial requires
    public void TutorialDone(string completed)
    {
        switch (completed)
        {
            case "WASD":
                WASD = true;
                WASDMove.color = Color.green;
                break;
            case "CAM":
                CAM = true;
                CAMMove.color = Color.green;
                break;
            case "SHOOT":
                SHOOT = true;
                SHOOTgun.color = Color.green;
                break;
            case "COLLECT":
                COLLECT = true;
                collectE.color = Color.green;
                break;
            case "UPGRADE":
                UPGRADE = true;
                upgradeE.color = Color.green;
                break;
            case "JUMP":
                JUMPED = true;
                JUMPmove.color = Color.green;
                break;
        }
        if (WASD && CAM && SHOOT && COLLECT && UPGRADE && JUMPED)
        {
            InTutorial = false;
            NextWaveText.enabled = true;
            waveCountdownText.enabled = true;
            TutorialPlane.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
