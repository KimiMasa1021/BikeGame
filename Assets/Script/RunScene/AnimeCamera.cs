using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimeCamera : MonoBehaviour
{
    public Camera PlayerCamera { get; set; }    
 
    void EndAnimation()
    {
        // カメラを削除
        Destroy(gameObject);
 
        // プレイヤーのカメラをオン
        PlayerCamera.enabled = true;
 
    }
}
