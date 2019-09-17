using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>

public class AdsPanel : MonoBehaviour
{
    private Text txt;
    private Button button;
    private Image bg;
    private bool isShow;
    private void Awake()
    {
        EventCenter.AddListener(EventType.ShowAdsPanel, Show);
        gameObject.SetActive(false);
        isShow = false;
        txt = transform.Find("Text").GetComponent<Text>();
        button = transform.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        bg = transform.Find("Bg").GetComponent<Image>();
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowAdsPanel, Show);
    }

    private void OnButtonClick()
    {
        EventCenter.Broadcast(EventType.ShowAds);
        isShow = true;
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
        bg.DOFade(0.745f, 1f);
        StartCoroutine(Dealy());
    }
    IEnumerator Dealy()
    {
        yield return new WaitForSeconds(0.5f);
        txt.DOText((int.Parse(txt.text) - 1).ToString(), 1f).OnComplete(() => {
            txt.DOText((int.Parse(txt.text) - 1).ToString(), 1f).OnComplete(()=> {
                txt.DOText((int.Parse(txt.text) - 1).ToString(), 1f).OnComplete(() => {
                    bg.DOFade(0, 1f).OnComplete(() => {
                        gameObject.SetActive(false);
                        if (!isShow)
                            EventCenter.Broadcast(EventType.GameOver);
                    });
                    button.image.DOFade(0, 1f);
                });
            });
        });
    }
}
