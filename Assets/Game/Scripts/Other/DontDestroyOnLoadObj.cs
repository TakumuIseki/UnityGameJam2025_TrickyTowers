using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーンを跨いでも破棄されないオブジェクト
/// </summary>
public class DontDestroyOnLoadObj : MonoBehaviour
{
    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
