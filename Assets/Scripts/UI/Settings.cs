using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public sealed class Settings : MonoBehaviour
{
    public static Settings GetInstance()
    {
        return Settings.instance;
    }
    
    private void Awake()
    {
        EventCenter.AddListener(EventType.FadeMode, Hide);
        Settings.instance = this;
        this.settingsButton = base.transform.GetChild(1).GetComponent<Button>();
        this.settingsButton.onClick.AddListener(new UnityAction(this.OnClick));
        this.bar = base.transform.GetChild(0).GetChild(0).GetComponent<CanvasGroup>();
        this.barTransform = this.bar.GetComponent<RectTransform>();
        this.Show();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.FadeMode, Hide);
    }

    private void OnClick()
    {
        this.shouldExpand = !this.shouldExpand;
        this.bar.interactable = this.shouldExpand;
        this.bar.blocksRaycasts = this.shouldExpand;
    }
    
    public void Show()
    {
        this.settingsButton.enabled = true;
        this.shouldShow = true;
        base.enabled = true;
        this.bar.alpha = 1f;
        this.expandSpeed = 10f;
    }
    
    public void Hide()
    {
        if (!this.settingsButton.enabled)
        {
            return;
        }
        this.settingsButton.enabled = false;
        this.shouldShow = false;
        if (this.shouldExpand)
        {
            this.shouldExpand = false;
            this.bar.blocksRaycasts = false;
            this.bar.interactable = false;
        }
        this.expandSpeed = 20f;
    }
    
    private void Update()
    {
        if (this.shouldExpand)
        {
            Vector2 anchoredPosition = this.barTransform.anchoredPosition;
            anchoredPosition.y += (-15f - this.barTransform.sizeDelta.y - anchoredPosition.y) * this.expandSpeed * Time.deltaTime;
            this.barTransform.anchoredPosition = anchoredPosition;
        }
        else
        {
            Vector2 anchoredPosition2 = this.barTransform.anchoredPosition;
            anchoredPosition2.y += (-15f - anchoredPosition2.y) * this.expandSpeed * Time.deltaTime;
            this.barTransform.anchoredPosition = anchoredPosition2;
        }
        if (this.shouldShow)
        {
            if (this.timer < 1f)
            {
                this.timer += this.animationSpeed * Time.deltaTime;
                if (this.timer > 1f)
                {
                    this.timer = 1f;
                }
                float num = this.OneMinusSinCardNormalized(this.timer);
                this.settingsButton.transform.localScale = new Vector3(num, num, num);
            }
        }
        else
        {
            this.timer -= this.animationSpeed * Time.deltaTime;
            if (this.timer < 0f)
            {
                this.timer = 0f;
                this.bar.alpha = 0f;
                base.enabled = false;
            }
            float num2 = this.OneMinusSinCardNormalized(this.timer);
            this.settingsButton.transform.localScale = new Vector3(num2, num2, num2);
        }
    }
    
    private float OneMinusSinCardNormalized(float x)
    {
        if (x == 0f)
        {
            return 0f;
        }
        return 1f - Mathf.Sin(x * 10f) * (0.1f / x - 0.1f);
    }
    
    private static Settings instance;
    
    private Button settingsButton;
    
    private CanvasGroup bar;
    
    private RectTransform barTransform;
    
    private bool shouldShow;
    
    private float timer;
    
    [SerializeField]
    private float animationSpeed = 2f;
    
    private bool shouldExpand;
    
    private float expandSpeed;
}
