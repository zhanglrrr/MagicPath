using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

///<summary>
///
///</summary>

public class Finger : MonoBehaviour
{
    public bool isPress;
    private float time;

    public static Finger instance;

    private void Awake()
    {
        instance = this;
        SceneManager.sceneLoaded += this.OnSceneLoaded;
        isPress = false;
        time = 0;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= this.OnSceneLoaded;
    }

    private void Update()
    {
        if (isPress)
        {
            time += 1;
        }
        else
        {
            time = 0;
        }

        if(time > 20)
        {
            GameManager.GetInstance().SetIsPressing(true);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject gameObject = GameObject.Find("Canvas");
        if (gameObject != null)
        {
            Finger.FingerPage component = new GameObject("FingerPage", new Type[]
            {
                typeof(Finger.FingerPage),
                typeof(RectTransform),
                typeof(Image)
            }).GetComponent<Finger.FingerPage>();
            component.transform.SetParent(gameObject.transform);
            component.transform.SetAsFirstSibling();
            RectTransform component2 = component.GetComponent<RectTransform>();
            component2.anchorMin = Vector2.zero;
            component2.anchorMax = Vector2.one;
            component2.sizeDelta = Vector2.one;
            component2.anchoredPosition = Vector2.zero;
            component2.localPosition = Vector3.zero;
            component2.localScale = Vector3.one;
            component2.localRotation = Quaternion.identity;
            component.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        }
    }

    private class FingerPage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
    {
        public void OnPointerDown(PointerEventData data)
        {
            if (!GameManager.GetInstance().GetIsGameStart())
            {
                if(GameManager.GetInstance().GetCanRestart())
                {
                    EventCenter.Broadcast(EventType.GameStart);
                    EventCenter.Broadcast(EventType.FadeMode);
                }
                return;
            }

            EventCenter.Broadcast(EventType.GameTouch);
            EventCenter.Broadcast(EventType.SetSkierPower, true);

            //GameManager.GetInstance().SetIsPressing(true);
            instance.isPress = true;
        }
        
        public void OnPointerUp(PointerEventData data)
        {
            if (!GameManager.GetInstance().GetIsGameStart()) return;

            EventCenter.Broadcast(EventType.SetSkierPower, false);

            instance.isPress = false;
            GameManager.GetInstance().SetIsPressing(false);
        }
    }
}
