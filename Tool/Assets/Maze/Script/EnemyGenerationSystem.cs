using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerationSystem : MonoBehaviour
{
    #region 敵人生成系統宣告
    [Header("敵人物件清單"),SerializeField]
    private List<Transform> enemyObj = new List<Transform>();
    [Header("敵人數量調整"), Range(1.0f, 4.0f)]
    public float enemyQuantity = 1.0f;
    [Header("敵人生成空間"), SerializeField]
    private Transform enemySpace;

    Transform enemyScratchpad;//敵人暫存器
    #endregion
    /// <summary>
    /// 初始化(initialization)這支程式的方法
    /// </summary>
    public void Initial()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 決定敵人物件的方法
    /// </summary>
    void DecideEnemyObj()
    {
        int number = Random.Range(0, enemyObj.Count);
        enemyScratchpad = enemyObj[number];
    }
    /// <summary>
    /// 敵人生成器的方法
    /// </summary>
    /// <param name="place">生成地點</param>
    /// <param name="number">生成編號</param>
    public void EnemyBuilder(Vector3 place,int number)
    {
        place += Vector3.up;//矯正生成位置
        DecideEnemyObj();//確認生成物
        Transform enemy = Instantiate(enemyScratchpad, place, Quaternion.identity, enemySpace);//生成
        enemy.name = enemyScratchpad.name + "(" + number.ToString() + ")";//改名子
    }
}
