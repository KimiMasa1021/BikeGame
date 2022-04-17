using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;

public class BikeContller : MonoBehaviour
{

    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float bodyAngleLimit;
    public float speedLimit;
    public float upForce;
    public Text displaySpeed;
    public Text displayCoin;
    public Transform handle;
    public Transform clanck;
    public Transform fixedLegsR;
    public Transform fixedLegsL;
    public AudioSource audioS;
    public AudioClip getCoinAudio;
    public AudioClip ObstacleAudio;
    public ParticleSystem slidingEfect;
    static public BikeContller instance;
    public bool gameFlg;
    Rigidbody rg;
    float ModelAngle;
    public bool angleRFlg;
    public bool angleLFlg;
    bool keySteerR;
    bool keySteerL;
    int coin;
    public bool slidingFlg;
    Vector3 position;

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -0.5f, 0.2f);
        audioS = GetComponent<AudioSource>();
        gameFlg = true;
        ModelAngle = 0;
        angleRFlg = false;
        angleLFlg = false;
        coin = 0;
        slidingFlg = false;

        if (instance == null)
            instance = this;

    }
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
            return;
        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
    public void FixedUpdate()
    {
        rg = this.GetComponent<Rigidbody>();
        Quaternion BikeModel = this.transform.rotation;
        position = this.transform.position;
        if (gameFlg)
        {

            if (!slidingFlg)
            {
                //キーが押されてない場合車体をまっすぐに
                if (!angleRFlg && !angleLFlg)
                {
                    ModelAngle = maxSteeringAngle * 0;
                    this.transform.rotation = Quaternion.RotateTowards(BikeModel, Quaternion.Euler(0, 0, 0), 100f);
                }
                //車体の角度の限界制御 Y軸
                if (BikeModel.y > bodyAngleLimit)
                {
                    this.transform.rotation = new Quaternion(BikeModel.x, bodyAngleLimit, BikeModel.z, BikeModel.w);
                }

                if (BikeModel.y < bodyAngleLimit * -1)
                    this.transform.rotation = new Quaternion(BikeModel.x, bodyAngleLimit * -1, BikeModel.z, BikeModel.w);
                //クランクの回転
                clanck.transform.Rotate(new Vector3(1, 0, 0), 10);
                //右折
                if (angleRFlg)
                    ModelAngle = maxSteeringAngle * 1;
                //左折
                if (angleLFlg)
                    ModelAngle = maxSteeringAngle * -1;
            }
            //一定の速度まで力を加える
            Vector3 force = new Vector3(0.0f, 0.0f, maxMotorTorque);
            if (rg.velocity.magnitude < speedLimit)
                rg.AddForce(force);
            //タイヤの個数分ループしてる
            foreach (AxleInfo axleInfo in axleInfos)
            {
                //ハンドルの角度の制御
                if (axleInfo.steering)
                {
                    axleInfo.Wheel.steerAngle = ModelAngle;
                    handle.localEulerAngles = new Vector3(0, ModelAngle * 2, 0);
                }
                ApplyLocalPositionToVisuals(axleInfo.Wheel);
            }
        }

        //速度(km/h)の取得
        this.displaySpeed.text = "速度：" + Mathf.Floor((float)(rg.velocity.magnitude * 3.6)).ToString() + "km/h";
        //crankのrotation取得
        Quaternion rookFoodDate = clanck.transform.rotation;
        //左右の足の角度を固定
        fixedLegsR.transform.rotation = new Quaternion(0, 0, 0, rookFoodDate.w);
        fixedLegsL.transform.rotation = new Quaternion(0, 0, 0, rookFoodDate.w);

        displayCoin.text = "コイン数：" + coin;
    }

    public void  RmConstraint()
    {
        rg.constraints = RigidbodyConstraints.None;
    }



    //  左右の動き  ///////////////////////////////////////
    public void RightMove()
    {
        if(HeightChecker())
        {
            angleRFlg = true;
                rg.AddForce(new Vector3(40000,0,0));
            rg.constraints = RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            if(GageController.instance.slider.value > 0)
            {
                rg.constraints = RigidbodyConstraints.None;
                angleRFlg = true;
                rg.AddForce(new Vector3(40000,0,0));
                GageController.instance.DownFitness();
            }
        }
    }
    public void LeftMove()
    {

        if(HeightChecker())
        {
            angleLFlg = true;
                rg.AddForce(new Vector3(-40000,0,0));
            rg.constraints = RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            if(GageController.instance.slider.value > 0)
            {
                rg.constraints = RigidbodyConstraints.None;
                angleLFlg = true;
                rg.AddForce(new Vector3(-40000, 0,0));
                GageController.instance.DownFitness();
            }
        }
    }
    //////////////////////////////////////////////////////
    void AllFalse()
    {
        angleLFlg = false;
        angleRFlg = false;
        rg.constraints = RigidbodyConstraints.None;
        rg.constraints = RigidbodyConstraints.FreezePositionX;
    }
    //衝突検知　音声///////////////////////////////////////
    public string freeOJ = "reset";
    private void OnTriggerEnter(Collider other)
    {

        //コース誘導のオブジェクト接触判定
        if (other.gameObject.CompareTag("CouseBar"))
        {
            if (freeOJ != "right")
                AllFalse();
            freeOJ = "right";
        }
        if (other.gameObject.CompareTag("CouseBar1"))
        {
            if (freeOJ != "center")
                AllFalse();
            freeOJ = "center";
        }
        if (other.gameObject.CompareTag("CouseBar2"))
        {
            if (freeOJ != "left")
                AllFalse();
            freeOJ = "left";
        }
        if (other.gameObject.CompareTag("Reset"))
        {
            freeOJ = "reset";
        }
        //コインの衝突
        if (other.gameObject.CompareTag("coin"))
        {
            other.gameObject.SetActive(false);
            audioS.PlayOneShot(getCoinAudio);
            coin++;
        }
        //障害物の衝突
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("policeBar"))
        {
            //移動系の制御を停止　上にあるやつ
            gameFlg = false;
            //重力を停止
            rg.isKinematic = true;
            audioS.PlayOneShot(ObstacleAudio);
            this.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, -100, 90), 100f);
        }
    }
    //////////////////////////////////////////////////////////

    //スライディング機能///////////////////////////////////////
    public async void slidingIvent()
    {
        if (this.transform.rotation == null) return;
        slidingFlg = true;
        if (!slidingEfect.isEmitting) slidingEfect.Play();
        this.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, -35, 50), 100f);
        await Task.Delay(600);
        this.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), 100f);
        if (slidingEfect.isEmitting) slidingEfect.Stop();
        slidingFlg = false;
    }
    //////////////////////////////////////////////////////////

    //ジャンプ機能/////////////////////////////////////////////
    public void Jump()
    {
        if(HeightChecker())
        {
            rg.AddForce(new Vector3(0, upForce,0)); //上に向かって力を加える
        }
    }
    //////////////////////////////////////////////////////////
    public bool HeightChecker()
    {
        if(rg.position.y <= -22.5)
            return true;
        else return false; 
    }

    //ホイールパラメータのクラス
    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider Wheel;
        //ハンドル操作をしたときに角度が変わるか
        public bool steering;
    }

    //ホーム画面に戻る
    public void GameOverToHome()
    {
        SceneManager.LoadScene("StartScene");
    }
}