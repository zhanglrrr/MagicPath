using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>

public class Pine : MonoBehaviour
{
    private SpriteRenderer bonus;
    private SpriteRenderer leaves;
    private TextMesh text;

    private void Awake()
    {
        bonus = transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>();
        bonus.enabled = false;

        leaves = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        leaves.color = Level.GetPineColor();

        transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>().sortingLayerName = "Pine";
        transform.GetChild(0).GetChild(4).GetComponent<MeshRenderer>().sortingOrder = 10;

        text = transform.GetChild(0).GetChild(4).GetComponent<TextMesh>();
        text.color = Level.GetSkierColor();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }

    ///<summary>
    ///擦边得分
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (!GameManager.GetInstance().GetIsGameStart()) return;

        if (collider2D.tag == "Skier")
        {
            Transform pineBody = transform.GetChild(0).transform;
            Vector3 tmp = pineBody.localScale;
            pineBody.DOScale(1.5f * pineBody.localScale, 0.1f).OnComplete(() =>
            {
                pineBody.DOScale(tmp, 0.1f);
            });

            bonus.enabled = true;
            bonus.DOFade(0, 2f);
            bonus.transform.DOScale(5f * bonus.transform.localScale, 1f);
            
            text.text = GameManager.GetInstance().GetBounsFraction().ToString();
            GameManager.GetInstance().AddBonusNum();
            if(GameManager.GetInstance().GetIsVoice())
                transform.GetComponent<AudioSource>().Play();

            if(GameManager.GetInstance().GetBonusNum() >=4)
                text.color = Level.GetFeverColor();

            text.color = new Color(text.color.r, text.color.g, text.color.b, 0.75f);
            text.transform.DOLocalMoveY(text.transform.localPosition.y + 0.5f, 1f);

            Encouragement.Instance.Whoosh();
            StartCoroutine(Dealy());
        }
    }

    IEnumerator Dealy()
    {
        yield return new WaitForSeconds(1f);
        text.color = Color.clear;
    }
}
