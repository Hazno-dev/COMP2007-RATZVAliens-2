using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Gun : MonoBehaviour
{
    [Header("Attributes")]
    public static int Damage = 10;
    public float range = 100f;
    public static float fireRate = 2f;
    public static int Ammo = 8;
    public Text AmmoUI;

    [Header("Camera")]
    public Transform FirePoint;
    public Camera cam;

    [Header("Visuals")]
    public VisualEffect shootFXGun;
    public VisualEffect GunCooldownFX;
    public GameObject ImpactFX;
    private float emissiveIntensity;
    private Color EmissiveColour;
    public GameObject GunPartEmiss1;

    [Header("Audio")]
    public AudioSource FireSound;
    public AudioSource Reload;
    public AudioSource ReloadEnd;
    public AudioClip EndPointSound;

    public static float nextTimeToFire = 0f;
    // Start is called before the first frame update

    private void Start()
    {
        if (FireSound != null) InvokeRepeating("UpdateVolume", 0f, 0.2f);
        emissiveIntensity = 0;
        EmissiveColour = GunPartEmiss1.GetComponent<MeshRenderer>().material.GetColor("_EmissiveColor");

        GunCooldownFX.enabled = false;
    }

    //Shoot gun
    public void Shoot()
    {
        FireSound.Play();
        AmmoUI.text = Ammo.ToString();
        RaycastHit hitinfo;
        Debug.Log("ran");
        shootFXGun.Play();
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitinfo, range))
        {
            if (hitinfo.transform.tag == "Enemy")
            {
                //Debug.Log("HitEnemy");
                EnemyScript e = hitinfo.transform.GetComponent<EnemyScript>();
                e.DamageTake(Damage);
            }
            GameObject ImpactTemp = Instantiate(ImpactFX, hitinfo.point, Quaternion.LookRotation(hitinfo.normal) * Quaternion.Euler(90,0,0));
            AudioSource.PlayClipAtPoint(EndPointSound, hitinfo.point, AudioManager.ReturnVolume() / 4);
            //ImpactTemp.transform.localPosition += Vector3.up;
            Destroy(ImpactTemp, 2f);
        }
        if (Gun.Ammo == 0)
        {
            StartCoroutine(ResetAmmo());
        }
    }

    //Waits a few seconds before resetting the players ammo
    IEnumerator ResetAmmo()
    {
        Reload.Play();
        emissiveIntensity = -10f;
        GunPartEmiss1.GetComponent<Renderer>().material.SetColor("_EmissiveColor", EmissiveColour * emissiveIntensity);
        GunCooldownFX.enabled = true;
        yield return new WaitForSeconds(3f);
        ReloadEnd.Play();
        Ammo = 8;
        GunCooldownFX.enabled = false;
        AmmoUI.text = Ammo.ToString();
        emissiveIntensity = 0f;
        GunPartEmiss1.GetComponent<Renderer>().material.SetColor("_EmissiveColor", EmissiveColour);
    }
    void UpdateVolume()
    {
        FireSound.volume = AudioManager.ReturnVolume() / 8;
        Reload.volume = AudioManager.ReturnVolume() / 8;
        ReloadEnd.volume = AudioManager.ReturnVolume() / 8;
    }
}
