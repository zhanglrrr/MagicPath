using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

///<summary>
///
///</summary>

public class PineSpawner : MonoBehaviour
{
    //① 指定范围&随机位置
    private int col = 600;
    private int row = 40;                                                          //首先定义地图总行数和列数
    private int pineNum = 300;
    private int x, y;
    private float xMultiple = 0.233f;
    private float yMultiple = 0.2f;
    private int level;


    private List<List<Vector2>> lstX = new List<List<Vector2>>();
    private List<Vector2> lstV = new List<Vector2>();

    private Transform finishLine;

    private void Awake()
    {
        EventCenter.AddListener(EventType.GenerateRollingStone, GenerateRollingStone);
        level = GameManager.GetInstance().GetLevel();
        col = 200 + 14 * level; // 600 + 7 * level;
        pineNum = 130 + 10 * level; //400 + 5 * level;
        finishLine = GameObject.Find("FinishLine").transform;
    }
    private void Start()
    {
        SetList();
        InstantiateItems(pineNum);
        GenerateRollingStone();

        finishLine.position = new Vector3(finishLine.position.x, -col * yMultiple - 5, finishLine.position.z);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.GenerateRollingStone, GenerateRollingStone);
    }

    private void GenerateRollingStone()
    {
        int maxNum = 0;
        if(GameManager.GetInstance().GetIsCrazy())
            maxNum = (int)10 + 2 * level + 1;
        else
            maxNum = level / 4 + 1;
        int ran = Random.Range(maxNum/2, maxNum + 1);
        for (int i = 1; i <= ran; i++)
        {
            int maxDistance = (int)(9 * col / 10);
            int minDistance = (int)(col / 10);
            float distance = Random.Range(minDistance, maxDistance);
            float y = 0 - yMultiple * distance;
            bool isRight = Random.Range(0, 2) == 0 ?   true : false;
            GameObject go = (GameObject) Instantiate(Resources.Load("Prefabs/RollingStone"));
            GameManager.GetInstance().AddRollStones(go);
            go.transform.position = new Vector3(6, y, 15);
            if (!isRight)
            {
                go.transform.position = new Vector3(-6, y, 15);
                float angle = go.transform. GetChild(2).transform.rotation.eulerAngles.z;//右315，左45
                go.transform.GetChild(2).transform.rotation = Quaternion.Euler(Vector3.forward * (angle + 90));
            }
        }
    }

    private void SetList()
    {
        for (y = 0; y < col; y++)
        {                                              //遍历需要生成图块的所有坐标，存放到List中。
            List<Vector2> vector2s = new List<Vector2>();
            for (x = 0; x < row; x++)
            {
                Vector2 v2 = new Vector2(xMultiple * x - 4.4f, (y - col) * yMultiple - 3);
                if(v2.x<-0.2 || v2.x>0.5)
                    vector2s.Add(v2);
            }
            lstX.Add(vector2s);
        }
    }

    //取得随机位置的方法
    private Vector2 randomPosition()
    {
        int index = Random.Range(0, lstX.Count);//在List中随机取得
        List<Vector2> positionList = lstX[index];
        int positionIndex = Random.Range(0, positionList.Count);//在List中随机取得
        Vector2 pos = positionList[positionIndex];
        lstX.RemoveAt(index);  //取过的位置从List中移除
        return pos;
    }
    private void InstantiateItems(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = randomPosition();//调用方法，取随机位置；
            this.lstV.Add(pos);
        }
        List<Vector2> lstV = this.lstV.OrderByDescending(a => a.y).ToList();
        foreach (Vector2 pos in lstV)
        {
            GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/Pine"), pos, Quaternion.identity) as GameObject;
            //GameManager.GetInstance().EnPine(go);
            //go.SetActive(false);
            int ran = Random.Range(0, 10);
            float scale = 0.5f;//标准尺寸
            switch (ran)
            {
                case 0:
                    scale = 0.4f;
                    break;
                case 1:
                    scale = 0.5f;
                    break;
                case 2:
                    scale = 0.45f;
                    break;
                case 3:
                    scale = 0.55f;
                    break;
                case 4:
                    scale = 0.35f;
                    break;
                default:
                    break;
            }
            float radius = go.transform.GetComponent<CircleCollider2D>().radius/2;//根据标准尺寸设置collider半径
            go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, 10);
            go.transform.localScale = new Vector3(scale * go.transform.localScale.x, scale * go.transform.localScale.y);
            
            go.transform.GetComponent<CircleCollider2D>().radius = radius/scale;

            go.transform.GetChild(0).GetChild(4).localScale = new Vector3(0.7f/ scale,0.7f/ scale,0.7f/ scale);
        }
    }
}
