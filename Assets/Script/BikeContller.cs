using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BikeContller : MonoBehaviour
{

    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float bodyAngleLimit;
    public float speedLimit;
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
    bool angleRFlg;
    bool angleLFlg;
    bool keySteerR;
    bool keySteerL;
    int coin;
    bool slidingFlg;

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
        if (gameFlg)
        {


            Vector3 force = new Vector3(0.0f, 0.0f, maxMotorTorque);

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

    private async void OnTriggerEnter(Collider other)
    {
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

    public void RPushDown()
    {
        angleRFlg = true;
    }
    public void RPushUp()
    {
        angleRFlg = false;
    }
    public void LPushDown()
    {
        angleLFlg = true;
    }
    public void LPushUp()
    {
        angleLFlg = false;
    }


    //スライディング機能　カッコ非同期処理
    public async void slidingIvent()
    {
        if (this.transform.rotation == null) return;
        slidingFlg = true;
        if (!slidingEfect.isEmitting) slidingEfect.Play();
        this.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, -30, 60), 100f);
        await Task.Delay(1000);
        this.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), 100f);
        if (slidingEfect.isEmitting) slidingEfect.Stop();
        slidingFlg = false;

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
        public void GameOverToHome(){
                SceneManager.LoadScene("StartScene");
    }
}