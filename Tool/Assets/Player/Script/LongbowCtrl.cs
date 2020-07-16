using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongbowCtrl : MonoBehaviour
{
    #region 弓箭角色控制器
    [Header("搖桿")]
    public GameObject VisualJoyStick;
    JoyStick joyStick;
    [Header("玩家動畫控制器"), SerializeField]
    private Animator longbowAnimator;
    [Header("玩家物件"), SerializeField]
    private Transform longbow;
    [Header("切換速度"), Range(0.0f, 5.0f)]
    public float switchSpeed = 1.0f;//預設一秒後切換
    [Header("玩家移動速度")]
    public float speed;
    [Header("玩家移動權重")]
    public float movingWeights;

    bool moving = false;//判斷是否移動
    float nowSpeed;//現在的速度
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        longbowAnimator = transform.GetChild(0).GetComponent<Animator>();//自動抓取
        longbow = transform.GetChild(0).GetComponent<Transform>();//自動抓取
        moving = false;
        longbowAnimator.SetBool("Walk", false);
        longbowAnimator.SetBool("Run", false);
        joyStick = VisualJoyStick.GetComponent<JoyStick>();//取得程式碼(JoyStick)的資料
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) == false)
        {
            ResetData();
            return;
        }
        else
        { 
            Move();
        }
    }
    /// <summary>
    /// 角色移動的方法
    /// </summary>
    void Move()
    {
        if (!moving)//只執行一次(如果沒有在移動)
        {
            longbowAnimator.SetBool("Walk", true);//先跑步
            nowSpeed = speed;//初始速度
            Invoke("Run", switchSpeed);//switchSpeed秒後轉成跑步
            moving = true;//正在跑步中
        }
        MovingSup();//執行跑步輔助器
    }
    /// <summary>
    /// 跑步的方法
    /// </summary>
    void Run()
    {
        longbowAnimator.SetBool("Run", true);//轉成跑步的動畫
        nowSpeed = speed * movingWeights;//加速
    }
    /// <summary>
    /// 跑步輔助器的方法
    /// </summary>
    void MovingSup()
    {
        //以下為角色移動
        Vector3 dir = new Vector3(joyStick.joyStickVec.x, 0, joyStick.joyStickVec.y);//求得移動方向
        this.transform.Translate(dir.normalized * Time.deltaTime * nowSpeed, Space.World);//執行角色移動
        //以下為角色旋轉
        Quaternion qua = Quaternion.LookRotation(dir.normalized);//將Vector3型別轉換四元數型別
        longbow.transform.rotation = Quaternion.Lerp(longbow.transform.rotation, qua, 0.5f);//四元數的插值，實現平滑過渡
    }
    /// <summary>
    /// 重設所有參數的方法
    /// </summary>
    void ResetData()
    {
        longbowAnimator.SetBool("Walk", false);
        longbowAnimator.SetBool("Run", false);
        moving = false;
    }
}


