using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    public GameObject uiObject;
    void Start()
    {
        uiObject.SetActive(false);
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D player)
    {
        if (player.gameObject.tag == "Player")
        {
            uiObject.SetActive(true);
            StartCoroutine("Wait");
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
        Destroy(uiObject);
    }
}
