using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour
{
    private static TouchManager instance = null;
    private Dictionary<int, List<JoyStickButton>> dicJoyStick;

    public static TouchManager getInstant()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        dicJoyStick = new Dictionary<int, List<JoyStickButton>>();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                this.checkAndRegist(touch.fingerId, touch.position);
                this.callTouchBegan(touch.fingerId, touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                this.callTouchMove(touch.fingerId, touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                this.callTouchEnd(touch.fingerId, touch.position);
                this.removeRegist(touch.fingerId);
            }
        }
    }
    private void callTouchBegan(int fingerId, Vector2 position)
    {
        if (this.dicJoyStick.ContainsKey(fingerId))
        {
            foreach (JoyStickButton jsb in this.dicJoyStick[fingerId])
            {
                jsb.onTouchBegan(position);
            }
        }
    }
    private void callTouchMove(int fingerId, Vector2 position)
    {
        if (this.dicJoyStick.ContainsKey(fingerId))
        {
            foreach (JoyStickButton jsb in this.dicJoyStick[fingerId])
            {
                jsb.onTouchMove(position);
            }
        }
    }
    private void callTouchEnd(int fingerId, Vector2 position)
    {
        if (this.dicJoyStick.ContainsKey(fingerId))
        {
            foreach (JoyStickButton jsb in this.dicJoyStick[fingerId])
            {
                jsb.onTouchEnd(position);
            }
        }
    }
    private void checkAndRegist(int fingerId, Vector2 position)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        for (int i = 0; i < results.Count; ++i)
        {
            JoyStickButton jsb = results[i].gameObject.GetComponent<JoyStickButton>();
            if (jsb && !jsb.isActive())
            {
                jsb.setActive(true);
                if (!dicJoyStick.ContainsKey(fingerId))
                {
                    dicJoyStick.Add(fingerId, new List<JoyStickButton>());
                }
                dicJoyStick[fingerId].Add(jsb);

                if (jsb.swallowTouch)
                {
                    break;
                }
            }
        }
    }
    private void removeRegist(int fingerId)
    {
        if (dicJoyStick.ContainsKey(fingerId))
        {
            foreach (JoyStickButton jsb in dicJoyStick[fingerId])
            {
                jsb.setActive(false);
            }
            dicJoyStick[fingerId].Clear();
            dicJoyStick.Remove(fingerId);
        }
    }
}
