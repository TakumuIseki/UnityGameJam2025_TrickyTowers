/// <summary>
/// テトリミノの最も高い頂点のy座標を算出するスクリプト。
/// </summary>
using UnityEngine;

public class TetriminoHeightCalculator : MonoBehaviour
{
    public float MaxY { get; private set; } = 0f;    // このテトリミノの最も高い頂点のy座標を格納する。

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
        MaxY = 0f;

        // 子のRendererからbounds.max.yを調べる
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            // 子のbounds.max.yをtopYに保存。
            float topY = renderer.bounds.max.y;

            // 子のbounds.max.yが記録していたyを上回ったら更新。
            if (topY > MaxY)
            {
                MaxY = topY;
            }
        }
    }
}
