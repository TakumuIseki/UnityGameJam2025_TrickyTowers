/// <summary>
/// テトリミノの最も高い頂点のy座標を算出するスクリプト。
/// </summary>
using UnityEngine;

public class TetriminoHeightCalculatorComponent : MonoBehaviour
{
    public float TopY { get; private set; } = 0f;    // このテトリミノの最も高い頂点のy座標を格納する。

    void Update()
    {
        CalcTetriminoTopY();
    }

    /// <summary>
    /// 子である各ブロックのbounds.max.yを比較し、
    /// 最も大きい値をこのテトリミノの頂点の高さとする。
    /// </summary>
    private void CalcTetriminoTopY()
    {
        // 初期化。
        TopY = 0f;

        // 子のRendererからbounds.max.yを調べる
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            // 子のbounds.max.yを保存。
            float currentY = renderer.bounds.max.y;

            // 最も高いbounds.max.yをこのテトリミノの高さとする。
            TopY = Mathf.Max(currentY, TopY);
        }
    }
}
