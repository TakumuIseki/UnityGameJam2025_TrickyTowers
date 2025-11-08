using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 制限時間タイマー
/// </summary>
public class GameLimitTimer : MonoBehaviour
{
    [Header("タイマーTextMesh"), SerializeField]
    private TextMeshProUGUI text_;

    [Header("制限時間"), SerializeField]
    private int maxTime_ = 60;

    [Header("画像"), SerializeField]
    private Image image_;

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        SetText(maxTime_);
    }

    /// <summary>
    /// 制限時間の計測タスク
    /// </summary>
    /// <returns></returns>
    public async UniTask MeasureLimitTimeTask()
    {
        for(var time = maxTime_;time >= 0; time--)
        {
            SetText(time);
            image_.fillAmount = Mathf.InverseLerp(0, maxTime_, time);
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }
    }

    /// <summary>
    /// テキスト設定
    /// </summary>
    private void SetText(int time)
    {
        text_.text = $"{time}";
    }
}
