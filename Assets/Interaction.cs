using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [Header("Do Not Change")]
    public CanvasManager manager;
    public Text EInteractText;
    public GameObject InteractionText;
    private bool EPressable = false;

    public Text CurrentDamage;
    public Text CurrentRange;
    public Text CurrentTurnSpeed;
    public Text CurrentFireRate;

    public Text RequiredWood;
    public Text RequiredScrap;
    public Text RequiredCrystal;

    public Text BonusDamage;
    public Text BonusRange;
    public Text BonusTurnSpeed;
    public Text BonusFireRate;
    public Text BonusLazers;

    public Text LazerEnabledText;
    public GameObject upgradeButton;

    Turret parentTurret;

    //Check if player is inside the turret upgrade zone
    private void OnTriggerEnter(Collider other)
    {
        EInteractText.enabled = true;
        EPressable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        EInteractText.enabled = false;
        EPressable = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        parentTurret = gameObject.GetComponentInParent<Turret>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && EPressable == true)
        {
            EInteractText.enabled = false;
            EPressable = false;
            InteractionText.SetActive(true);
            PlayerStats.Interacting = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            manager.SelectedTurret(parentTurret);

            upgradeButton.SetActive(true);
            BonusDamage.enabled = true;
            BonusRange.enabled = true;
            BonusTurnSpeed.enabled = true;
            BonusFireRate.enabled = true;
            BonusLazers.enabled = false;
            LazerEnabledText.enabled = false;

            //Current Stats
            CurrentDamage.text = parentTurret.damage.ToString();
            CurrentRange.text = parentTurret.range.ToString();
            CurrentTurnSpeed.text = parentTurret.turnSpeed.ToString();
            CurrentFireRate.text = parentTurret.fireRate.ToString();

            //CurrentItems VS Required
            //Bonus Stats
            switch (parentTurret.TurretLevel)
            {
                case 0:
                    RequiredScrap.text = PlayerStats.Scrap.ToString() + "/" + "10";
                    if (PlayerStats.Scrap < 10) RequiredScrap.color = Color.red;
                    else RequiredScrap.color = Color.green;

                    RequiredWood.text = PlayerStats.Wood.ToString() + "/" + "10";
                    if (PlayerStats.Wood < 10) RequiredWood.color = Color.red;
                    else RequiredWood.color = Color.green;

                    RequiredCrystal.text = PlayerStats.Crystal.ToString() + "/" + "0";
                    if (PlayerStats.Crystal < 0) RequiredCrystal.color = Color.red;
                    else RequiredCrystal.color = Color.green;

                    BonusDamage.text = "+10";
                    BonusRange.text = "+25";
                    BonusTurnSpeed.text = "+10";
                    BonusFireRate.text = "+1";
                    break;
                case 1:
                    RequiredScrap.text = PlayerStats.Scrap.ToString() + "/" + "20";
                    if (PlayerStats.Scrap < 20) RequiredScrap.color = Color.red;
                    else RequiredScrap.color = Color.green;

                    RequiredWood.text = PlayerStats.Wood.ToString() + "/" + "20";
                    if (PlayerStats.Wood < 20) RequiredWood.color = Color.red;
                    else RequiredWood.color = Color.green;

                    RequiredCrystal.text = PlayerStats.Crystal.ToString() + "/" + "0";
                    if (PlayerStats.Crystal < 0) RequiredCrystal.color = Color.red;
                    else RequiredCrystal.color = Color.green;

                    BonusDamage.text = "+15";
                    BonusRange.text = "+10";
                    BonusTurnSpeed.text = "+5";
                    BonusFireRate.text = "+1";
                    break;
                case 2:
                    RequiredScrap.text = PlayerStats.Scrap.ToString() + "/" + "20";
                    if (PlayerStats.Scrap < 20) RequiredScrap.color = Color.red;
                    else RequiredScrap.color = Color.green;

                    RequiredWood.text = PlayerStats.Wood.ToString() + "/" + "20";
                    if (PlayerStats.Wood < 20) RequiredWood.color = Color.red;
                    else RequiredWood.color = Color.green;

                    RequiredCrystal.text = PlayerStats.Crystal.ToString() + "/" + "1";
                    if (PlayerStats.Crystal < 1) RequiredCrystal.color = Color.red;
                    else RequiredCrystal.color = Color.green;

                    BonusDamage.text = "+25";
                    BonusRange.text = "+10";
                    BonusTurnSpeed.text = "+5";
                    BonusFireRate.text = "+1";
                    BonusLazers.enabled = true;
                    break;
                case 3:
                    RequiredScrap.text = PlayerStats.Scrap.ToString() + "/" + "0";
                    if (PlayerStats.Scrap < 0) RequiredScrap.color = Color.red;
                    else RequiredScrap.color = Color.green;

                    RequiredWood.text = PlayerStats.Wood.ToString() + "/" + "0";
                    if (PlayerStats.Wood < 0) RequiredWood.color = Color.red;
                    else RequiredWood.color = Color.green;

                    RequiredCrystal.text = PlayerStats.Crystal.ToString() + "/" + "0";
                    if (PlayerStats.Crystal < 0) RequiredCrystal.color = Color.red;
                    else RequiredCrystal.color = Color.green;

                    upgradeButton.SetActive(false);
                    BonusDamage.enabled = false;
                    BonusRange.enabled = false;
                    BonusTurnSpeed.enabled = false;
                    BonusFireRate.enabled = false;
                    LazerEnabledText.enabled = true;
                    break;
            }

        }
    }
}
