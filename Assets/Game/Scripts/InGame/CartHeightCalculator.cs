/// <summary>
/// カートの上面の高さを算出するスクリプト。
/// </summary>
using UnityEngine;

public class CartHeightCalculator : MonoBehaviour
{
    public float maxY_ { get; private set; } = 0f;    // カートの最も高い頂点のy座標を格納する。

    void Start()
    {
        CalcCartTopY();
    }

    /// <summary>
    /// カートの上面の高さを算出する。
    /// </summary>
    private void CalcCartTopY()
    {
        var collider = GetComponent<Collider2D>();
        maxY_ = collider.bounds.max.y;
    }
}
