using UnityEngine;

public class JoyStickButton : MonoBehaviour
{
    private bool active = false;
    public bool swallowTouch = true;
    // Start is called before the first frame update
    void Start()
    {

    }
    void OnDestroy()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void onTouchBegan(Vector2 position)
    {
        Debug.Log("=========onTouchBegan=========: " + gameObject.name);
    }
    public void onTouchMove(Vector2 position)
    {
        Debug.Log("=========onTouchMove=========: " + gameObject.name);
    }
    public void onTouchEnd(Vector2 position)
    {
        Debug.Log("=========onTouchEnd=========: " + gameObject.name);
    }
    public void setActive(bool state)
    {
        this.active = state;
    }
    public bool isActive()
    {
        return active;
    }
}
