/// <summary>
/// ランキングスコア表示
/// </summary>
using TMPro;
using UnityEngine;

public class RankingScoreViewer : MonoBehaviour
{
    /// <summary>
    /// 表示するランキング順位
    /// </summary>
    private enum RankingType
    {
        First,
        Second,
        Third,
    }

    // 表示するランキングの順位
    [SerializeField]
    private RankingType rankingType_ = RankingType.First;

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        // ランキングデータの取得
        var rankingDatas = SaveData.Get().GetRankingDatas();
        // 指定した順位がランキングデータにない場合は非表示にしておく
        if (rankingDatas.Count <= (int)rankingType_)
        {
            gameObject.SetActive(false);
            return;
        }
        // ランキングデータのソート
        // 高いスコア順にソート
        rankingDatas.Sort((a, b) => b.height.CompareTo(a.height));
        // ランキングデータをテキストに反映 
        var children = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var child in children)
        {
            RankingData rankingData = rankingDatas[(int)rankingType_];
            if (child.gameObject.name == "Name")
            {
                SetName(child, rankingData.name);
            }
            else if (child.gameObject.name == "Score")
            {
                SetScore(child, rankingData.height);
            }
        }
    }

    /// <summary>
    /// ユーザー名設定
    /// </summary>
    private void SetName(TextMeshProUGUI textMeshPro, string name)
    {
        if (textMeshPro == null)
        {
            return;
        }
        textMeshPro.text = name;
    }

    /// <summary>
    /// スコア設定
    /// </summary>
    private void SetScore(TextMeshProUGUI textMeshPro, uint score)
    {
        if (textMeshPro == null)
        {
            return;
        }
        textMeshPro.text = score.ToString() + "m"; // スコアの単位をメートルに設定
    }
}