using UnityEngine;


/// <summary>
/// ミノステートのインタフェース
/// </summary>
public interface IMinoState
{
    /// <summary>
    /// 現在のステート
    /// </summary>
    public MinoState CurrentState { get; }

    /// <summary>
    /// ステート開始時に実行
    /// </summary>
    void Enter();

    /// <summary>
    /// フレームごとに実行
    /// </summary>
    void Update();

    /// <summary>
    /// ステート終了時に実行
    /// </summary>
    void Exit();

    /// <summary>
    /// 衝突時に実行
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision);
}

/// <summary>
/// ステート
/// </summary>
public enum MinoState 
{ 
    Wait,           // 待機
    ControlFall,    // 落下操作
    Tower           // タワー
}