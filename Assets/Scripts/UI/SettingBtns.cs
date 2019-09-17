using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingBtns : MonoBehaviour
{
    private Button btnV, btnA, btnE;
    private Image imgA, imgV, imgE;

    private void Awake()
    {
        btnA = transform.GetChild(0).GetComponent<Button>();
        btnA.onClick.AddListener(OnclickBtnA);
        btnV = transform.GetChild(1).GetComponent<Button>();
        btnV.onClick.AddListener(OnclickBtnV);
        btnE = transform.GetChild(2).GetComponent<Button>();
        btnE.onClick.AddListener(OnclickBtnE);

        imgA = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        imgV = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        imgE = transform.GetChild(2).GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        if (GameManager.GetInstance().GetIsAds())
        {
            imgA.color = new Color(255, 255, 255, 1);
        }
        else
        {
            imgA.color = new Color(255, 255, 0, 1);
        }

        if (GameManager.GetInstance().GetIsVoice())
        {
            imgV.sprite = Resources.Load("Img/soundOn", typeof(Sprite)) as Sprite;
        }
        else
        {
            imgV.sprite = Resources.Load("Img/soundOff", typeof(Sprite)) as Sprite;
        }

        if (GameManager.GetInstance().GetIsEn())
        {
            imgE.sprite = Resources.Load("Img/motivationalon", typeof(Sprite)) as Sprite;
            EventCenter.Broadcast(EventType.SetEncourageActive, true);
        }
        else
        {
            imgE.sprite = Resources.Load("Img/motivationaloff", typeof(Sprite)) as Sprite;
            EventCenter.Broadcast(EventType.SetEncourageActive, false);
        }
    }

    private void OnclickBtnA()
    {
        GameManager.GetInstance().SetIsAds(!GameManager.GetInstance().GetIsAds());
        if (GameManager.GetInstance().GetIsAds())
        {
            imgA.color = new Color(255, 255, 255, 1);
        }
        else
        {
            imgA.color = new Color(255, 255, 0, 1);
        }
        GameManager.GetInstance().Save();
    }
    private void OnclickBtnV()
    {
        GameManager.GetInstance().SetIsVoice(!GameManager.GetInstance().GetIsVoice());
        EventCenter.Broadcast(EventType.SetMute);
        if(GameManager.GetInstance().GetIsVoice())
        {
            imgV.sprite = Resources.Load("Img/soundOn", typeof(Sprite)) as Sprite;
        }
        else
        {
            imgV.sprite = Resources.Load("Img/soundOff", typeof(Sprite)) as Sprite;
        }
        GameManager.GetInstance().Save();
    }
    private void OnclickBtnE()
    {
        GameManager.GetInstance().SetIsEn(!GameManager.GetInstance().GetIsEn());

        if (GameManager.GetInstance().GetIsEn())
        {
            imgE.sprite = Resources.Load("Img/motivationalon", typeof(Sprite)) as Sprite;
            EventCenter.Broadcast(EventType.SetEncourageActive, true);
        }
        else
        { 
            imgE.sprite = Resources.Load("Img/motivationaloff", typeof(Sprite)) as Sprite;
            EventCenter.Broadcast(EventType.SetEncourageActive,false);
        }
        GameManager.GetInstance().Save();
    }
}
