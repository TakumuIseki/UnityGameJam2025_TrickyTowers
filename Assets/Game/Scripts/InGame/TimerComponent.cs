/// <summary>
/// 制限時間の計算と表示を管理するスクリプト。
/// </summary>
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("タイマーTextMesh"), SerializeField]
    private TextMeshProUGUI timerText_;

    [Header("制限時間"), SerializeField]
    private float maxTime_ = 60.0f;

    private float currentTime_ = 0.0f; // 残り時間を格納する変数。

    void Start()
    {
        // 残り時間に制限時間を代入。
        currentTime_ = maxTime_;
    }

    void Update()
    {
        // 経過時間に応じて残り時間を算出。
        currentTime_ -= Time.deltaTime;

        // currentTime_は0より小さくならない。
        if (currentTime_ < 0.0f)
        {
            currentTime_ = 0.0f;
        }

        // 残り時間を表示。
        timerText_.text = $"TIME: {(int)currentTime_}";
    }
}
