using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Turret : MonoBehaviour
{
    [Header("Canvas")]
    public CanvasManager manager;

    [Header("Do Not Change")]
    private Transform target;
    private Transform target2;
    public GameObject Fire;

    public Transform PartToRotate;
    public string enemyTag = "Enemy";
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform firePoint2;
    public int TurretLevel = 0;
    public ParticleSystem impactFXLazer;

    [Header("Attributes")]
    public float range = 25f;
    public float turnSpeed = 10f;
    public float fireRate = 1f;
    public int damage = 50;
    private float fireCountdown = 0f;

    [Header("Lazer Stuff")]
    public bool LazersEnabled = false;
    public LineRenderer lineRender;
    EnemyScript enemyScriptLazer;
    private float LazerDmgCooldown = 0f;

    [Header("Tutorial Reference")]
    public Tutorial tut;

    [Header("Audio")]
    public AudioClip Fire1;
    public AudioSource Fire2;
    private bool Fire2Playing = false;
    public AudioClip UpgradeSuccess;

    // Start is called before the first frame update
    void Start()
    {
        Fire2Playing = false;
        PartToRotate.rotation = Quaternion.Euler(30f, 0f, 0f);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Pause.isPaused) return;
        if (target2 == null)
        {
            if (Fire2Playing)
            {
                Fire2.Stop();
                Fire2Playing = false;
            }
            lineRender.enabled = false;
            impactFXLazer.Stop();
        }
        if (target == null) return;

        LockOnTarget();

        if (LazersEnabled && target2 != null)
        {
            if (!Fire2Playing)
            {
                Fire2.Play();
                Fire2Playing = true;
            }
            LazerBeam();
        }
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    //Rotate turret to the current target
    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(PartToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        PartToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
    }

    //Shoot a bullet at the enemy
    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            AudioSource.PlayClipAtPoint(Fire1, transform.position, AudioManager.ReturnVolume() / 4);
            bullet.Seek(target, damage);
        }
    }

    //Itteratively find a new, optimal target
    void UpdateTarget()
    {
        if (TurretLevel == 0) return;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float ShortestDistance = Mathf.Infinity;
        float SecondShortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        GameObject lazerEnemy = null;
        
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            EnemyScript e = enemy.GetComponent<EnemyScript>();
            if (distanceToEnemy < ShortestDistance && e.Enemyhealth > 0)
            {
                ShortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (enemies.Length > 1)
        {
            foreach (GameObject enemy in enemies)
            {
                EnemyScript e = enemy.GetComponent<EnemyScript>();
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (enemy != nearestEnemy && e.Enemyhealth > 0 && distanceToEnemy < SecondShortestDistance)
                {
                    SecondShortestDistance = distanceToEnemy;
                    lazerEnemy = enemy;
                }
            }
        }
        if (nearestEnemy != null)
        {
            if (nearestEnemy != null && ShortestDistance <= range)
            {
                target = nearestEnemy.transform;
            }
            else target = null;
        }
        if (lazerEnemy != null)
        {
            if (lazerEnemy != null && SecondShortestDistance <= range)
            {
                target2 = lazerEnemy.transform;
                enemyScriptLazer = lazerEnemy.GetComponent<EnemyScript>();
            }
            else { 
                target2 = null;
                enemyScriptLazer = null;
            }
        }
    }


    //Editor script for visualizing range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    //Second firepoint shoots a lazer beam between the firepoint/enemy prefab
    void LazerBeam()
    {
        if (!lineRender.enabled) { 
            lineRender.enabled = true;
            impactFXLazer.Play();
        }
        lineRender.SetPosition(0, firePoint2.position);
        lineRender.SetPosition(1, target2.position + (Vector3.up * 2));

        impactFXLazer.transform.position = target2.position + (Vector3.up * 2);
        if (LazerDmgCooldown <= 0f)
        {
            enemyScriptLazer.DamageTake(40);
            LazerDmgCooldown = 0.5f;
        }
        else LazerDmgCooldown -= Time.deltaTime;
    }

    //Hardcoded :( Upgrade system, extendable if required
    public void Upgrade()
    {
        switch (TurretLevel)
        {
            case 0:
                if (PlayerStats.Wood >= 10 && PlayerStats.Scrap >= 10)
                {
                    if (Tutorial.InTutorial) tut.TutorialDone("UPGRADE");
                    AudioSource.PlayClipAtPoint(UpgradeSuccess, transform.position, AudioManager.ReturnVolume() / 4);
                    PlayerStats.Wood -= 10;
                    PlayerStats.Scrap -= 10;
                    damage = 10;
                    range = 25;
                    turnSpeed = 10f;
                    fireRate = 1f;
                    manager.CloseTurretUpgrade();
                    PartToRotate.rotation = Quaternion.Euler(0f, 0f, 0f);
                    Object.Destroy(Fire);
                    TurretLevel = 1;
                }
                break;
            case 1:
                if (PlayerStats.Wood >= 20 && PlayerStats.Scrap >= 20)
                {
                    AudioSource.PlayClipAtPoint(UpgradeSuccess, transform.position, AudioManager.ReturnVolume() / 4);
                    PlayerStats.Wood -= 20;
                    PlayerStats.Scrap -= 20;
                    damage += 15;
                    range += 10;
                    turnSpeed += 5f;
                    fireRate += 1f;
                    manager.CloseTurretUpgrade();
                    TurretLevel += 1;
                }
                break;
            case 2:
                if (PlayerStats.Wood >= 20 && PlayerStats.Scrap >= 20 && PlayerStats.Crystal >= 1)
                {
                    AudioSource.PlayClipAtPoint(UpgradeSuccess, transform.position, AudioManager.ReturnVolume() / 4);
                    LazersEnabled = true;
                    PlayerStats.Wood -= 20;
                    PlayerStats.Scrap -= 20;
                    PlayerStats.Crystal -= 1;
                    damage += 25;
                    range += 10;
                    turnSpeed += 5f;
                    fireRate += 1f;
                    manager.CloseTurretUpgrade();
                    TurretLevel += 1;
                }
                break;
        }
    }
}
