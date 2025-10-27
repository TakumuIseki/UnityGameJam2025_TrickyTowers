using UnityEngine;

/// <summary>
/// 重心を表示/設定する
/// </summary>
[ExecuteAlways]
public class ShowCenterOfMass : MonoBehaviour
{
    [Header("Rigidbody2D"), SerializeField]
    private Rigidbody2D rigidbody_;

    [Header("重心の色"), SerializeField]
    private Color color_ = Color.red;

    [Header("重心のオフセット（ローカル座標）"), SerializeField]
    private Vector2 centerOfMassOffset_ = Vector2.zero;

    /// <summary>
    /// インスペクタで値が変更されたときに即反映
    /// </summary>
    void OnValidate()
    {
        rigidbody_.centerOfMass = centerOfMassOffset_;
    }

    /// <summary>
    /// 重心を表示
    /// </summary>
    void OnDrawGizmos()
    {
        // 重心の色を設定
        Gizmos.color = Color.red;

        // ローカル → ワールド座標に変換
        Vector3 worldCOM = transform.TransformPoint(rigidbody_.centerOfMass);

        // 重心の位置に球を描画
        Gizmos.DrawSphere(worldCOM, 1.0f);
    }
}