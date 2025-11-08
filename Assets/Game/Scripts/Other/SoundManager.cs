using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("BGMソース"), SerializeField]
    private AudioSource bgm_;

    [Header("SEソース"), SerializeField]
    private AudioSource se_;

    [Header("SEクリップ"), SerializeField]
    private List<AudioClip> seClips_;

    [Header("BGMクリップ"), SerializeField]
    private List<AudioClip> bgmClips_;

    private Dictionary<string, AudioClip> seDict_ = new();
    private Dictionary<string, AudioClip> bgmDict_ = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (var clip in seClips_)
        {
            seDict_[clip.name] = clip;
        }
        foreach (var clip in bgmClips_)
        {
            bgmDict_[clip.name] = clip;
        }
    }

    // BGM再生
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

    // 非同期SE再生
    public static async UniTask PlaySEAsync(string clipName, float volume = 1.0f, float pitch = 1.0f)
    {
        if (!Instance.seDict_.TryGetValue(clipName, out var clip))
        {
            Debug.LogError($"SEクリップが見つかりません: {clipName}");
            return;
        }

        Instance.se_.volume = volume;
        Instance.se_.pitch = pitch;
        Instance.se_.PlayOneShot(clip);

        // 再生が終わるまで待機
        await UniTask.Delay((int)(clip.length * 1000f));
    }

    // 同期SE再生
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

    public static void StopBGM() => Instance.bgm_.Stop();
    public static void StopSE() => Instance.se_.Stop();
}
