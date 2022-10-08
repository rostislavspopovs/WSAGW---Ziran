using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private Transform leavesParent;
    private float flourishTransitionTime;
    // Start is called before the first frame update
    void Start()
    {
        leavesParent = transform.GetChild(0);
        leavesParent.gameObject.SetActive(false);
        Flourish();
    }

    public void Flourish()
    {
        leavesParent.gameObject.SetActive(true);
        flourishTransitionTime = Random.Range(0.4f, 1.2f);
        StartCoroutine(FlourishCoroutine());
    }

    private IEnumerator FlourishCoroutine()
    {
        leavesParent.transform.localScale = Vector3.zero;
        float t = 0;
        while (t<flourishTransitionTime)
        {
            t += Time.deltaTime;
            leavesParent.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(1, 1, 1), AnimationCurve.EaseInOut(0, 0, flourishTransitionTime, 1).Evaluate(t));
            yield return new WaitForEndOfFrame();
        }
    }


}
