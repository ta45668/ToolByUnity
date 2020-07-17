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
    // Start is called before the first frame update
    void Start()
    {
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
    }
    /// <summary>
    /// 翻滾的方法
    /// </summary>
    public void Dive()
    {
        longbowAnimator.SetBool("Dive", true);//開始翻滾動畫
    }
    /// <summary>
    /// 舉起弓箭的方法
    /// </summary>
    public void Aim()
    {
        longbowAnimator.SetTrigger("AimWalk");
    }
    /// <summary>
    /// 射出箭的方法
    /// </summary>
    public void AimRecoil()
    {
        longbowAnimator.SetTrigger("AimRecoil");
    }
}
