/// <summary>
/// カートの上面の高さを算出するスクリプト。
/// </summary>
using UnityEngine;

public class CartHeightCalculatorComponent : MonoBehaviour
{
    [Header("当たり判定"),SerializeField]
    private Collider2D collider_;

    public float TopY { get; private set; } = 0f;    // カートの最も高い頂点のy座標を格納する。

    private void Start()
    {
        StartCoroutine(InitAfterPhysics());
    }

    /// <summary>
    /// 現在のフレームの物理演算が完了した後、オブジェクトを初期化します。
    /// </summary>
    private System.Collections.IEnumerator InitAfterPhysics()
    {
        // 1フレーム待って物理計算後に取得
        yield return new WaitForFixedUpdate();
        TopY = collider_.bounds.max.y;
    }
}
