using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GageController : MonoBehaviour
{

    public Slider slider;
    static public GageController instance;
    public bool GamePauseFlg;

    void Start()
    {
        if (instance == null)
            instance = this;
    }
    /////////////////////////////////////
    //　体力ゲージ
    public void DownFitness()
    {
        if (slider.value <= 0)
            return;
        slider.value = slider.value - 1;
    }
}
