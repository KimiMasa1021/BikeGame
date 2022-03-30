using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    //StartSceneからRunSceneへの遷移
    public void GameStart(){
        Debug.Log("よみこみでござんす。");
        SceneManager.LoadScene("RunScene");
    }
}
