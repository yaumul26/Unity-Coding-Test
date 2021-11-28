using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    void OnTriggerEnter(Collider otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            this.transform.GetChild(2).gameObject.SetActive(true);
            StartCoroutine(delayExplode(0.4f));
        }
    }

    IEnumerator delayExplode(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
