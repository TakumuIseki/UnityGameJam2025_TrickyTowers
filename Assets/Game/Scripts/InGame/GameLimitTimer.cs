/// <summary>
/// 制限時間タイマー
/// </summary>
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLimitTimer : MonoBehaviour
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

        if (currentTime_ < 0.0f)
        {
            // リザルトへ遷移
            // TODO:iseki 今は即時遷移...
            SceneManager.LoadScene("RankingScene");
        }

        // 残り時間を表示。
        timerText_.text = $"{(int)currentTime_}";
    }
}
