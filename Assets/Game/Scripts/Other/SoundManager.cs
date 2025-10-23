using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サウンドマネージャー
/// </summary>
public class SoundManager : MonoBehaviour
{
    [Header("BGMソース"),SerializeField]
    private AudioSource bgm_;

    [Header("SEソース"),SerializeField]
    private AudioSource se_;

    /// <summary>
    /// BGM再生
    /// </summary>
    public void PlayBGM()
    {
        bgm_.Play();
    }

    /// <summary>
    /// SE再生
    /// </summary>
    public void PlaySE()
    {
        se_.Play();
    }
}
