using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Jump : MonoBehaviour
{
    //[SerializeField] Animator anim;
    private Rigidbody rb;
    public float upForce; //上方向にかける力
    private bool isGround; //着地しているかどうかの判定
    Vector3 position;


    void Start()
    {
        rb = GetComponent<Rigidbody>(); //リジッドボディを取得
    }
    void Update()
    {
        if (!isGround)
            ReSetJump();

    }


    async void ReSetJump()
    {
        await Task.Delay(1000);
        isGround = true;
    }

    public void ButtonJump()
    {
        if (isGround)//着地しているとき
        {
            isGround = false;
            rb.AddForce(new Vector3(0, upForce, 0)); //上に向かって力を加える
        }
    }
}
