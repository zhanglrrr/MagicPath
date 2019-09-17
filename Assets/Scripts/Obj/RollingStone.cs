using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>

public class RollingStone : MonoBehaviour
{
    public float stongeRotateSpeed = 3f;

    private ParticleSystem power;

    private SpriteRenderer stone;

    private Vector3 velocity;

    private float rotateSpeed;

    private bool isRight;

    private float xSpeed,ySpeed;

    private float stoneWidth = 0.5f;

    private bool isStartedMove;

    private void Awake()
    {
        power = transform.Find("PowerPS").GetComponent<ParticleSystem>();
        stone = transform.Find("Stone").GetComponent<SpriteRenderer>();
        isStartedMove = false;
    }
    private void Start()
    {
        if (power.transform.rotation.eulerAngles.z == 315)
            isRight = true;
        else
            isRight = false;

        SetMove();
    }

    private void Update()
    {
        if((ySpeed < -2 || xSpeed > 2 || xSpeed < -2))
        {
            if (!isStartedMove && Camera.main.transform.position.y - transform.position.y > -1)
                return;
        }
        else
        {
            if (!isStartedMove && Camera.main.transform.position.y - transform.position.y > 0.5)
                return;
        }

        isStartedMove = true;

        float deltaTime = Time.deltaTime;
        
        UpdateMovement(deltaTime);

        if (Camera.main.transform.position.x - transform.position.x > 10 ||-10 > Camera.main.transform.position.x - transform.position.x)
            gameObject.SetActive(false);
    }

    private void SetMove()
    {
        ySpeed = -3f + 0.5f * Random.Range(0, 4);//纵速度在-1.5到-3之间
        float ran = 0.5f * Random.Range(0, 9);//横速度在1到5之间，-代表方向

        rotateSpeed = 2 * Mathf.Sqrt( xSpeed * xSpeed + ySpeed * ySpeed )/ stoneWidth;

        
        if (isRight)
        {
            xSpeed = -1f - ran;
            rotateSpeed = stongeRotateSpeed;
        }
        else
        {
            xSpeed = 1f + ran;
            rotateSpeed = -stongeRotateSpeed;
        }

        velocity.x = xSpeed;
        velocity.y = ySpeed;
    }

    void UpdateMovement(float deltaTime)
    {
        Vector3 deltaMove = velocity * deltaTime;

        gameObject.transform.Translate(deltaMove, Space.World);

        stone.transform.Rotate(rotateSpeed * Vector3.forward);
    }
}
