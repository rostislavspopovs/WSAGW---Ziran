using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEmerge : MonoBehaviour
{
    public GameObject tower;
    public GameObject lightBeam;

    private Vector3 direction;
    private Vector3 target;
    private float distance;
    private float time = 15.0f;//seconds
    private bool close;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
        direction = new Vector3(0,40,0);
        target = tower.transform.position + direction;
        distance = direction.magnitude;
    }

    void Update()
    {
        if (close)
        {
            Destroy(lightBeam);
            if (distance > 0)
            {
                tower.transform.Translate(direction * (Time.deltaTime * (distance / time)));
                distance = (target - tower.transform.position).magnitude;
            }
            else
            {
                Destroy(this.GetComponent<TowerEmerge>());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            close = true;
            source.Play();
        }
    }
}
