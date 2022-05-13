using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    public GameObject InteractionText;
    private Turret selectedturret;

    public Text ScrapCountT;
    public Text WoodCountT;
    public Text CrystalCountT;
    public Scrollbar HealthHud;

    // Start is called before the first frame update
    void Start()
    {
        selectedturret = null;
    }

    // Update is called once per frame
    void Update()
    {
        //DONT USE THSI PLS FIX LATER
        ScrapCountT.text = PlayerStats.Scrap.ToString();
        WoodCountT.text = PlayerStats.Wood.ToString();
        CrystalCountT.text = PlayerStats.Crystal.ToString();
        HealthHud.size = Mathf.InverseLerp(0, PlayerStats.MaxLives, PlayerStats.Lives);
    }

    public void CloseTurretUpgrade()
    {
        InteractionText.SetActive(false);
        PlayerStats.Interacting = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        selectedturret = null;
    }

    public void SelectedTurret(Turret turretselected)
    {
        selectedturret = turretselected;
    }
    public void UpgradeSelected()
    {
        if (selectedturret == null) return;
        selectedturret.Upgrade();
    }
}
