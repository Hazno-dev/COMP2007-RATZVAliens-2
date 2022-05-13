using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInteractionPointUI : MonoBehaviour
{
    public string InteractionTag = "TurretInteractionPoint";
    public Transform player;
    GameObject[] InteractionPoints;
    // Start is called before the first frame update
    void Start()
    {
        InteractionPoints = GameObject.FindGameObjectsWithTag(InteractionTag);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
