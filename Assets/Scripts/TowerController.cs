using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerController : MonoBehaviour
{
    public int symbolNum;
    public AudioClip nextBGM;
    public GameObject doors;
    public GameObject nextTerrain;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            doors.SetActive(true);
        }
    }

    public abstract void activateNextTerrain();
}
