using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthSystem : MonoBehaviour
{
    #region 迷宮系統宣告
    [Header("迷宮大小(X*X)")]
    public int labyrinthSize;
    [Header("迷宮組成物件"), Tooltip("0=地板，1=牆壁Z，2=牆壁Y"), SerializeField]
    private GameObject[] labyrinthCube = new GameObject[3];
    /*舊宣告物件
    [Header("地板物件")]
    public GameObject floorCube;
    [Header("牆壁物件(Z)")]
    public GameObject wallCubeZ;
    [Header("牆壁物件(Y)")]
    public GameObject wallCubeY;
    */
    [Header("牆壁生成空間"), SerializeField]
    private Transform wallSpace;
    [Header("地板生成空間"), SerializeField]
    private Transform floorSpace;
    [Header("牆壁高度")]
    public int wallHigh;
    [HideInInspector]//在面板中隱藏資料
    public List<Transform> floorList = new List<Transform>();//地板清單(用來控制用)
    /// <summary>
    /// 宣告enemyGenerationSystem是屬於EnemyGenerationSystem程式碼的代理人
    /// </summary>
    EnemyGenerationSystem enemyGenerationSystem
    {
        get { return GameObject.Find("EnemyGeneration").GetComponent<EnemyGenerationSystem>(); }//取得在此物件上的程式碼
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        labyrinthSize = Mathf.Clamp(labyrinthSize, 5, 20);//限制地圖大小[Clamp(你所要判斷的值,最小值,最大值)=讓值在最大最小值之間]
        BuildFloor();//建造地板
        BuildWall(-0.55f);//建造1/2牆
        BuildWall(-0.55f + labyrinthSize);//建造1/2牆
        BuildPillarAndEnemy();//建造柱子
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 創建地板的方法
    /// </summary>
    void BuildFloor()
    {
        for (int i = 0; i < labyrinthSize; i++)
        {
            for (int j = 0; j < labyrinthSize; j++)
            {
                Vector3 vector = new Vector3((float)i, 0, (float)j);//指定生成位置
                GameObject floor = Instantiate(labyrinthCube[0], vector, Quaternion.identity, floorSpace);//生成地板
                floor.name = "FloorCube(" + ((labyrinthSize * i) + j).ToString() + ")";//改名子
                floorList.Add(floor.transform);//加入清單中(方便管理)
            }
        }
    }
    /// <summary>
    /// 創建牆壁的方法
    /// </summary>
    /// <param name="SP">StartingPoint建造起始點位置</param>
    void BuildWall(float SP)
    {
        for (int i = 0; i < labyrinthSize; i++)
        {
            for (int j = 0; j < wallHigh; j++)
            {
                Vector3 vectorZ = new Vector3(SP, ((float)j + 0.55f), (float)i);//指定生成位置(Z牆)
                Vector3 vectorY = new Vector3((float)i, ((float)j + 0.55f), SP);//指定生成位置(Y牆)
                string nameZ = "wallCubeZ(" + ((wallHigh * i) + j).ToString() + ")" + SP.ToString();//命名(Z牆)
                string nameY = "wallCubeY(" + ((wallHigh * i) + j).ToString() + ")" + SP.ToString();//命名(Y牆)
                //使用輔助器建造
                BuildWallSUP(labyrinthCube[1], vectorZ, nameZ);//創建牆壁Z
                BuildWallSUP(labyrinthCube[2], vectorY, nameY);//創建牆壁Y
            }
        }
    }
    /// <summary>
    /// 創建牆壁輔助器
    /// </summary>
    /// <param name="wallCube">創建牆壁的物件</param>
    /// <param name="vector">創建位置</param>
    /// <param name="wallName">創建物件名稱</param>
    void BuildWallSUP(GameObject wallCube, Vector3 vector, string wallName)
    {
        GameObject wall = Instantiate(wallCube, vector, Quaternion.identity, wallSpace);//生成牆壁
        wall.name = wallName;//改名子
    }
    /// <summary>
    /// 創建柱子與敵人的方法
    /// </summary>
    void BuildPillarAndEnemy()
    {
        int floorTotal = labyrinthSize * labyrinthSize;//計算總共有幾格地板
        bool[] RandomNumberBool = new bool[floorTotal];//防止會有重複的地板產生

        for (int i = 0; i < floorTotal; i++)//初始所有地板
        {
            RandomNumberBool[i] = false;
        }

        #region 創造柱子的方法
        int pillarQuantity = 0;//形成柱子的計數器

        while (pillarQuantity < labyrinthSize)//生成柱子的方法
        {
            int pollar = Random.Range(0, floorTotal);//隨機一個數

            if (!RandomNumberBool[pollar])//如果被隨機到的地板還沒變成柱子或敵人
            {
                #region 變成柱子
                floorList[pollar].localScale = Vector3.one + Vector3.up * wallHigh;
                floorList[pollar].position += Vector3.up * wallHigh / 2;
                RandomNumberBool[pollar] = true;
                #endregion
                pillarQuantity++;//次數+1次
            }
        }
        #endregion

        #region 創造怪物的方法
        int enemyQuantity = 0;//形成怪物的計數器
        float enemyCount = Mathf.Round(labyrinthSize * enemyGenerationSystem.enemyQuantity);//計算要生成幾個敵人

        while (enemyQuantity < enemyCount)//生成怪物的方法
        {
            int pollar = Random.Range(0, floorTotal);//隨機一個數

            if (!RandomNumberBool[pollar])//如果被隨機到的地板還沒變成柱子或敵人
            {
                #region 生成敵人的方法
                enemyGenerationSystem.EnemyBuilder(floorList[pollar].position, enemyQuantity);
                RandomNumberBool[pollar] = true;
                #endregion
                enemyQuantity++;//次數+1次
            }
        }
        #endregion
    }
}
