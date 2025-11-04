using UnityEngine;

/// <summary>
/// ミノステート　：　タワー
/// </summary>
public class MinoStateTower : IMinoState
{
    /// <summary>
    /// ミノ
    /// </summary>
    Mino _mino;

    /// <summary>
    /// 剛体
    /// </summary>
    Rigidbody2D rigidbody_;

    /// <summary>
    /// タワータグ
    /// </summary>
    private string towerTag_;

    /// <summary>
    /// ステート
    /// </summary>
    public MinoState CurrentState => MinoState.Tower;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MinoStateTower(Mino mino, Rigidbody2D rigidbody, string towerTag)
    {
        _mino = mino;
        rigidbody_ = rigidbody;
        towerTag_ = towerTag;
    }

    /// <summary>
    /// ステート開始時に実行
    /// </summary>
    public void Enter()
    {
        // バインドスキルが有効化されていたら発動
        //if (isActiveBindSkill_)
        //{
        //    BindWithTower(collision.gameObject);
        //}

        // 重力を設定
        rigidbody_.gravityScale = MinoConst.GRAVITY;

        // タワータグを付与する
        _mino.gameObject.tag = towerTag_;

        // 新しいミノをスポーン
        _mino.SpawnNewMino();
        // その次のミノを表示
        _mino.ViewNextMino();
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
