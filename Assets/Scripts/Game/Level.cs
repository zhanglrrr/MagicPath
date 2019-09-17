using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

///<summary>
///
///</summary>

public class Level : MonoBehaviour
{
    private static Color backgroundColor;
    
    private static Color skierColor;
    
    private static Color pineColor;
    
    private static Color feverColor;

    private Text txt_nowLevel;

    private Text txt_nextLevel;

    private Image middleBg;
    private Image rightBg;
    private Image left;
    private Image score;
    private Image bestScore;
    private Image fill;
    private Image fillBest;

    private float distance;

    private float percentage;

    private float lastPercentage;

    public float fillLength;


    private void Awake()
    {
        EventCenter.AddListener(EventType.SetNextLevelColors, SetNextLevelColors);
       // EventCenter.AddListener(EventType.SetDistance, SetDistance);
        txt_nowLevel = transform.Find("Left/Text").GetComponent<Text>();
        txt_nextLevel = transform.Find("Right/Text").GetComponent<Text>();
        middleBg = transform.Find("MiddleBg").GetComponent<Image>();
        rightBg = transform.Find("RightBg").GetComponent<Image>();
        left = transform.Find("Left").GetComponent<Image>();
        score = transform.Find("Middle/Score").GetComponent<Image>();
        bestScore = transform.Find("Middle/BestScore").GetComponent<Image>();
        fill = transform.Find("Middle/Fill").GetComponent<Image>();
        fillBest = transform.Find("Middle/FillBest").GetComponent<Image>();
        LoadColors();
        Init();
    }

    private void Start()
    {
        txt_nowLevel.text = GameManager.GetInstance().GetLevel().ToString();
        txt_nextLevel.text = (GameManager.GetInstance().GetLevel() + 1).ToString();
        SetLevelPanelColor();
        score.transform.localPosition = new Vector3(0, score.transform.localPosition.y, score.transform.localPosition.z);
        SetBestFill();
    }

    private void Update()
    {
        SetFill();
    }
    
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.SetNextLevelColors, SetNextLevelColors);
        //EventCenter.RemoveListener(EventType.SetDistance, SetDistance);
    }

    //private void SetDistance()
    //{
    //    distance = Skier.GetInitialSkierY() - FinishLine.Instance.GetFinishLineY();
    //}
    private void Init()
    {
        lastPercentage = 0;
        percentage = 0;
    }

    private void SetFill()
    {
        if (percentage > 1) return;

        float p = (Skier.GetInitialSkierY() - Skier.GetSkierY()) / (Skier.GetInitialSkierY() - FinishLine.Instance.GetFinishLineY());
        percentage = p <= 1 ? p : 1;//(distance - (Skier.GetSkierY() - FinishLine.Instance.GetFinishLineY())) / distance;

        fill.fillAmount = percentage;
        score.transform.localPosition = new Vector3(fillLength * percentage, score.transform.localPosition.y, score.transform.localPosition.z);

        score.GetComponentInChildren<Text>().text = ((int)(100 * percentage)).ToString() + "%";

        if (!GameManager.GetInstance().GetIsGameStart()) return;
        if (percentage - lastPercentage >= 0.005)
        {
            GameManager.GetInstance().SetFraction(GameManager.GetInstance().GetFraction() + 30);
            lastPercentage = percentage;
        }
    }

    private void SetBestFill()
    {
        float bs = GameManager.GetInstance().GetBestScore();
        if (bs > 0)
        {
            bestScore.transform.localPosition = new Vector3(fillLength * bs, bestScore.transform.localPosition.y, bestScore.transform.localPosition.z);
            fillBest.fillAmount = bs;
            bestScore.GetComponentInChildren<Text>().text = ((int)(100 * bs)).ToString() + "%";
        }
        else
            bestScore.gameObject.SetActive(false);
    }

    private void SetLevelPanelColor()
    {
        txt_nextLevel.color = Level.skierColor;
        middleBg.color = Level.skierColor;
        rightBg.color = Level.skierColor;
        left.color = Level.skierColor;
        score.color = Level.skierColor;
        fill.color = Level.skierColor;
    }

    public static Color GetBackgroundColor()
    {
        return Level.backgroundColor;
    }
    
    public static Color GetSkierColor()
    {
        return Level.skierColor;
    }
    
    public static Color GetPineColor()
    {
        return Level.pineColor;
    }
    
    public static Color GetFeverColor()
    {
        return Level.feverColor;
    }

    private void SetNextLevelColors()
    {
        int max = Parameters.COLOR_TEMPLATES.Length / 4;
        string text = Parameters.COLOR_TEMPLATES[UnityEngine.Random.Range(0, max) * 4];
        string text2 = Parameters.COLOR_TEMPLATES[UnityEngine.Random.Range(0, max) * 4 + 2];
        string text3 = text2;
        while (text3 == text2)
        {
            text3 = Parameters.COLOR_TEMPLATES[UnityEngine.Random.Range(0, max) * 4 + 1];
        }
        string text4 = text3;
        while (text4 == text3 || text4 == text2)
        {
            text4 = Parameters.COLOR_TEMPLATES[UnityEngine.Random.Range(0, max) * 4 + 3];
        }
        Level.backgroundColor = Utility.HexToColor(text);
        Level.skierColor = Utility.HexToColor(text2);
        Level.pineColor = Utility.HexToColor(text3);
        Level.feverColor = Utility.HexToColor(text4);
        LevelData.SaveString("liorefiuehfh", string.Format("{0};{1};{2};{3}", new object[]
        {
            text,
            text2,
            text3,
            text4
        }));
    }

    private static void LoadColors()
    {
        if (LevelData.HasKey("liorefiuehfh"))
        {
            string[] array = LevelData.LoadString("liorefiuehfh", null).Split(new char[]
            {
                ';'
            });
            Level.backgroundColor = Utility.HexToColor(array[0]);
            Level.skierColor = Utility.HexToColor(array[1]);
            Level.pineColor = Utility.HexToColor(array[2]);
            Level.feverColor = Utility.HexToColor(array[3]);
        }
        else
        {
            Level.backgroundColor = Utility.HexToColor(Parameters.COLOR_TEMPLATES[0]);
            Level.skierColor = Utility.HexToColor(Parameters.COLOR_TEMPLATES[2]);
            Level.pineColor = Utility.HexToColor(Parameters.COLOR_TEMPLATES[1]);
            Level.feverColor = Utility.HexToColor(Parameters.COLOR_TEMPLATES[3]);
        }
    }

}
