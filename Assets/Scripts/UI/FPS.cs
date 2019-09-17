using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///
///</summary>

public class FPS : MonoBehaviour
{
    public Text t;
    public float timer = 0.5f;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            t.text =((int) (1.0f / Time.smoothDeltaTime)).ToString();
            timer = 0.5f;
        }
    }
}
