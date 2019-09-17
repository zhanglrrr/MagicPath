using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

///<summary>
///
///</summary>

public class FinishLine : MonoBehaviour
{
    private ParticleSystem fireworks1;
    private ParticleSystem fireworks2;

    public static FinishLine Instance;

    private void Awake()
    {
        Instance = this; 
        fireworks1 = transform.GetChild(0).GetComponent<ParticleSystem>();
        fireworks2 = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Skier")
        {
            GameManager.GetInstance().SetIsCameraFollow(false);
            EventCenter.Broadcast(EventType.SetNextLevelColors);
            EventCenter.Broadcast(EventType.SkierVictory);
            EventCenter.Broadcast(EventType.CompleteTextDiaplay);

            GameManager.GetInstance().SetLevel(GameManager.GetInstance().GetLevel() + 1);
            GameManager.GetInstance().SetBestScore(0);
            GameManager.GetInstance().Save();

            fireworks1.Play();
            fireworks2.Play();

            transform.GetComponent<AudioSource>().Play();

            StartCoroutine(Dealy());
        }
    }

    IEnumerator Dealy()
    {
        yield return new WaitForSeconds(3f);

        //相机平滑移动，淡入淡出场景
        GameManager.GetInstance().SetIsGameOver(true);
        yield return new WaitForSeconds(1f);
        GameManager.GetInstance().SetIsGameOver(false);
        GameManager.GetInstance().SetFraction(0);
        GameManager.GetInstance().SetBestFraction(0);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.GetInstance().SetCanRestart(true);
        GameManager.GetInstance().SetIsCameraFollow(true);
        GameManager.GetInstance().SetBonusNum(0);
    }

    public float GetFinishLineY()
    {
        return transform.position.y;
    }
}
