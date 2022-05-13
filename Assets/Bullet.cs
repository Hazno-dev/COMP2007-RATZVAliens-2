using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Do Not Change")]
    private Transform target;
    public GameObject impactEffect;

    public int damage = 0;
    public float Speed = 70f;

    //Called from turret when this is instantiated to find the enemy
    public void Seek(Transform _target, int turretDMG)
    {
        target = _target;
        damage = turretDMG;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = Speed * Time.deltaTime;
        
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget(target);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    //The bullet has reached the enemy and will now destroy itself and damage the enemy
    void HitTarget(Transform enemy)
    {
        EnemyScript e = enemy.GetComponent<EnemyScript>();
        if (e != null)
        {
            if (e.Enemyhealth > 0) e.DamageTake(damage);
        }


        GameObject effectins = (GameObject)Instantiate(impactEffect, transform.position + new Vector3(0f, 2f, 0f), transform.rotation);
        Destroy(effectins, 2f);
        
        Destroy(gameObject);
        return;
    }
}
