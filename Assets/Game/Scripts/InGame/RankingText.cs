using UnityEngine;
using TMPro;

/// <summary>
/// ランキングテキスト更新
/// </summary>
public class RankingText : MonoBehaviour
{
    [Header("ランキングテキスト"), SerializeField]
    private TextMeshProUGUI[] rankingTexts_;

    [Header("各プレイヤーの乗っているタワー数計算コンポーネント"), SerializeField]
    private CalcRideTowerNum[] calcRideTowerNums_;

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // ランキングテキスト更新
        UpdateRankingText();
    }

    /// <summary>
    /// ランキングテキスト更新
    /// </summary>
    private void UpdateRankingText()
    {
        // 参加プレイヤー数をカウント
        var activePlayerCount = 0;
        for (var i = 0; i < rankingTexts_.Length; i++)
        {
            if (rankingTexts_[i].gameObject.activeSelf)
            {
                activePlayerCount++;
            }
        }
        // 各プレイヤーの順位を算出
        for (var i = 0; i < activePlayerCount; i++)
        {
            var rank = 1;
            for (var j = 0; j < activePlayerCount; j++)
            {
                if (i != j && calcRideTowerNums_[i].RideTowerNum < calcRideTowerNums_[j].RideTowerNum)
                {
                    rank++;
                }
            }

            // ランキングテキスト更新
            rankingTexts_[i].text = $"{rank}い";
        }
    }
}
