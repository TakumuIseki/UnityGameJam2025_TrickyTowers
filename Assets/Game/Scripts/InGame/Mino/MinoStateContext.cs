using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ミノステートの状態変更を取り持つクラス
/// </summary>
public class MinoStateContext
{
    /// <summary>
    /// 現在のステート
    /// </summary>
    IMinoState _currentState;

    /// <summary>
    /// 直前の状態
    /// </summary>
    IMinoState _previousState;

    /// <summary>
    /// ステートのテーブル
    /// </summary>
    Dictionary<MinoState, IMinoState> _stateTable;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(MinoState initState,Mino mino,Transform transform,Rigidbody2D rigidBody,string towerTag)
    {
        if (_stateTable != null)
        {
            return;
        }

        // 各状態選クラスの初期化
        Dictionary<MinoState, IMinoState> table = new()
        {
            { MinoState.Wait, new MinoStateWait(mino) },
            { MinoState.ControlFall, new MinoStateControlFall(mino,transform) },
            { MinoState.Tower, new MinoStateTower(mino,rigidBody,towerTag) },
        };
        _stateTable = table;
        
        // 初期状態を直接設定
        _currentState = _stateTable[initState];
        _currentState.Enter();
    }

    /// <summary>
    /// ステートチェンジ
    /// </summary>
    /// <param name="next">次のステート</param>
    public void ChangeState(MinoState next)
    {
        // 未初期化
        if (_stateTable == null) 
        {
            return;
        }

        // 同じステートには遷移しない
        if (_currentState == null || _currentState.CurrentState == next)
        {
            return;
        }

        var nextState = _stateTable[next];
        // 現在のステートを前のステートに
        _previousState = _currentState;
        // ステート退場
        _previousState?.Exit();
        // 現在のステートを変更
        _currentState = nextState;
        // ステート入場
        _currentState.Enter();
    }

    /// <summary>
    /// 現在のステートをUpdate
    /// </summary>
    public void Update() => _currentState?.Update();

    /// <summary>
    /// 衝突時の処理を現在のステートに委譲
    /// </summary>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState?.OnCollisionEnter2D(collision);
    }
}
