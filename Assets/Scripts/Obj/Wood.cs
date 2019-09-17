using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///
///</summary>

public class Wood : MonoBehaviour
{
    private TextMesh tm;

    private void Awake()
    {
        tm = transform.GetChild(1).GetComponent<TextMesh>();
    }

    private void Start()
    {
        tm.text = GameManager.GetInstance().GetLevel().ToString();
    }
}
