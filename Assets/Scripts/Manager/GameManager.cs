using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

///<summary>
///
///</summary>

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private bool isGameStart;
    public bool canRestart;//是否可以重启游戏，初值true，刚死亡设为false
    private bool isPressing;
    private bool isCameraFollow;
    private int bonusNum;
    private float bonusTime;
    private float bonusFraction;
    private bool isGameOver;
    private Queue<GameObject> queuePine;
    private bool isAdsReady;
    private bool isCrazy;
    private List<GameObject> rollStones;

    private Data data;
    private int level;
    private bool isFirstGame;
    private float bestScore;
    private float bestFraction;
    private float fraction;
    private bool isAds;
    private bool isVoice;
    private bool isEncourage;

    public static GameManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        EventCenter.AddListener(EventType.SetMute, SetMute);
        if (_instance != null)
        {
            Destroy(gameObject);//销毁新创建的go而保留第一个go
            
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);//防止重新加载场景时销毁
        Init();
        InitData();
        SetMute();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.SetMute, SetMute);
    }

    private void Update()
    {
        bonusTime -= Time.deltaTime;
        if(bonusTime <= 0) 
        {
            bonusNum = 0;
            bonusFraction = 30;
        }
    }

    private void Init()
    {
        canRestart = true;
        isGameStart = false;
        isPressing = false;
        isCameraFollow = true;
        bonusNum = 0;
        bonusTime = 4;
        bonusFraction = 30;
        fraction = 0;
        queuePine = new Queue<GameObject>();
        isAdsReady = false;
        rollStones = new List<GameObject>();
    }
    
    private void InitData()
    {
        Read();

        if (data == null)
        {
            isFirstGame = true;
        }
        else
        {
            isFirstGame = data.IsFirstGame;
        }
        if (isFirstGame)
        {
            isFirstGame = false;
            level = 1;
            bestScore = 0;
            bestFraction = 0;
            isCrazy = false;
            isVoice = true;
            isEncourage = true;
            isAds = true;

            data = new Data();
            Save();
        }
       else
        {
            isFirstGame = data.IsFirstGame;
            level = data.Level;
            bestScore = data.BestScore;
            bestFraction = data.BestFraction;
            isCrazy = data.IsCrazy;
            isVoice = data.IsVoice;
            isEncourage = data.IsEncourage;
            isAds = data.IsAds;
        }
    }

    ///<summary>
    ///读档
    ///</summary>
    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data", FileMode.Open))
            {
                data = (Data)bf.Deserialize(fs);
                level = data.Level;
                isFirstGame = data.IsFirstGame;
                bestScore = data.BestScore;
                bestFraction = data.BestFraction;
                isCrazy = data.IsCrazy;
                isVoice = data.IsVoice;
                isEncourage = data.IsEncourage;
                isAds = data.IsAds;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    ///<summary>
    ///保存
    ///</summary>
    public void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                data.Level = level;
                data.IsFirstGame = isFirstGame;
                data.BestScore = bestScore;
                data.BestFraction = bestFraction;
                data.IsCrazy = isCrazy;
                bf.Serialize(fs, data);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    private void SetMute()
    {
        transform.GetComponent<AudioSource>().mute =!GetIsVoice();
    }

    public float GetBestScore()
    {
        return bestScore;
    }

    public void SetBestScore(float bestScore)
    {
        this.bestScore = bestScore;
    }

    public void SetIsCameraFollow( bool b)
    {
        isCameraFollow = b;
    }

    public bool GetIsCameraFollow()
    {
        return isCameraFollow;
    }

    public void SetIsPressing(bool b)
    {
        isPressing = b;
    }

    public bool GetIsPressing()
    {
        return isPressing;
    }

    public void SetIsGameStart(bool b)
    {
        isGameStart = b;
    }

    public bool GetIsGameStart()
    {
        return isGameStart;
    }

    public void SetCanRestart(bool b)
    {
        canRestart = b;
    }

    public bool GetCanRestart()
    {
        return canRestart;
    }

    public  int GetLevel()
    {
        return level;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }
    
    public void AddBonusNum()
    {
        bonusNum++;
        fraction += bonusFraction;
        bonusFraction += 30;
        bonusTime = 3;
    }

    public int GetBonusNum()
    {
        return bonusNum;
    }

    public void SetBonusNum(int bonus)
    {
        bonusNum = bonus;
    }

    public float GetBounsFraction()
    {
        return bonusFraction;//擦边得分
    }

    public void SetBonusFraction(int bonus)
    {
        bonusFraction = bonus;
    }

    public float GetFraction()
    {
        return fraction;//当前的分
    }

    public void SetFraction(float bonus)
    {
        fraction = bonus;
    }

    public float GetBestFraction()
    {
        return bestFraction;//当前的分
    }

    public void SetBestFraction(float bonus)
    {
        bestFraction = bonus;
    }

    public bool GetIsGameOver()
    {
        return isGameOver;//当前的分
    }

    public void SetIsGameOver(bool b)
    {
        isGameOver = b;
    }

    public GameObject DePine()
    {
        return queuePine.Dequeue();
    }

    public void EnPine(GameObject go)
    {
        queuePine.Enqueue(go);
    }

    public int GetPineNum()
    {
        return queuePine.Count;
    }

    public void ClearPine()
    {
        queuePine.Clear();
    }

    public bool GetIsAdsReady()
    {
        return isAdsReady;//当前的分
    }

    public void SetIsAdsReady(bool b)
    {
        isAdsReady = b;
    }

    public bool GetIsCrazy()
    {
        return isCrazy;//当前的分
    }

    public void SetIsCrazy(bool b)
    {
        isCrazy = b;
    }

    public void AddRollStones(GameObject go)
    {
        rollStones.Add(go);
    }

    public void ClearRollStones()
    {
        foreach (GameObject go in rollStones)
            Destroy(go);
    }

    public bool GetIsAds()
    {
        return isAds;//当前的分
    }

    public void SetIsAds(bool b)
    {
        isAds = b;
    }

    public bool GetIsEn()
    {
        return isEncourage;//当前的分
    }

    public void SetIsEn(bool b)
    {
        isEncourage = b;
    }

    public bool GetIsVoice()
    {
        return isVoice;//当前的分
    }

    public void SetIsVoice(bool b)
    {
       isVoice = b;
    }
}
