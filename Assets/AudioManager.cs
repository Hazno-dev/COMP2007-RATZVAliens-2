using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Do Not Change")]
    public static float volume;
    private static GameObject instance;

    //Static object that is carried across scenes to accomidate volume changes in the main menu/game
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (instance == null)
        {
            instance = transform.gameObject;
            volume = 1;
        }
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public static void ChangeVolume(float vol)
    {
        volume = vol;
    }

    public static float ReturnVolume()
    {
        return volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
