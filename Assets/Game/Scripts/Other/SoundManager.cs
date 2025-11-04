using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サウンドマネージャー
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// インスタンス
    /// </summary>
    public static SoundManager Instance { get; set; }

    [Header("BGMソース"),SerializeField]
    private AudioSource bgm_;

    [Header("SEソース"), SerializeField]
    private AudioSource se_;

    [Header("SEクリップ"), SerializeField]
    private List<AudioClip> seClips_;

    [Header("BGMクリップ"), SerializeField]
    private List<AudioClip> bgmClips_;

    [Header("SEディクショナリー"), SerializeField]
    private Dictionary<string, AudioClip> seDict_ = new();

    [Header("BGMディクショナリー"), SerializeField]
    private Dictionary<string, AudioClip> bgmDict_ = new();

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // ディクショナリー初期化
        foreach (var clip in seClips_)
        {
            seDict_[clip.name] = clip;
        }
        foreach (var clip in bgmClips_)
        {
            bgmDict_[clip.name] = clip;
        }
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    public static void PlayBGM(string clipName, float volume = 0.5f, float pitch = 1.0f, bool loop = true)
    {
        if (!Instance.bgmDict_.TryGetValue(clipName, out var clip))
        {
            Debug.LogError($"BGMクリップが見つかりません: {clipName}");
            return;
        }
        Instance.bgm_.clip = clip;
        Instance.bgm_.loop = loop;
        Instance.bgm_.volume = volume;
        Instance.bgm_.pitch = pitch;
        Instance.bgm_.Play();
    }

    /// <summary>
    /// SE再生
    /// </summary>
    public static void PlaySE(string clipName, float volume = 1.0f, float pitch = 1.0f)
    {
        if (!Instance.seDict_.TryGetValue(clipName, out var clip))
        {
            Debug.LogError($"SEクリップが見つかりません: {clipName}");
            return;
        }
        Instance.se_.volume = volume;
        Instance.se_.pitch = pitch;
        Instance.se_.PlayOneShot(clip);
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public static void StopBGM()
    {
        Instance.bgm_.Stop();
    }

    /// <summary>
    /// SE停止
    /// </summary>
    public static void StopSE()
    {
        Instance.se_.Stop();
    }
}
