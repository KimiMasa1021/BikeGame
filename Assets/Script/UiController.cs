using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public GameObject[] ActiveUI;
    public GameObject[] StopUI;



    void Start()
    {

    }
    void Update()
    {

        if (BikeContller.instance.gameFlg)
        {
            /*for (int i = 0; i < ActiveUI.Length; i++)
                ActiveUI[i].SetActive(true);*/

            for (int i = 0; i < StopUI.Length; i++)
                StopUI[i].SetActive(false);
        }
        else
        {

            for (int i = 0; i < StopUI.Length; i++)
                StopUI[i].SetActive(true);

           /* for (int i = 0; i < ActiveUI.Length; i++)
                ActiveUI[i].SetActive(false);*/

        }

    }
}
