using TMPro;
using UnityEngine;

/// <summary>
/// タワーに乗っているミノの数を算出
/// </summary>
public class CalcRideTowerNum : MonoBehaviour
{
    [Header("乗っているタワー数表示UI"), SerializeField]
    TextMeshProUGUI rideTowerNumText_;

    [Header("親オブジェクト"), SerializeField]
    private GameObject parentObject_;

    /// <summary>
    /// 乗っているタワーの数
    /// </summary>
    int rideTowerNum_;

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        rideTowerNum_ = 0;
        rideTowerNumText_.text = $"{GetRideTowerMinosNum()}こ";
    }

    /// <summary>
    /// カートに乗っているミノの数を取得
    /// </summary>
    public int GetRideTowerMinosNum()
    {
        // タワータグのついたオブジェクトの総数をカウント
        for (var i = 0; i < parentObject_.transform.childCount; i++)
        {
            var child = parentObject_.transform.GetChild(i);
            if (child.CompareTag("Tower"))
            {
                rideTowerNum_++;
            }
        }

        // ミノの数を出力
        return rideTowerNum_;
    }
}
