using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>

public class Encouragement : MonoBehaviour
{
   public static Encouragement Instance;
    private Text txt;
    private float timer;

    private void Awake()
    {
        EventCenter.AddListener<bool>(EventType.SetEncourageActive, SetEncourageActive);
        Instance = this;
        txt = GetComponentInChildren<Text>();
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
    }

    private void Start()
    {

    }

    private void OnDestroy()
    {

        EventCenter.RemoveListener<bool>(EventType.SetEncourageActive, SetEncourageActive);
    }

    private void Update()
    {
        if (this.timer < 1f)
        {
            this.timer += 2f * Time.deltaTime;
            if (this.timer > 1f)
            {
                this.timer = 1f;
            }
            float num = 4f * this.timer - 3f;
            num = (9f - num * num) * 0.1f;
            this.txt.transform.localScale = new Vector3(num, num, num);
        }
        else if (this.timer < 2f)
        {
            this.timer += Time.deltaTime;
        }
        else
        {
            this.timer += 2f * Time.deltaTime;
            if (this.timer > 3f)
            {
                this.timer = 3f;
                base.enabled = false;
            }
            Color color = this.txt.color;
            color.a = 3f - this.timer;
            this.txt.color = color;
        }
    }

    private void SetEncourageActive(bool b)
    {
        gameObject.SetActive(b);
    }

    public void Play(Goodness goodness)
    {
        string[] array = this.wordings[goodness];
        this.txt.text = array[UnityEngine.Random.Range(0, array.Length)];
        this.timer = 0f;
        base.enabled = true;
        Color color = this.txt.color;
        color.a = 1f;
        this.txt.color = color;
    }

    public void Whoosh()
    {
        int whooshCombo = GameManager.GetInstance().GetBonusNum();
        if (whooshCombo > 6)
        {
            Play(Goodness.Perfect);
        }
        else if (whooshCombo > 3)
        {
            Play(Goodness.Exquisite);
        }
        else if (whooshCombo > 2)
        {
            Play(Goodness.Awesome);
        }
        else if (whooshCombo > 1)
        {
            Play(Goodness.Good);
        }
    }

    private readonly Dictionary<Goodness, string[]> wordings = new Dictionary<Goodness, string[]>
    {
        {
            Goodness.Good,
            new string[]
            {
                "Good job!",
                "Yeah!",
                "Smooth!"
            }
        },
        {
            Goodness.Awesome,
            new string[]
            {
                "Awesome!",
                "Wow!",
                "Chilly!"
            }
        },
        {
            Goodness.Exquisite,
            new string[]
            {
                "What a style!",
                "Incredible!",
                "Amazing!",
                "Exquisite!"
            }
        },
        {
            Goodness.Perfect,
            new string[]
            {
                "King of the mountain!",
                "Unstoppable!",
                "Mountain is yours!",
                "Chilly Snow master!",
                "You're on fire!"
            }
        }
    };

    private readonly Dictionary<Goodness, string[]> wordcs = new Dictionary<Goodness, string[]>
    {
        {
            Goodness.Good,
            new string[]
            {
                "骨灰拌饭!",
                "灵车漂移!",
                "坟头蹦迪"
            }
        },
        {
            Goodness.Awesome,
            new string[]
            {
                "寿衣走秀!",
                "骨灰拌饭!",
                "病房K歌!"
            }
        },
        {
            Goodness.Exquisite,
            new string[]
            {
                "葬礼庆典!",
                "灵堂拍片!",
                "送葬摇滚!",
                "棺材冲浪!"
            }
        },
        {
            Goodness.Perfect,
            new string[]
            {
                "丧宴烤尸!",
                "脑浆浇花!",
                "骨髓煮汤!",
                "灵堂酒会!",
                "尸块养猪!"
            }
        }
    };

    public enum Goodness
    {
        Good,
       
        Awesome,
       
        Exquisite,
      
        Perfect
    }
}
