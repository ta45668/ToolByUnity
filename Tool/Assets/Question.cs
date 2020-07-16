using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Question : MonoBehaviour
{
    int a = 100;
    Transform target;
    #region Question
    // 1、有哪些事件函數
    // Awake Start Update FixedUpdate LateUpdate OnEnable OnDisable OnDestroy OnGUI
    // OnCollisionEnter(Collision col) OnCollisionStay(Collision col) OnCollisionExit(Collision col)
    // OnTriggerEnter(Collider o) OnTriggerStay(Collider o) OnTriggerExit(Collider o)


    // 2、如何查找場景中遊戲物體
    // GameObject obj = GameObject.Find("ObjName/ChildObjName")


    // 3、將自己設置為 找到的那個遊戲物體的 父物體
    // obj.transform.parent = transform;


    // 4、掛載腳本的必要條件
    // 繼承MonoBehaviour 、所有的代碼無錯誤、文件名和類名一致、不能是抽象類


    // 5、物體的移動、旋轉


    // 改變位置的移動
    // 1)、往世界的前方移動
    // transform.position += Vector3.forward;
    // 2)、往自身的前方移動
    // transform.position += transform.forward;


    // API移動
    // 1)、往世界的前方移動
    // transform.Translate(Vector3.forward, Space.World);
    // 2)、往自身的前方移動
    // transform.Translate(transform.forward, Space.World);
    // transform.Translate(Vector3.forward, Space.Self);


    // 使物體旋轉  (自身)
    // 改變歐拉角的形式 來旋轉
    // 世界的Y軸轉
    // transform.eulerAngles += Vector3.up;
    // 自身的Y軸轉
    // transform.eulerAngles += transform.up;


    // API
    // 世界的Y軸轉
    // transform.Rotate(Vector3.up, Space.World)
    // 自身的Y軸轉
    // transform.Rotate(transform.up, Space.World)
    // transform.Rotate(Vector3.up)


    // 使物體旋轉  (繞其他物體)
    // GameObjet obj; (被繞的遊戲物體)
    // 繞Y軸轉（世界的）
    // transform.RotateAround(obj.transform.position, Vector3.up, 1);
    // 繞Y軸轉（被繞的遊戲物體）
    // transform.RotateAround(obj.transform.position, obj.transform.up, 1);


    // 6、如何生成、銷毀遊戲物體？


    // 生成（需要有預制體）  BulletPrefab
    // public GameObject bulletPrefab;    // 引用資源中的預制體
    // Transform spawnTrans;   // 生成的位置及方位
    // Instantiate(bulletPrefab, spawnTrans.position  /*位置 Vector3*/, Quaternion.identity /*spawnTrans.rotation*/ /*方位 Quaternion*/)


    // 銷毀
    // 不要循環調用
    // 銷毀當前的腳本組件
    // 銷毀的時機： 在 Update 之後，渲染之前
    // Destroy(this);
    // 銷毀當前的遊戲物體
    // Destroy(gameObject);
    // 5秒以後銷毀 當前腳本組件
    // Destroy(this, 5); 


    // 7、物體之間發生碰撞的必要條件
    // 1)、雙方都必須有碰撞器 2）、至少有一個非休眠的剛體 3）、如果一方是運動學剛體，另一個必須是非運動學剛體
    // 觸發的事件函數  雙方的
    // OnCollisionEnter(Collision col) OnCollisionStay(Collision col) OnCollisionExit(Collision col)
    // 如果勾選了 IsTrigger 會？    在所有的 Collider上
    // 會變成觸發器，不會再有碰撞效果了


    // 什麽情況下會觸發 觸發器
    // 1)雙方都有碰撞器，至少有一方勾選了IsTrigger 2)、至少有一個有剛體
    // 觸發的事件函數  雙方的
    // OnTriggerEnter(Collider o) OnTriggerStay(Collider o) OnTriggerExit(Collider o)
    #endregion
    // Use this for initialization
    void Start()
    {
        Destroy(this);
        a = 200;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
