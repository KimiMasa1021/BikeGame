using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool GamePauseFlg;
    public Text GamePauseText;
    void Start()
    {
        GamePauseFlg = true;
        GamePauseText.text = "| |";
    }
    void Update()
    {
        if (GamePauseFlg)
        {
            Time.timeScale = 1.0f;
            GamePauseText.text = "| |";
        }else
        {
            Time.timeScale = 0f;
            GamePauseText.text = "▷";
        }
    }

    ///////////////////////////////////
    //　一時停止機能
    public void GamePause()
    {
        Debug.Log("やりますね！！");
       if(GamePauseFlg)
       {
            GamePauseFlg = false;
       }else
       {
            GamePauseFlg = true;
       }
    }
}
