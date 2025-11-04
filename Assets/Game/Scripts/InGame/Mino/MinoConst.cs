using UnityEngine;

/// <summary>
/// ミノ定数
/// </summary>
public static class MinoConst
{
    /// <summary>
    /// このY座標を下回ったら破棄
    /// </summary>
    public static readonly float DESTROY_Y_THRESHOLD = -620.0f;

    /// <summary>
    /// 重力
    /// </summary>
    public static readonly float GRAVITY = 30.0f;

    /// <summary>
    /// 通常時の落下速度
    /// </summary>
    public static readonly float NORMAL_FALL_SPEED = 100.0f;

    /// <summary>
    /// 加速倍率
    /// </summary>
    public static readonly float SPEED_UP_MAGNIFICATION = 2.0f;

    /// <summary>
    /// 回転角度
    /// </summary>
    public static readonly Vector3 ROTATION_ANGLE = new Vector3(0.0f, 0.0f, 90.0f);

    /// <summary>
    /// 左右移動の幅
    /// </summary>
    public static readonly float MOVE_DISTANCE_PER_KEY = 10.0f;

    /// <summary>
    /// ミノがタワーの一部になった時のタグ
    /// </summary>
    public static readonly string TOWER_TAG = "Tower";
}
