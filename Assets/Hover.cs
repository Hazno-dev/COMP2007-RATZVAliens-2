using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    private float hoverHeight;
    public float hoverRange;
    public float hoverSpeed;

    private void Start()
    {
        hoverHeight = transform.position.y;
        hoverRange = 1;
        hoverSpeed = 2f;
    }
    // Start is called before the first frame update
    void Update()
    {
        this.transform.position = new Vector3(transform.position.x, hoverHeight + Mathf.Cos(Time.time * hoverSpeed) * hoverRange, transform.position.z);
    }
}
