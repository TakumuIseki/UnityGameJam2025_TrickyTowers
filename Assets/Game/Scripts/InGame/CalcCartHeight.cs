using UnityEngine;

/// <summary>
/// カートの上面の高さを算出
/// </summary>
public class CalcCartHeight : MonoBehaviour
{
    [Header("当たり判定"),SerializeField]
    private Collider2D collider_;

    /// <summary>
    /// カートの最も高い頂点のY座標
    /// </summary>
    public float TopY { get; private set; }

    /// <summary>
    /// Start
    /// </summary>
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
