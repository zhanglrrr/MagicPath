using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>

public class PromptPanel : MonoBehaviour
{
    private Text txtL, txtR;
    private Image imgL, imgR;

    private void Awake()
    {
        EventCenter.AddListener(EventType.FadePrompt,FadePrompt);
        txtL = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        txtR = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        imgL = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        imgR = transform.GetChild(1).GetChild(1).GetComponent<Image>();
        txtL.color = new Color(txtL.color.r, txtL.color.g, txtL.color.b, 0);
        txtR.color = new Color(txtR.color.r, txtR.color.g, txtR.color.b, 0);
        imgL.color = new Color(imgL.color.r, imgL.color.g, imgL.color.b, 0);
        imgR.color = new Color(imgR.color.r, imgR.color.g, imgR.color.b, 0);
    }

    private void Start()
    {
        txtL.DOFade(1, 1f).SetEase(Ease.InQuart);
        txtR.DOFade(1, 1f).SetEase(Ease.InQuart);
        imgL.DOFade(0.5f, 1f).SetEase(Ease.InQuart);
        imgR.DOFade(0.5f, 1f).SetEase(Ease.InQuart);
    }

    private void Update()
    {
        if(GameManager.GetInstance().GetIsGameStart())
        {
            txtL.DOFade(0, 0.5f);
            txtR.DOFade(0, 0.5f);
            imgL.DOFade(0, 0.5f);
            imgR.DOFade(0, 0.5f);

        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.FadePrompt,FadePrompt);
    }

    private void FadePrompt()
    {
        txtL.DOFade(0, 1f).OnComplete(() =>
        {
           txtL.DOFade(1, 1f);
        });
        txtR.DOFade(0, 1f).OnComplete(() =>
        {
           txtR.DOFade(1, 1f);
        });
        imgL.DOFade(0, 1f).OnComplete(() =>
        {
           imgL.DOFade(0.5f, 1f);
        });
        imgR.DOFade(0, 1f).OnComplete(() =>
        {
           imgR.DOFade(0.5f, 1f);
        });
    }
}
