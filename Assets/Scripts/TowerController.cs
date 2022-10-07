using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
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
            other.GetComponent<PlayerController>().collectUI.gameObject.SetActive(true);
        }
    }
}
