using UnityEngine;
using TMPro;

/// <summary>
/// ランキングテキスト表示
/// </summary>
public class DisplayRankingText : MonoBehaviour
{
    [Header("ランキングテキスト"), SerializeField]
    private TextMeshProUGUI rankingText_;

    [Header("タワーに乗っているミノの数を表示"), SerializeField]
    private DisplayRideTowerNum displayRideTowerNum_;

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
        // プレイヤーたちの乗っているタワー数計算コンポーネント取得
        var displayRideTowerNums_ = FindObjectsOfType<DisplayRideTowerNum>();

        // 各プレイヤーの数と比較し、順位を算出
        var rank = 1;
        foreach (var displayRideTowerNum in displayRideTowerNums_)
        {
            // 自分自身のコンポーネントとは比較しない
            if (displayRideTowerNum == displayRideTowerNum_)
            {
                continue;
            }

            if (displayRideTowerNum.RideTowerNum > displayRideTowerNum_.RideTowerNum)
            {
                rank++;
            }
        }
        // ランキングテキスト更新
        rankingText_.text = $"{rank}い";
    }
}