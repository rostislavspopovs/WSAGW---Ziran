using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footpath : MonoBehaviour
{
    [SerializeField] float stepPeriod = 0.2f;

    // Start is called before the first frame update
    void OnEnable()
    {
        foreach (Transform footprint in transform)
        {
            footprint.gameObject.SetActive(false);
        }
        SpawnFootprints(stepPeriod);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFootprints(float stepPeriod)
    {
        StartCoroutine(SpawnFootprintsCorountine(stepPeriod));
    }

    private IEnumerator SpawnFootprintsCorountine(float stepPeriod)
    {
        foreach (Transform footprint in transform)
        {
            footprint.gameObject.SetActive(true);
            footprint.GetComponent<Animation>().Play();
            footprint.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(stepPeriod);
        }
    }

}
