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
    }

    public void Dive()
    {
        transform.position += longbow.transform.forward * 3.0f;//讓物件往自身的前方移動
        longbowAnimator.SetTrigger("Dive");
    }
    public void Aim()
    {
        longbowAnimator.SetTrigger("AimWalk");
    }
    public void AimRecoil()
    {
        longbowAnimator.SetTrigger("AimRecoil");
    }
}
