using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    [Header("Do Not Change")]
    public float YSensitivity;
    public float XSensitivity;

    public Tutorial tut;

    public Transform orientation;
    public Transform Gun;

    float YRot;
    float xRot;

    public Slider SensScroller;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If player is interacting you dont want to move the camera
        if (PlayerStats.Interacting == true) return;
        float mousex = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * XSensitivity;
        float mousey = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * YSensitivity;

        //Calls tutorial to say the camera has been moved
        if (mousex != 0 || mousey != 0 && Tutorial.InTutorial) tut.TutorialDone("CAM");

        YRot += mousex;
        xRot -= mousey;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        //Rotates camera
        transform.rotation = Quaternion.Euler(xRot, YRot, 0);
        orientation.rotation = Quaternion.Euler(0, YRot, 0);

        //Ensures gun is constantly on screen
        Gun.rotation = Quaternion.Euler(xRot, YRot, 0);

    }
    public void ChangeSens()
    {
        XSensitivity = SensScroller.value + 1;
        YSensitivity = SensScroller.value + 1;
    }
}
