﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickII : MonoBehaviour//失敗品
{
    #region 解析
    //[HideInInspector] 表示將原本顯示在面板上的序列化值隱藏起來。
    //[SerializeField] 表示將原本不會被序列化的私有變量和保護變量變成可以被序列化的，那麼它們在下次讀取的值就是你上次賦值的值。
    #endregion
    private float Width, WidthHalf;//搖桿底圖的X軸長度，一半(半徑)
    private float Height, HeightHalf;//搖桿底圖的X軸長度，一半(半徑)
    private RectTransform rectTransform;//取得底圖的(RectTransform)資料
    [Header("底圖位置"), SerializeField]
    private Transform visualJoyStick;
    [Header("搖桿位置"), SerializeField]
    private Transform Stick;
    [Header("點擊位置(隱藏)"), SerializeField]
    private Transform Block;
    [Header("顯示目前距離"), SerializeField]
    private Text OuptText;
    [Header("使用懸浮搖桿"), SerializeField]
    private bool IsFloatJoystick;
    private bool IsTouch;//是否點擊在搖桿裡面
    [Header("搖桿變化量")]
    public Vector2 joyStickVec;

    Touch touchData;//手指資料
    public Text[] text;
    // Use this for initialization
    void Start()
    {
        joyStickVec = Vector2.zero;//搖桿中心點歸零
        rectTransform = visualJoyStick.GetComponent<RectTransform>();
        Width = rectTransform.sizeDelta.x;//取得底圖的搖桿X軸長度
        Height = rectTransform.sizeDelta.y;//取得底圖的搖桿Y軸長度
        WidthHalf = Width * 0.5f;
        HeightHalf = Height * 0.5f;
        IsTouch = false;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateJoyStickVec();
        Vector3 vector = new Vector3(touchData.position.x, touchData.position.y, 0.0f);
        //取得點位置與搖桿距離
        float dist = Vector2.Distance(vector, visualJoyStick.position);
        text[0].text = touchData.phase.ToString() + "[Phase]";//手指狀態TouchPhase.Began TouchPhase.Moved TouchPhase.Ended
        text[1].text = dist.ToString() + "[距離]" + visualJoyStick.position.ToString() + "[visJS_Pos]";
        text[2].text = touchData.rawPosition.ToString() + "[rawPosition]";//手指一開始的位置
        text[3].text = touchData.position.ToString() + "[position]";//手指目前位置
        text[4].text = Input.touchCount.ToString() + "[touchCount]";
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.GetTouch(i);
            text[5].text = touch.fingerId.ToString() + "[F_Id]" + touch.position.ToString() + "[touchCount]";
        }
    }
    /// <summary>
    /// 顯示搖桿中心座標的方法
    /// </summary>
    /// <param name="vec">搖桿中心目前座標</param>
	void PrintJoyStickVec(Vector2 vec)
    {
        OuptText.text = vec.ToString("F2");//只顯示到小數點第二位[ToString("F2")]
    }

    /// <summary>
    /// 更新搖桿目前狀態的方法
    /// </summary>
	private void UpdateJoyStickVec()
    {
        SetJoyStickTouch();//設定手指
        
        //如果滑鼠為點擊或點擊到的介面不是此搖桿則跳出程式
        if (touchData.phase == TouchPhase.Ended)//如果沒有手指
        {
            RestJoyStick();//重設搖桿
            IsTouch = false;//沒有點擊到
            return;
        }

        // 如果開啟漂浮搖桿功能時..
        if (IsFloatJoystick)
        {
            if (Input.GetMouseButtonDown(0)) transform.position = Input.mousePosition;
        }

        Vector3 vector = new Vector3(touchData.position.x, touchData.position.y, 0.0f);
        //取得點位置與搖桿距離
        float dist = Vector2.Distance(vector, visualJoyStick.position);

        //如果滑鼠為點擊位置與搖桿間距離小於WidthHalof 則 Istouch= true;
        if (dist < WidthHalf)
        {
            IsTouch = true;
        }

        //如果點擊處為搖桿範圍內
        if (dist < WidthHalf)
        {
            Stick.position = vector;//中心點座標等於滑鼠座標
            GetJoyStickVec();//取得搖桿中心點位置位於底圖的距離向量(位置)
            PrintJoyStickVec(joyStickVec);//顯示目前的拖曳參數
            return;
        }

        //如果滑鼠拖曳出搖桿盤內
        if (IsTouch && (dist > WidthHalf))
        {
            //如果拖拉滑鼠盤脫離搖桿盤的範圍，取得圓的交點
            Block.transform.position = vector;//儲存滑鼠目前位置
            GetIntersections();//取得搖桿在底圖上的位置(虛擬)
            GetJoyStickVec();//取得搖桿中心點位置位於底圖的距離向量(位置)
            PrintJoyStickVec(joyStickVec);//顯示目前的拖曳參數
            return;
        }
    }
    /// <summary>
    /// 計算搖桿與底圖位置的方法
    /// </summary>
	private void GetIntersections()
    {
        /*
        Vector2 a = new Vector2(Input.mousePosition.x, Input.mousePosition.y);//滑鼠位置
        Vector2 b = new Vector2(transform.position.x, transform.position.y);//底圖位置
        Vector3 c = new Vector3(transform.position.x, transform.position.y, WidthHalf);//底圖位置+半徑
        Vector2 x = _GetIntersections(a.x, a.y, b.x, b.y, c.x, c.y, c.z);//計算
        Vector2 a = new Vector2(touchData.position.x, touchData.position.y);//滑鼠位置(第二版)
        */
        Vector2 b = new Vector2(visualJoyStick.position.x, visualJoyStick.position.y);//底圖位置
        Vector2 x = _getIntersections(b, touchData.position, WidthHalf);
        Stick.transform.position = new Vector3(x.x, x.y, 0);//回饋虛擬位置
    }
    /// <summary>
    /// 取得搖桿中心點位置位於底圖的距離向量(位置)的方法
    /// </summary>
	private void GetJoyStickVec()
    {
        joyStickVec = Stick.transform.position - visualJoyStick.position;//計算底圖中心點與搖桿中心點的位置距離
        joyStickVec.x /= WidthHalf;//歸一化
        joyStickVec.y /= HeightHalf;//歸一化
    }
    /// <summary>
    /// 重製搖桿參數的方法
    /// </summary>
	private void RestJoyStick()
    {
        joyStickVec = Vector2.zero;
        Stick.transform.position = visualJoyStick.position;
        PrintJoyStickVec(Vector3.zero);
    }
    /// <summary>
    /// 計算搖桿中心與點擊點的距離線與搖桿半徑的交點
    /// </summary>
    /// <param name="RockerCenter">搖桿中心點(底圖位置)</param>
    /// <param name="ClickPoint">點擊點</param>
    /// <param name="Radius">點擊半徑</param>
    /// <returns></returns>
    Vector2 _getIntersections(Vector2 RockerCenter, Vector2 ClickPoint, float Radius)
    {
        Vector2 directionVector = ClickPoint - RockerCenter;//距離座標
        float distance = directionVector.magnitude;//兩點的距離

        if (distance < Radius)//如果距離小於半徑
        {
            return ClickPoint;//點擊點
        }
        else
        {
            Vector2 finalAnswer;//最後計算結果變數
            finalAnswer.x = (Radius / distance) * directionVector.x;//圖解
            finalAnswer.y = (Radius / distance) * directionVector.y;//圖解
            finalAnswer += RockerCenter;
            return finalAnswer;
        }
    }
    /// <summary>
    /// 使用懸浮搖桿的開關
    /// </summary>
    public void ToggleIsFloatJoystick()
    {
        IsFloatJoystick = !IsFloatJoystick;

    }

    void SetJoyStickTouch()
    {
        //判斷進入的手指是哪個
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.GetTouch(i);//取得手指進入的資訊
            Vector3 vector = new Vector3(touch.position.x, touch.position.y, 0.0f);
            float x = Vector2.Distance(vector, visualJoyStick.position);
            if (x < WidthHalf)
            {
                touchData = touch;;//取得手指進入的資訊
            }
        }
    }

}
