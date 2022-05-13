using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //Static variables not changed in editor
    //This class is a single static instance used to reference the players stats and such
    public static int Wood;
    public static int Scrap;
    public static int Crystal;
    public static bool Interacting;
    public static int Lives;
    public static int MaxLives = 20;

    [Header("Starting Attributes")]
    public int startWood = 400;
    public int startScrap = 400;
    public int startCrystal = 0;

    // Start is called before the first frame update
    void Start()
    {
        MaxLives = 20;
        Wood = startWood;
        Scrap = startScrap;
        Crystal = startCrystal;
        Lives = MaxLives;
        Interacting = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
