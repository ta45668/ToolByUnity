using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongbowAnimation : MonoBehaviour
{
    #region 弓箭手動畫的事件宣告
    [Header("玩家攻擊物件"),SerializeField]
    private GameObject playerLongbow;
    LongbowAction longbowAction;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        playerLongbow = this.transform.parent.gameObject;
        longbowAction = playerLongbow.GetComponent<LongbowAction>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 啟動翻滾結束的方法(在動畫裡)
    /// </summary>
    public void DiveEnd()
    {
        longbowAction.SetDiveEnd();
    }

    
}
