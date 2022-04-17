using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool GamePauseFlg;
    public Text GamePauseText;
    public bool IntroCameraView;
    public GameObject mainCamera;
    public GameObject subCamera;
    Animation ani;
    static public GameController instance;
    void Start()
    {
        if (instance == null)
            instance = this;

        GamePauseFlg = true;
        GamePauseText.text = "| |";

        IntroCameraView = true;
        ani = subCamera.GetComponent<Animation>();
        ani.Play();

    }
    void Update()
    {
        //　一時停止機能
        if (GamePauseFlg)
        {
            Time.timeScale = 1.0f;
            GamePauseText.text = "| |";
        }
        else
        {
            Time.timeScale = 0f;
            GamePauseText.text = "▷";
        }
        //カメラ切り替え
        if (IntroCameraView)
        {
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
        }
        else
        {
            mainCamera.SetActive(true);
            subCamera.SetActive(false);
        }
    }

    ///////////////////////////////////
    //　一時停止機能
    public void GamePause()
    {
        if (GamePauseFlg)
        {
            GamePauseFlg = false;
        }
        else
        {
            GamePauseFlg = true;
        }
    }

    public void GameStart()
    {
        //カメラ切り替え
        if (IntroCameraView)
            IntroCameraView = false;
        BikeContller.instance.IntroViewEnd();
    }



}
