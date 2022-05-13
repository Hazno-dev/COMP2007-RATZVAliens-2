using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.Audio;

public class WaveSpawner : MonoBehaviour
{
    [Header("Do Not Change")]
    public Transform enemyPrefab;
    public Transform SpawnPoint1;
    public Transform SpawnPoint2;
    public Transform SpawnPoint3;

    [Header("Visuals")]
    public VisualEffect SP1FX;
    public VisualEffect SP2FX;
    public VisualEffect SP3FX;
    public VisualEffect SPMEGAFX;

    public float timeBetweenWaves = 10f;
    private float countdown = 40f;

    public Text waveCountdownText;

    public static int waveNumber;

    public CrystalInteraction crystal;

    [Header("Audio")]
    public AudioClip Lightning;
    public AudioSource Sirens;
    // Start is called before the first frame update
    void Start()
    {
        waveNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Tutorial.InTutorial) return;
        if (GameManager2.GameOver) return;
        if (Pause.isPaused) return;

        if (countdown <= 0f)
        {
            //Start a new wave
            Sirens.Play();
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves + (waveNumber * 0.5f) ;

        }

        waveCountdownText.text = Mathf.Ceil(countdown).ToString();
        countdown -= Time.deltaTime;
    }

    //Itteratively create a new enemy for the current wave count
    IEnumerator SpawnWave()
    {
        Gun.Damage = 10 + (10 * waveNumber);
        waveNumber++;
        ResetLoot();
        for (int i = 0; i < waveNumber; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
        Debug.Log("Wave Incoming!");

    }

    void SpawnEnemy()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                Transform temp = Instantiate(enemyPrefab, SpawnPoint1.position, SpawnPoint1.rotation);
                EnemyScript e = temp.GetComponent<EnemyScript>();
                e.NewHealth(100 + (10 * waveNumber));
                Debug.Log(e.Enemyhealth);
                SP1FX.Play();
                SPMEGAFX.Play();
                AudioSource.PlayClipAtPoint(Lightning, SpawnPoint1.position, AudioManager.ReturnVolume() / 4);
                break;
            case 1:
                Transform temp2 = Instantiate(enemyPrefab, SpawnPoint2.position, SpawnPoint2.rotation);
                EnemyScript e2 = temp2.GetComponent<EnemyScript>();
                e2.NewHealth(100 + (10 * waveNumber));
                Debug.Log(e2.Enemyhealth);
                SP2FX.Play();
                SPMEGAFX.Play();
                AudioSource.PlayClipAtPoint(Lightning, SpawnPoint2.position, AudioManager.ReturnVolume() / 4);
                break;
            case 2:
                Transform temp3 = Instantiate(enemyPrefab, SpawnPoint3.position, SpawnPoint3.rotation);
                EnemyScript e3 = temp3.GetComponent<EnemyScript>();
                e3.NewHealth(100 + (10 * waveNumber));
                SP3FX.Play();
                SPMEGAFX.Play();
                AudioSource.PlayClipAtPoint(Lightning, SpawnPoint3.position, AudioManager.ReturnVolume() / 4);
                break;
        }
    }
    
    //Reset the collection points at every interaction zone
    void ResetLoot()
    {
        GameObject[] Trashs = GameObject.FindGameObjectsWithTag("Trash");
        foreach (GameObject garbo in Trashs)
        {
            TrashInteraction interactor = garbo.GetComponent<TrashInteraction>();
            interactor.CanSearch = true;
            for (int i = 0; i < garbo.transform.childCount; i++)
            {
                garbo.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        crystal.CanSearch = true;
        for (int i = 0; i < crystal.transform.childCount; i++)
        {
            crystal.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
