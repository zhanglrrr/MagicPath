using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

///<summary>
///
///</summary>

public class Skier : MonoBehaviour
{
    #region 平滑加速属性
    public Vector3 velocity = Vector3.zero;
    public Vector3 deltaMove = Vector3.zero;
    public float speedY = 3;
    public float speedX = 2.6f;
    public float accelerationY = 1.0f;
    public float accelerationX = 1.0f;
    public float pressSpeed = 4.5f;
    public float envSpeed =0.5f;
    float envSpeedAdd = 0.5f;
    float xInput = 0;
    float yInput = 0;
    #endregion

    #region 组件属性
    private ParticleSystem astral;
    private ParticleSystem dots;
    private ParticleSystem death;
    private ParticleSystem power;
    private TrailRenderer trail;
    private SpriteRenderer shadow;
    private SpriteRenderer ball;
    #endregion

    #region 其他属性

    ///<summary>
    ///是否向左移动
    ///</summary>
    private bool isMoveLeft;
    private bool isPressTurn;
    private static float skierY;
    private bool isAfterVictory;
    public static float initialSkierY;
    public float outInitialSkierY;
    private float per;
    private bool isDotsPlaying;
    private bool isPause;
    private bool isShowAdsPanel;
    private bool isAdsFinished;
    private GameObject goPine;
    private readonly ParticleSystem.MinMaxCurve notEmitting = new ParticleSystem.MinMaxCurve(0f);
    private readonly ParticleSystem.MinMaxCurve doEmitting = new ParticleSystem.MinMaxCurve(100f);
    private readonly ParticleSystem.MinMaxCurve doEmittingAs = new ParticleSystem.MinMaxCurve(25f);
    private readonly ParticleSystem.MinMaxCurve doEmittingDots = new ParticleSystem.MinMaxCurve(50f);

    private AudioSource adDeath;
    private AudioSource adSki;
    private AudioSource adPerfect;
    #endregion

    private void Awake()
    {
        AddEvent();
        InitComponent();
        isPressTurn = false;
        isAfterVictory = false;
        initialSkierY = outInitialSkierY;
        per = 0;
        isDotsPlaying = false;
        goPine = null;
        isPause = false;
        isShowAdsPanel = false;
        isAdsFinished = false;
    }
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, initialSkierY, transform.position.z);
        SetSkierColor(Level.GetSkierColor());
    }

    void Update()
    {
        //SetPine();//动态设置树木active

        SetDots();

        skierY = transform.position.y;

        if (!GameManager.GetInstance().GetIsGameStart() && !isAfterVictory) return;

        if (trail.time < 5) trail.time += Time.deltaTime;//处理拖尾特效的持续时间变化

        if (GameManager.GetInstance().GetIsPressing())
        {
            //PlayPower();

            speedX = pressSpeed; //4.5f;
        }
        else
        {
            speedX = 2.6f;

            //StopPower();
        }

        #region Code:平滑加速输入

        if (!isPause)
        {
            if (isMoveLeft)
                xInput = -1;
            else
                xInput = 1;
        }

        float deltatime = Time.deltaTime;
        HandleKeyInput(deltatime);
        AddEnvSpeed();
        #endregion

        //#region 滑雪音效
        //this.UpdatePowderSpreadSound();
        //#endregion


        
        ///<summary>
        /// void LateUpdate()
        ///</summary>
        
        #region Code:平滑加速移动

        deltatime = Time.deltaTime;
        UpdateHorForce(deltatime);

        UpdateMovement(deltatime);
        #endregion

        if (transform.position.x > 2 || transform.position.x < -2)
        {
            Vector3 p = Camera.main.WorldToViewportPoint(transform.position);

            if (!isAfterVictory && (p.x > 1 || p.x < 0))//判断是否两侧出界
            {
                bool b = true;
                if (Ads.Instance.GetIsAdvertisementReady() && !isShowAdsPanel)// (GameManager.GetInstance().GetIsAdsReady())
                {
                    b = false;
                    StartCoroutine(AdsPanel());
                }
                DoDeath(b);
            }
        }

    }
    //void LateUpdate()
    //{
    //    if (!GameManager.GetInstance().GetIsGameStart() && !isAfterVictory) return;

    //    #region Code:平滑加速移动

    //    float deltatime = Time.deltaTime;
    //    UpdateHorForce(deltatime);

    //    UpdateMovement(deltatime);
    //    #endregion

    //    if (transform.position.x > 2 || transform.position.x < -2)
    //    {
    //        Vector3 p = Camera.main.WorldToViewportPoint(transform.position);

    //        if (!isAfterVictory && (p.x > 1 || p.x < 0))//判断是否两侧出界
    //        {
    //            GameOver();
    //        }
    //    }

    //}

    private void OnDestroy()
    {
        RemoveEvent();
    }

    //回收Pine
    private void SetPine()
    {
        if (GameManager.GetInstance().GetPineNum() <= 0) return;
        if (goPine != null)
        {
            if (goPine.transform.position.y + 10 >= transform.position.y)
            {
                goPine.SetActive(true);
                StartCoroutine(Dealy1(goPine));
                goPine = null;
            }
            return;
        }
        goPine = GameManager.GetInstance().DePine();
    }

    IEnumerator Dealy1(GameObject go)
    {
        yield return new WaitUntil(GameManager.GetInstance().GetIsGameStart);
        yield return new WaitForSeconds(8);
       Destroy(go);
    }

    ///<summary>
    /// 滑雪音效
    ///</summary>
    private void UpdatePowderSpreadSound(bool b)
    {
        adSki.volume = 0.025f;
        if (b /*|| GameManager.GetInstance().GetIsPressing()*/)
        {
            if (adSki.volume < 0.1f)
                adSki.volume += 5f * Time.deltaTime;
            if (adSki.volume > 0.1f)
            {
                adSki.volume = 0.1f;
            }
        }
        else if (adSki.volume > 0.025f)
        {
            adSki.volume -= 0.05f * Time.deltaTime;
            if (adSki.volume <= 0.025f)
            {
                adSki.volume = 0.025f;
            }
        }

        if (!GameManager.GetInstance().GetIsVoice())
        {
            adSki.volume = 0;
        }
    }

    private void SetSkierColor(Color c)
    {
        ball.color = c;
        ParticleSystem.MainModule mainModule = astral.main;
        mainModule.startColor = c;
        mainModule = dots.main;
        mainModule.startColor = c;
        mainModule = death.main;
        mainModule.startColor = c;

        c.a = 0.39f;
        this.trail.startColor = c;
        c.a = 0f;
        this.trail.endColor = c;
    }

    private void AddEvent()
    {
        EventCenter.AddListener(EventType.GameStart, GameStart);
        EventCenter.AddListener(EventType.GameTouch, GameTouch);
        EventCenter.AddListener<bool>(EventType.SetSkierPower, SetSkierPower);
        EventCenter.AddListener(EventType.SkierVictory, Victory);
        EventCenter.AddListener(EventType.DoAfterAds, DoAfterAds);
        EventCenter.AddListener(EventType.GameOver, GameOver);
    }

    private void RemoveEvent()
    {
        EventCenter.RemoveListener(EventType.GameStart, GameStart);
        EventCenter.RemoveListener(EventType.GameTouch, GameTouch);
        EventCenter.RemoveListener<bool>(EventType.SetSkierPower, SetSkierPower);
        EventCenter.RemoveListener(EventType.SkierVictory, Victory);
        EventCenter.RemoveListener(EventType.DoAfterAds, DoAfterAds);
        EventCenter.RemoveListener(EventType.GameOver, GameOver);
    }

    private void SetPower()
    {
        if(isMoveLeft)
        {
            this.power.transform.eulerAngles = new Vector3(0f, 0f, -50f);
        }
        else
        {
            this.power.transform.eulerAngles = new Vector3(0f, 0f, 50f);
        }
    }

    private void PlayPower()
    {
        ParticleSystem.EmissionModule mainModule = this.power.emission;
        mainModule.rateOverTime = this.doEmitting;
    }

    private void StopPower()
    {
        ParticleSystem.EmissionModule mainModule = this.power.emission;
        mainModule.rateOverTime = this.notEmitting;
    }
    

    private void PlayDots()
    {
        if (isDotsPlaying) return;
        ParticleSystem.EmissionModule mainModule = this.dots.emission;
        mainModule.rateOverTime = doEmittingDots;
        mainModule = this.astral.emission;
        mainModule.rateOverTime = doEmittingAs;
        if(GameManager.GetInstance().GetIsVoice())
            adPerfect.Play();
        isDotsPlaying = true;
    }

    private void StopDots()
    {
        ParticleSystem.EmissionModule mainModule = this.dots.emission;
        mainModule.rateOverTime = this.notEmitting;
        mainModule = this.astral.emission;
        mainModule.rateOverTime = this.notEmitting;
        adPerfect.Stop();
        isDotsPlaying = false;
    }

    private void SetDots()
    {
        if (GameManager.GetInstance().GetBonusNum() >= 4)
        {
            PlayDots();
            SetSkierColor(Level.GetFeverColor());
        }
        else
        {
            StopDots();
            SetSkierColor(Level.GetSkierColor());
        }
    }

    private void SetSkierPower(bool b)
    {
        if (b) PlayPower();
        else StopPower();
        UpdatePowderSpreadSound(b);
    }

    public static float GetSkierY()
    {
        return  skierY;
    }

    public static float GetInitialSkierY()
    {
        return initialSkierY;
    }

    ///<summary>
    ///游戏中Touch
    ///</summary>
    private void GameTouch()
    {
        isMoveLeft = !isMoveLeft;
        SetPower();
    }

    ///<summary>
    ///游戏开始属性设置
    ///</summary>
    private void GameStart()
    {
        GameManager.GetInstance().SetIsGameStart(true);
        yInput = -1;
        
        astral.Play();//播放星芒特效
        dots.Play();//播放烟雾特效
        adSki.Play();
    }

    ///<summary>
    ///游戏结束属性设置
    ///</summary>
    private void GameOver()
    {
       // if (!GameManager.GetInstance().GetIsGameStart()) return;
        
        float now = (Skier.GetInitialSkierY() - Skier.GetSkierY()) / (Skier.GetInitialSkierY() - FinishLine.Instance.GetFinishLineY());
        if(now > GameManager.GetInstance().GetBestScore())
        {
            GameManager.GetInstance().SetBestScore(now);
        }
        float fraction = GameManager.GetInstance().GetFraction();
        float bestFraction = GameManager.GetInstance().GetBestFraction();
        if (fraction> bestFraction)
        {
            GameManager.GetInstance().SetBestFraction(fraction);
        }
        GameManager.GetInstance().Save();
        GameManager.GetInstance().SetFraction(0);

        
        GameManager.GetInstance().SetIsGameStart(false);
        GameManager.GetInstance().SetCanRestart(false);//刚死亡暂时不可以重开，由系统机制决定何时重开
        //GameManager.GetInstance().SetBonusNum(0);
        //death.Play();
        //adDeath.Play();
        //adSki.Stop();
        //StopPower();
        //ball.enabled = false;
        //shadow.enabled = false;
        //dots.Stop();
        //xInput = 0;
        //yInput = 0;
        //velocity = Vector3.zero;
        StartCoroutine(Dealy());
    }

    IEnumerator Dealy()
    {
        float wait = 1;
        if(isShowAdsPanel && !isAdsFinished)
            wait = 0;
        yield return new WaitForSeconds(wait);

        //相机平滑移动，淡入淡出场景
        GameManager.GetInstance().SetIsGameOver(true);
        yield return new WaitForSeconds(1f);
        GameManager.GetInstance().SetIsGameOver(false);


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.GetInstance().SetCanRestart(true);
        GameManager.GetInstance().SetIsCameraFollow(true);
        GameManager.GetInstance().ClearPine();
        GameManager.GetInstance().SetIsAdsReady(false);
    }

    private void Victory()
    {
        isAfterVictory = true;
        GameManager.GetInstance().SetIsGameStart(false);
        GameManager.GetInstance().SetCanRestart(false);

        StartCoroutine(AfterVictory());
        adSki.Stop();
    }

    IEnumerator AfterVictory()
    {
        for (int i = 0; i < 8; i++)
        {
            GameTouch();
            yield return new WaitForSeconds(0.5f - 0.05f * i);
        }
    }

    ///<summary>
    ///初始化组件属性
    ///</summary>
    private void InitComponent()
    {
        trail = GameObject.Find("Skier").GetComponent<TrailRenderer>();
        trail.time = 0;

        astral = GameObject.Find("Skier/AstralPS").GetComponent<ParticleSystem>();
        dots = GameObject.Find("Skier/DotsPS").GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule mainModule = this.dots.emission;
        mainModule.rateOverTime = this.notEmitting;
        death = GameObject.Find("Skier/DeathPS").GetComponent<ParticleSystem>();
        ball = transform.GetComponent<SpriteRenderer>();
        shadow = transform.Find("BallShadow").GetComponent<SpriteRenderer>();
        power = transform.Find("PowerPS").GetComponent<ParticleSystem>();

        adDeath = GameObject.Find("Skier/DeathPS").GetComponent<AudioSource>();
        adSki = transform.GetComponent<AudioSource>();
        adPerfect = transform.Find("AstralPS").GetComponent<AudioSource>();
    }

    ///<summary>
    ///擦边得分
    ///</summary>
    //private void OnTriggerEnter2D(Collider2D collider2D)
    //{
    //    if (!GameManager.GetInstance().GetIsGameStart()) return;

    //    if (collider2D.tag == "PineCollider")
    //    {
    //        Transform pineBody = collider2D.transform.parent.GetChild(0).transform;
    //        Vector3 tmp = pineBody.localScale;
    //        pineBody.DOScale(1.5f * pineBody.localScale, 0.1f).OnComplete(() =>
    //        {
    //            pineBody.DOScale(tmp, 0.1f);
    //        });
    //    }
    //}

    ///<summary>
    ///产生碰撞
    ///</summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAfterVictory && (collision.collider.tag == "PineCollider" || collision.collider.tag == "RollingStoneCollider"))
        {
            bool b = true;
            if(Ads.Instance.GetIsAdvertisementReady() && !isShowAdsPanel)// (GameManager.GetInstance().GetIsAdsReady())
            {
                b = false;
                StartCoroutine(AdsPanel());
            }
            DoDeath(b);
            //GameOver();
        }
    }

    IEnumerator AdsPanel()
    {
        yield return new WaitForSeconds(1f);
        isShowAdsPanel = true;
        EventCenter.Broadcast(EventType.ShowAdsPanel);
    }

    private void DoDeath(bool b)
    {
        GameManager.GetInstance().SetIsGameStart(false);
        GameManager.GetInstance().SetCanRestart(false);//刚死亡暂时不可以重开，由系统机制决定何时重开
        GameManager.GetInstance().SetBonusNum(0);
        death.Play();
        if (GameManager.GetInstance().GetIsVoice())
            adDeath.Play();
        adSki.Stop();
        StopPower();
        ball.enabled = false;
        shadow.enabled = false;
        dots.Stop();
        xInput = 0;
        yInput = 0;
        isPause = true;
        velocity = Vector3.zero;

        if (b) GameOver();

        //StartCoroutine(SS());
    }

    private void  DoAfterAds()
    {
        transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        ball.enabled = true;
        shadow.enabled = true;
        isMoveLeft = false;
        isPause = false;
        trail.time = 0;
        isAdsFinished = true;
        GameManager.GetInstance().SetCanRestart(true);
    }

    #region Code:平滑加速方法
    void UpdateMovement(float deltatime)
    {
        deltaMove = velocity * deltatime;

        gameObject.transform.Translate(deltaMove, Space.World);

    }
    void HandleKeyInput(float deltatime)
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xInput = -1.0f;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            xInput = 1.0f;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            yInput = 1.0f;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            yInput = -1.0f;
        }

    }
    void UpdateHorForce(float deltatime)
    {
        float val = 0;
        if (Mathf.Abs(velocity.x) > 2.6f)
            isPressTurn = true;
        else if(Mathf.Abs(velocity.x) < 1f)
            isPressTurn = false;
        if (velocity.x / xInput < 0 && isPressTurn)
            val = Mathf.Lerp(velocity.x, speedX * xInput, 0.5f * deltatime * accelerationX);
        else
            val = Mathf.Lerp(velocity.x, speedX * xInput, deltatime * accelerationX);

        //if (Mathf.Abs(val) > maxSpeedX) val = xInput * maxSpeedX;
        SetForceX(val);
        val = Mathf.Lerp(velocity.y, (speedY+ envSpeedAdd) * yInput, deltatime * accelerationY);
        SetForceY(val);
    }
    public void AddEnvSpeed()
    {
        per = (Skier.GetInitialSkierY() - Skier.GetSkierY()) / (Skier.GetInitialSkierY() - FinishLine.Instance.GetFinishLineY());
        if (per >0)
            envSpeedAdd = envSpeed * (per);
    }
    public void SetForceX(float x)
    {
        velocity.x = x;
    }
    public void SetForceY(float y)
    {
        velocity.y = y;
    }
    #endregion
}

