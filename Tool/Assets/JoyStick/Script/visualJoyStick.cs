using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class visualJoyStick : MonoBehaviour
{
    #region 解析
    //[HideInInspector] 表示將原本顯示在面板上的序列化值隱藏起來。
    //[SerializeField] 表示將原本不會被序列化的私有變量和保護變量變成可以被序列化的，那麼它們在下次讀取的值就是你上次賦值的值。
    #endregion
    private float Width, WidthHalf;//搖桿底圖的X軸長度，一半(半徑)
    private float Height, HeightHalf;//搖桿底圖的X軸長度，一半(半徑)
    private RectTransform RectTransform;
	[Header("搖桿中心圖"),SerializeField] private Transform Stick, Block;
	[Header("顯示目前距離"),SerializeField] private UnityEngine.UI.Text OuptText;
	[Header("使用懸浮搖桿"),SerializeField] private bool IsFloatJoystick;
    private bool IsTouch;//是否點擊在搖桿裡面
	[Header("搖桿變化量")]
    public Vector2 joyStickVec;
	// Use this for initialization

	void Start()
	{
		joyStickVec = Vector2.zero;//搖桿中心點歸零
		RectTransform = GetComponent<RectTransform>();
		Width = RectTransform.sizeDelta.x;//取得底圖的搖桿X軸長度
		Height = RectTransform.sizeDelta.y;//取得底圖的搖桿Y軸長度
        WidthHalf = Width * 0.5f;
		HeightHalf = Height * 0.5f;
		IsTouch = false;
    }


	// Update is called once per frame
	void Update()
	{
		UpdateJoyStickVec();
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
		//如果滑鼠為點擊或點擊到的介面不是此搖桿則跳出程式
		if (Input.GetMouseButton(0) == false)
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

		//取得點位置與搖桿距離
		float dist = Vector2.Distance(Input.mousePosition, transform.position);


		//如果滑鼠為點擊位置與搖桿間距離小於WidthHalof 則 Istouch= true;
		if (dist < WidthHalf)
		{
			IsTouch = true;
		}

		//如果點擊處為搖桿範圍內
		if (dist < WidthHalf)
		{
			Stick.position = Input.mousePosition;//中心點座標等於滑鼠座標
			GetJoyStickVec();//取得搖桿中心點位置位於底圖的距離向量(位置)
			PrintJoyStickVec(joyStickVec);//顯示目前的拖曳參數
			return;
		}

        //如果滑鼠拖曳出搖桿盤內
        if (IsTouch && (dist > WidthHalf))
        {
            //如果拖拉滑鼠盤脫離搖桿盤的範圍，取得圓的交點
            GetIntersections();//取得搖桿在底圖上的位置(虛擬)
            GetJoyStickVec();//取得搖桿中心點位置位於底圖的距離向量(位置)
            PrintJoyStickVec(joyStickVec);//顯示目前的拖曳參數
            Block.transform.position = Input.mousePosition;//儲存滑鼠目前位置
            return;
        }
	}
    /// <summary>
    /// 計算搖桿與底圖位置的方法
    /// </summary>
	private void GetIntersections()
	{
		Vector2 a = new Vector2(Input.mousePosition.x, Input.mousePosition.y);//滑鼠位置
		Vector2 b = new Vector2(transform.position.x, transform.position.y);//底圖位置
		Vector3 c = new Vector3(transform.position.x, transform.position.y, WidthHalf);//底圖位置+半徑
        Vector2 x = _getIntersections(a.x, a.y, b.x, b.y, c.x, c.y, c.z);//計算
		Stick.transform.position = new Vector3(x.x, x.y, 0);//回饋虛擬位置
	}
    /// <summary>
    /// 取得搖桿中心點位置位於底圖的距離向量(位置)的方法
    /// </summary>
	private void GetJoyStickVec()
	{
		joyStickVec = Stick.transform.position - this.transform.position;//計算底圖中心點與搖桿中心點的位置距離
		joyStickVec.x /= WidthHalf;//歸一化
		joyStickVec.y /= HeightHalf;//歸一化
    }
    /// <summary>
    /// 重製搖桿參數的方法
    /// </summary>
	private void RestJoyStick()
	{
		joyStickVec = Vector2.zero;//
		Stick.transform.position = this.transform.position;
		PrintJoyStickVec(Vector3.zero);
	}
    /// <summary>
    /// 取得交點用的方法(用來計算位置)
    /// </summary>
    /// <param name="ax">滑鼠位置(X座標)</param>
    /// <param name="ay">滑鼠位置(Y座標)</param>
    /// <param name="bx">底圖位置(X座標)</param>
    /// <param name="by">底圖位置(Y座標)</param>
    /// <param name="cx">底圖位置(X座標)</param>
    /// <param name="cy">底圖位置(Y座標)</param>
    /// <param name="cz">底圖半徑</param>
    /// <returns>搖桿中心位置(虛擬)</returns>
    Vector2 _getIntersections(float ax, float ay, float bx, float by, float cx, float cy, float cz)
	{
        #region 數學註解
        /*
         * Mathf.Pow 次方 => Pow (f , p) [計算 f 的 p 次方]
         * Mathf.Sqrt 平方根 => Sqrt (f , p) [對f開以p為底的根號]
         */
        #endregion
        float[] a = { ax, ay }, b = { bx, by }, c = { cx, cy, cz };//儲存陣列變數
		// Calculate the euclidean distance between a & b[計算a和b之間的距離]X^2+Y^2後開根號
		float eDistAtoB = Mathf.Sqrt(Mathf.Pow(b[0] - a[0], 2) + Mathf.Pow(b[1] - a[1], 2));//X^2+Y^2後開根號

        // compute the direction vector d from a to [取得方向向量]
        float[] d = {
			(b[0] - a[0]) / eDistAtoB,
			(b[1] - a[1]) / eDistAtoB
		};

		// Now the line equation is x = dx*t + ax, y = dy*t + ay with 0 <= t <= 1.

		// compute the value t of the closest point to the circle center (cx, cy)
		var t = (d[0] * (c[0] - a[0])) + (d[1] * (c[1] - a[1]));

		// compute the coordinates of the point e on line and closest to c
		var ecoords0 = (t * d[0]) + a[0];
		var ecoords1 = (t * d[1]) + a[1];

		// Calculate the euclidean distance between c & e
		var eDistCtoE = Mathf.Sqrt(Mathf.Pow(ecoords0 - c[0], 2) + Mathf.Pow(ecoords1 - c[1], 2));

		// test if the line intersects the circle
		if (eDistCtoE < c[2])
		{
			// compute distance from t to circle intersection point
			var dt = Mathf.Sqrt(Mathf.Pow(c[2], 2) - Mathf.Pow(eDistCtoE, 2));

			// compute first intersection point
			var fcoords0 = ((t - dt) * d[0]) + a[0];
			var fcoords1 = ((t - dt) * d[1]) + a[1];
			// check if f lies on the line
			//        f.onLine = is_on (a, b, f.coords);

			// compute second intersection point
			// var gcoords0 = ((t + dt) * d[0]) + a[0];
			// var gcoords1 = ((t + dt) * d[1]) + a[1];
			Vector2 finalAnswer = new Vector2(fcoords0, fcoords1);

			// check if g lies on the line
			return (finalAnswer);

		}

		return Vector2.zero;

	}

	public void ToggleIsFloatJoystick()
	{
		IsFloatJoystick = !IsFloatJoystick;

	}

}
