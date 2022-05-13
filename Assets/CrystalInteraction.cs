using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalInteraction : MonoBehaviour
{
    [Header("Do Not Change")]
    public bool CanSearch = true;
    public Text EInteractText;
    private bool EPressable = false;
    private bool Searching = false;
    public GameObject scrollbarobject;
    public Scrollbar scroller;
    public float timer = 10f;
    private float counter = 0f;

    [Header("Audio")]
    public AudioSource SuccessAudio;
    public AudioSource FailAudio;
    public AudioSource RummageAudio;

    public AudioClip FailAudioDebug;
    public AudioClip SuccessAudioDebug;
    // Start is called before the first frame update
    void Start()
    {
        if (SuccessAudio != null) InvokeRepeating("UpdateVolume", 0f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSearch == false) return;
        if (Input.GetKeyDown(KeyCode.E) && EPressable == true)
        {
            //Start interaction
            RummageAudio.Play();
            EInteractText.enabled = false;
            EPressable = false;
            Searching = true;
            movement.vertInput = 0f;
            movement.hozInput = 0f;
            scrollbarobject.SetActive(true);
        }
        if (Searching && counter < timer)
        {
            scroller.size = Mathf.InverseLerp(0f, timer, counter);
            counter += Time.deltaTime;
            if (movement.vertInput != 0f || movement.hozInput != 0f)
            {
                //Player moved :(
                RummageAudio.Stop();
                //FailAudio.Play();
                AudioSource.PlayClipAtPoint(FailAudioDebug, transform.position, AudioManager.ReturnVolume() / 6);
                scroller.size = 0f;
                Searching = false;
                scrollbarobject.SetActive(false);
                counter = 0f;
            }
        }
        if (Searching && counter >= timer)
        {
            //Give crystals!!
            RummageAudio.Stop();
            //SuccessAudio.Play();
            AudioSource.PlayClipAtPoint(SuccessAudioDebug, transform.position, AudioManager.ReturnVolume() / 4);
            PlayerStats.Crystal += 1;
            scroller.size = 0f;
            counter = 0f;
            Searching = false;
            scrollbarobject.SetActive(false);
            CanSearch = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    
    //Check if player is inside the crystal collection zone
    private void OnTriggerEnter(Collider other)
    {
        if (CanSearch == false) return;
        EInteractText.enabled = true;
        EPressable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (CanSearch == false) return;
        EInteractText.enabled = false;
        EPressable = false;
    }
    void UpdateVolume()
    {
        SuccessAudio.volume = AudioManager.ReturnVolume() / 4;
        FailAudio.volume = AudioManager.ReturnVolume() / 6;
        RummageAudio.volume = AudioManager.ReturnVolume();
    }
}
