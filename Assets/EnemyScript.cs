using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class EnemyScript : MonoBehaviour
{
    [Header("Attributes")]
    public float speed = 5f;
    private Transform target;
    private float attackCooldown = 2.633f;
    private float currentCooldown = 1f;

    [Header("Do Not Change")]
    public Image healthbar;

    private int healthstart;
    public int Enemyhealth;

    private MovementState state;
    private enum MovementState { 
        walking,
        running,
        attacking,
        dying
    }
    Animator anim;

    [Header("Audio")]
    public AudioClip DeathCry;
    public AudioClip HitWall;

    // Start is called before the first frame update
    void Start()
    {
        healthstart = 100;
        Enemyhealth = healthstart;
        target = Waypoint.points[0];
        anim = GetComponent<Animator>();
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        anim.speed = speed;



        if (speed > 5)
        {
            state = MovementState.running;
            anim.speed = 1f;
            anim.SetBool("Running", true);
        }
        else state = MovementState.walking;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != MovementState.dying && state != MovementState.attacking)
        {
            //Constantly change the enemys direction so the animator doesnt break
            if (state == MovementState.running)
            {
                Vector3 dir = target.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotation = lookRotation.eulerAngles;
                transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            }
            //Vector3 test = dir.normalized;
            //transform.Translate(test.x * speed * Time.deltaTime, 0f, test.z * speed * Time.deltaTime, Space.World);
            if (state == MovementState.walking || state == MovementState.running)
            {
                if (Vector3.Distance(transform.position, target.position) <= 0.4f)
                {
                    anim.speed = 1f;
                    target = Waypoint.points[1];
                    Vector3 dir = target.position - transform.position;
                    Quaternion lookRotation = Quaternion.LookRotation(dir);
                    Vector3 rotation = lookRotation.eulerAngles;
                    transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
                    anim.SetBool("AtDoor", true);
                    state = MovementState.attacking;
                }
            }
        }
        //Attacking the wall
        if (state == MovementState.attacking)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0f)
            {
                currentCooldown = attackCooldown;
                PlayerStats.Lives -= 1;
                if (!GameManager2.GameOver) AudioSource.PlayClipAtPoint(HitWall, transform.position, AudioManager.ReturnVolume() / 2);
            }
        }
    }

    //Called from other scripts, damages the enemy
    public void DamageTake(int amount)
    {
        Enemyhealth -= amount;
        healthbar.fillAmount = Mathf.InverseLerp(0, healthstart, Enemyhealth);
        if (Enemyhealth <= 0 && state != MovementState.dying)
        {
            Death();
        }
    }

    //Dead :(
    private void Death()
    {
        state = MovementState.dying;
        StartCoroutine(Dying());
    }
    IEnumerator Dying()
    {
        AudioSource.PlayClipAtPoint(DeathCry, transform.position, AudioManager.ReturnVolume() / 4);
        anim.SetBool("Dying", true);

        //2 sec wait
        yield return new WaitForSeconds(2);

        //die
        Destroy(gameObject);
    }

    //Sets up the enemys new health, each wave this slowly increases. Called from wave spawner and will increase the enemy health per wave
    public void NewHealth(int hp)
    {
        StartCoroutine(NewHPDelayed(hp));
    }
    
    IEnumerator NewHPDelayed(int hp)
    {
        yield return new WaitForSeconds(0.2f);
        healthstart = hp;
        Enemyhealth = hp;
    }
}
