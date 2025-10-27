using UnityEngine;

/// <summary>
/// ミノの最も高い頂点のy座標を算出
/// </summary>
public class CalcMinoHeight : MonoBehaviour
{
    /// <summary>
    /// このミノの最も高い頂点(y座標)
    /// </summary>
    public float TopY { get; private set; } = 0f;

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        CalcMinoTopY();
    }

    /// <summary>
    /// 子である各ブロックのbounds.max.yを比較し最も大きい値をこのミノの頂点の高さとする
    /// </summary>
    private void CalcMinoTopY()
    {
        // 初期化。
        TopY = 0f;

        // 積まれているミノを全て調べる
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            // 積まれているミノのうち最も高いbounds.max.yをこのテトリミノの高さとする
            TopY = Mathf.Max(renderer.bounds.max.y, TopY);
        }
    }
}
