using TMPro;
using UnityEngine;

/// <summary>
/// タワーに乗っているミノの数を表示
/// </summary>
public class DisplayRideTowerNum : MonoBehaviour
{
    [Header("乗っているタワー数表示UI"), SerializeField]
    TextMeshProUGUI rideTowerNumText_;

    [Header("親オブジェクト"), SerializeField]
    private GameObject parentObject_;

    /// <summary>
    /// 乗っているタワーの数
    /// </summary>
    public int RideTowerNum { get; private set; } = 0;

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        rideTowerNumText_.text = $"{CalcRideTowerNum()}こ";
    }

    /// <summary>
    /// カートに乗っているミノの数を算出
    /// </summary>
    public int CalcRideTowerNum()
    {
        RideTowerNum = 0;

        // タワータグのついたオブジェクトの総数をカウント
        for (var i = 0; i < parentObject_.transform.childCount; i++)
        {
            var child = parentObject_.transform.GetChild(i);
            if (child.CompareTag("Tower"))
            {
                RideTowerNum++;
            }
        }

        // ミノの数を出力
        return RideTowerNum;
    }
}
