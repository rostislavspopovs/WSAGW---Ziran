using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject gameobject;

    private void OnTriggerEnter(Collider other)
    {
        gameobject.SetActive(true);
    }
}
