using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>

public class CompleteTxt : MonoBehaviour
{
    private Text txtComplete;

    private void Awake()
    {
        EventCenter.AddListener(EventType.CompleteTextDiaplay,Display);
        txtComplete = GetComponent<Text>();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.CompleteTextDiaplay, Display);
    }

    private void Display()
    {
        txtComplete.text = "Level  " + GameManager.GetInstance().GetLevel().ToString() + "  Complete !";
        gameObject.SetActive(true);
    }
}