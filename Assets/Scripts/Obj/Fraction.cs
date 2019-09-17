using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>

public class Fraction : MonoBehaviour
{
    private Text txt_best;
    private Text txt_normal;
    private CanvasGroup cg_best;
    private CanvasGroup cg_normal;

    private void Awake()
    {
        txt_best = transform.Find("BestFraction").GetComponent<Text>();
        txt_normal = transform.Find("NormalFraction").GetComponent<Text>();
    }

    private void Start()
    {
        txt_best = transform.Find("BestFraction").GetComponent<Text>();
        cg_best = transform.Find("BestFraction").GetComponent<CanvasGroup>();
        cg_normal = transform.Find("NormalFraction").GetComponent<CanvasGroup>();

        txt_best.text = GameManager.GetInstance().GetBestFraction().ToString();
    }

    private void Update()
    {
        if (!GameManager.GetInstance().GetIsGameStart())
        {
            //cg_best.alpha =  Mathf.Lerp(cg_best.alpha, 1, Time.deltaTime);
            //cg_normal.alpha = Mathf.Lerp(cg_normal.alpha, 0, 6 * Time.deltaTime);
            return;
        }

        cg_best.alpha = Mathf.Lerp(cg_best.alpha, 0, 6 * Time.deltaTime);
        cg_normal.alpha = Mathf.Lerp(cg_normal.alpha, 1, Time.deltaTime);
        txt_normal.text = GameManager.GetInstance().GetFraction().ToString();
    }
}
