using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private GameObject cam;
    private Transform camtrans;
    // Start is called before the first frame update
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        camtrans = cam.transform;
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        transform.LookAt(transform.position + camtrans.forward);
    }
}
