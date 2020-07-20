using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongbowAction : MonoBehaviour
{
    #region 弓箭手的動作宣告
    [Header("玩家動畫控制器"), SerializeField]
    private Animator longbowAnimator;
    [Header("玩家物件"), SerializeField]
    private Transform longbow;   
    [Header("玩家翻滾速度")]
    public float diveSpeed;

    AnimatorStateInfo stateInfo;//取得動畫 狀態訊息(stateInfo)
    #endregion
    #region 攻擊鏡頭切換
    [Header("攝影機物件(世界)"), SerializeField]
    private Transform longbowCamera;
    [Header("攝影機物件(攻擊時)"), SerializeField]
    private Transform cameraPosition;
    [Header("攝影機物件(原本)"), SerializeField]
    private Transform originalCameraPos;

    bool isDive = false;//判斷是否翻滾
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        isDive = false;//初始
        longbowAnimator = transform.GetChild(0).GetComponent<Animator>();//自動抓取
        longbow = transform.GetChild(0).GetComponent<Transform>();//自動抓取
    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = longbowAnimator.GetCurrentAnimatorStateInfo(0);//取得動畫層[0] 指向Base Layer
        if (stateInfo.IsName("Base Layer.DiveForward"))//如果正在做翻滾動畫
        {
            if (!longbowAnimator.IsInTransition(0))//如果此動畫還在播放
            {
                transform.Translate(longbow.forward * 0.01f * diveSpeed, Space.World);//位移
            }
            longbowAnimator.SetBool("Dive", false);//結束後取消
        }
        
        if (stateInfo.IsName("Base Layer.AimWalkForward"))//如果正在做舉弓動畫
        {
            if (!longbowAnimator.IsInTransition(0))//如果此動畫還在播放
            {
                longbowCamera.position = cameraPosition.position;//移動位置
                longbowCamera.rotation = cameraPosition.rotation;//移動旋轉位
            }
            longbowAnimator.SetBool("AimWalk", false);//結束後取消
        }

        if (stateInfo.IsName("Base Layer.AimRecoil"))//如果正在做舉弓動畫
        {
            if (!longbowAnimator.IsInTransition(0))//如果此動畫還在播放
            {
            }
            longbowAnimator.SetBool("AimRecoil", false);//結束後取消
            longbowCamera.position = originalCameraPos.position;//移動位置
            longbowCamera.rotation = originalCameraPos.rotation;//移動旋轉位
        }
    }
    /// <summary>
    /// 翻滾開始的方法
    /// </summary>
    public void Dive()
    {
        longbowAnimator.SetBool("Dive", true);//開始翻滾動畫
        isDive = true;
    }
    /// <summary>
    /// 設定翻滾結束的方法
    /// </summary>
    public void SetDiveEnd()
    {
        isDive = false;
    }
    /// <summary>
    /// 舉起弓箭的方法
    /// </summary>
    public void Aim()
    {
        if (!isDive)
        {
            longbowAnimator.SetBool("AimWalk",true);
            longbowAnimator.SetBool("AimRecoil", false);
        }
        return;
    }
    /// <summary>
    /// 射出箭的方法
    /// </summary>
    public void AimRecoil()
    {
        longbowAnimator.SetBool("AimWalk", false);
        longbowAnimator.SetBool("AimRecoil",true);
    }
}
