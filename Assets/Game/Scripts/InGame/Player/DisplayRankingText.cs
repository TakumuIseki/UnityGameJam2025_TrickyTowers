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
    /// プレイヤー番号
    /// </summary>
    private int playerNum_;

    /// <!--/summary>-->
    /// Start
    /// </summary>
    private void Start()
    {
        // 親の親のゲームオブジェクトの名前から番号を取得
        var parentObjName = transform.parent.parent.gameObject.name;
        var playerNumStr = parentObjName.Replace("Player", "");

        if (int.TryParse(playerNumStr, out var playerNum))
        {
            playerNum_ = playerNum;
        }
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // 制限時間が0以下の場合は更新しない
        if (GameData.Instance.LimitTime <= 0)
        {
            return;
        }

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

        // 1位の時勝利したプレイヤー番号リストに追加
        if (rank == 1)
        {
            GameData.Instance.AddWinPlayerNum(playerNum_);
        }
        else
        {
            GameData.Instance.RemoveWinPlayerNum(playerNum_);
        }
    }
}