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
        for(GameData.Instance.LimitTime = maxTime_; GameData.Instance.LimitTime >= 0; GameData.Instance.LimitTime--)
        {
            SetText(GameData.Instance.LimitTime);
            image_.fillAmount = Mathf.InverseLerp(0, maxTime_, GameData.Instance.LimitTime);
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
