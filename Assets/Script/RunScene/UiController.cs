using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public GameObject[] StopUI;
    void Update()
    {
        if (BikeContller.instance.gameFlg)
        {
            for (int i = 0; i < StopUI.Length; i++)
                StopUI[i].SetActive(false);
        }
        else
        {
            for (int i = 0; i < StopUI.Length; i++)
                StopUI[i].SetActive(true);
        }
    }
}
