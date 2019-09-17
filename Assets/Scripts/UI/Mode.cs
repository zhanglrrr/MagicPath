using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>

public class Mode : MonoBehaviour
{
    private Text text;
    private Button btn;
    private Image img;
    private bool canClick;

    private void Awake()
    {
        EventCenter.AddListener(EventType.FadeMode, FadeMode);
        btn =transform.GetChild(0).GetComponent<Button>();
        img = transform.GetChild(0).GetComponent<Image>();
        btn.onClick.AddListener(OnClick);
        text = GetComponentInChildren<Text>();
        canClick = false;
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.FadeMode, FadeMode);
    }

    private void Start()
    {
        SetColor();
        StartCoroutine(Dealy());
    }

    IEnumerator Dealy()
    {
        yield return new WaitForSeconds(1.1f);
        canClick = true;
    }

    private void FadeMode()
    {
        img.DOFade(0, 1f);
    }

    private void OnClick()
    {
        if (!canClick) return;
        btn.onClick.RemoveListener(OnClick);
        GameManager.GetInstance().SetIsCrazy(!GameManager.GetInstance().GetIsCrazy());
        GameManager.GetInstance().Save();
        SetColor();

        GameManager.GetInstance().ClearRollStones();
        EventCenter.Broadcast(EventType.GenerateRollingStone);
        EventCenter.Broadcast(EventType.FadePrompt);

        text.DOFade(1, 1f).OnComplete(() =>
        {
            text.DOFade(0, 0.5f).OnComplete(()=>
            {
                btn.onClick.AddListener(OnClick);
            });
        });
    }

    private void SetColor()
    {
        if (GameManager.GetInstance().GetIsCrazy())
        {
            //img.color = new Color(255, 0, 0, 1);
            text.text = "Crazy Mode";
        }
        else
        {
            //img.color = new Color(255, 255, 255, 1);
            text.text = "Normal Mode";
        }
    }
}
