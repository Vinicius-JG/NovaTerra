using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterArea : MonoBehaviour
{
    GameObject[] cams;
    GameObject myCam;

    private void Awake()
    {
        cams = GameObject.FindGameObjectsWithTag("Cam");
        myCam = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < cams.Length; i++)
            {
                cams[i].SetActive(false);
            }
            myCam.SetActive(true);
        }
    }
}
