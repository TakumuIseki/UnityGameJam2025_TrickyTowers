using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// カウントダウン
/// </summary>
public class CountDown : MonoBehaviour
{
    [Header("カウントダウンテキスト"),SerializeField]
    private TextMeshProUGUI countDownText_;

    [Header("表示テキスト"),SerializeField]
    private string[] displayTexts_;

    /// <summary>
    /// カウントダウンタスク
    /// </summary>
    public async UniTask CoundDownTask()
    {
        foreach(string displayText in displayTexts_)
        {
            countDownText_.text = displayText;
            await UniTask.Delay(TimeSpan.FromSeconds(1));
        }
        Destroy(gameObject);
    }
}
