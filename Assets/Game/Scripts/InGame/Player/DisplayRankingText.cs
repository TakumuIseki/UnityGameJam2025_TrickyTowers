using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// ランキングテキスト表示
/// </summary>
public class DisplayRankingText : MonoBehaviour
{
    [Header("ランキング画像"), SerializeField]
    private Image rankingImage_;

    [Header("タワーに乗っているミノの数を表示"), SerializeField]
    private DisplayRideTowerNum displayRideTowerNum_;

    [Header("順位画像リスト"),SerializeField]
    private Sprite[] rankingSprites_;

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
        // ランキング画像更新
        if (rank != 4)
        {
            rankingImage_.enabled = true;
            rankingImage_.sprite = rankingSprites_[rank - 1];
        }
        else
        {
            // Imageコンポーネントを非表示にする
            rankingImage_.enabled = false;
        }
    }
}