using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

///<summary>
///
///</summary>

public class CameraFollow : MonoBehaviour
{
    public Transform skier;
    private float distanceUp;
    private float smooth = 50f;
    private Vector3 cameraPosition;
    private SpriteRenderer bg, fadeBg;
    private float mlp, mlp2;
    Vector2 velocity;

    private void Awake()
    {
        bg = transform.Find("bg").GetComponent<SpriteRenderer>();
        fadeBg = transform.Find("fadeBg").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        distanceUp = transform.position.y - skier.position.y;
        bg.color = Level.GetBackgroundColor();
        fadeBg.color = Level.GetBackgroundColor();
        fadeBg.color = new Color(fadeBg.color.r, fadeBg.color.g, fadeBg.color.b, 1);
        GameManager.GetInstance().SetCanRestart(false);
        fadeBg.DOFade(0, 1f).SetEase(Ease.InQuart).OnComplete(() =>
             {
                 GameManager.GetInstance().SetCanRestart(true);
             });
        mlp = 0.5f;
        mlp2 = 5;
    }

    private void Update()
    {
        if (!GameManager.GetInstance().GetIsCameraFollow() || GameManager.GetInstance().GetIsGameOver()) return;

        float posY = Mathf.SmoothDamp(transform.position.y,
               skier.position.y + distanceUp, ref velocity.y, 0.05f);

        if (posY< transform.position.y)
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);

        //if (!GameManager.GetInstance().GetIsCameraFollow() || GameManager.GetInstance().GetIsGameOver()) return;

        //cameraPosition = new Vector3(cameraPosition.x, skier.position.y + distanceUp, cameraPosition.y);

        //transform.position = cameraPosition;// Vector3.Lerp(transform.position, cameraPosition, smooth * Time.deltaTime);
    }

    private void LateUpdate()
    {
        //if (!GameManager.GetInstance().GetIsGameStart() || GameManager.GetInstance().GetCanRestart()) return;

        //cameraPosition = new Vector3(cameraPosition.x, skier.position.y + 5, cameraPosition.y);

        //transform.position = Vector3.Lerp(transform.position, cameraPosition, smooth * Time.deltaTime);
        if (!GameManager.GetInstance().GetIsGameOver()) return;
        mlp *= 0.99f;
        mlp2 -= 0.055f;
        cameraPosition = new Vector3(cameraPosition.x, skier.position.y + 5 > FinishLine.Instance.GetFinishLineY() + 5 ? skier.position.y + 5 : FinishLine.Instance.GetFinishLineY() + 6, cameraPosition.y);
        transform.position = Vector3.Lerp(transform.position, cameraPosition, mlp * Time.deltaTime);
        fadeBg.color = Color.Lerp(fadeBg.color, new Color(fadeBg.color.r, fadeBg.color.g, fadeBg.color.b, 1), mlp2 * Time.deltaTime);

    }
}
