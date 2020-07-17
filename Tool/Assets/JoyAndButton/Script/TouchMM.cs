using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchMM : MonoBehaviour
{
    public Text[] text;

    int number;
    Touch touch1;
    // Start is called before the first frame update
    void Start()
    {
        number = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //判斷要放在最下層
        //判斷是哪個手指[Input.touchCount = 記錄你的手指觸碰螢幕的順序]
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (i == 0)
            {
                touch1 = Input.GetTouch(i);
            }
            Touch touch = Input.GetTouch(i);
            text[0].text = touch1.phase.ToString() + "[Phase]";//手指狀態TouchPhase.Began TouchPhase.Moved TouchPhase.Ended
            text[1].text = touch.tapCount.ToString() + "[tapCount]";//手指點擊次數
            text[2].text = touch1.rawPosition.ToString() + "[rawPosition]";//手指一開始的位置
            text[3].text = touch1.position.ToString() + "[position]";//手指目前位置
            text[4].text = Input.touchCount.ToString()+ "[touchCount]" + number.ToString();            
        }
    }

    public void ButtonTouch()
    {
        number++;
    }
}
