using UnityEngine;

/// <summary>
/// ミノステート　：　待機
/// </summary>
public class MinoStateWait : IMinoState
{
    /// <summary>
    /// ミノ
    /// </summary>
    Mino _mino;

    /// <summary>
    /// ステート
    /// </summary>
    public MinoState CurrentState => MinoState.Wait;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MinoStateWait(Mino mino) => _mino = mino;

    /// <summary>
    /// ステート開始時に実行
    /// </summary>
    public void Enter() 
    {

    }

    /// <summary>
    /// フレームごとに実行
    /// </summary>
    public void Update()
    {
    }
    
    /// <summary>
    /// ステート終了時に実行
    /// </summary>
    public void Exit()
    {

    }

    /// <summary>
    /// 他のオブジェクトと当たったとき
    /// </summary>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
