using UnityEngine;

/// <summary>
/// ミノステート　：　落下操作
/// </summary>
public class MinoStateControlFall : IMinoState
{
    /// <summary>
    /// 通常時の落下速度
    /// </summary>
    private static readonly float NORMAL_FALL_SPEED = 100.0f;

    /// <summary>
    /// 加速倍率
    /// </summary>
    private static readonly float SPEED_UP_MAGNIFICATION = 2.0f;

    /// <summary>
    /// 回転角度
    /// </summary>
    private static readonly Vector3 ROTATION_ANGLE = new Vector3(0.0f, 0.0f, 90.0f);

    /// <summary>
    /// 左右移動の幅
    /// </summary>
    private static readonly float MOVE_DISTANCE_PER_KEY = 10.0f;

    /// <summary>
    /// ミノ
    /// </summary>
    Mino _mino;
    
    /// <summary>
    /// ミノのトランスフォーム
    /// </summary>
    Transform _transform;

    /// <summary>
    /// ステート
    /// </summary>
    public MinoState CurrentState => MinoState.ControlFall;

    /// <summary>
    /// 回転負荷スキルが有効か
    /// </summary>
    private bool isLockRotation_ = false;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MinoStateControlFall(Mino mino,Transform transform)
    {
        _mino = mino;
        _transform = transform;
    }

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
        // 落下速度決定
        var fallSpeed = Input.GetKey(KeyCode.DownArrow) ? NORMAL_FALL_SPEED * SPEED_UP_MAGNIFICATION : NORMAL_FALL_SPEED;

        // 時間によって位置を下げる
        _transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // 左に移動
        Move(Input.GetKeyDown(KeyCode.LeftArrow), Vector3.left);

        // 右に移動
        Move(Input.GetKeyDown(KeyCode.RightArrow), Vector3.right);

        // 回転操作
        Rotation();
    }
    
    /// <summary>
    /// ステート終了時に実行
    /// </summary>
    public void Exit()
    {
    }

    /// <summary>
    /// 左右移動処理用メソッド。
    /// </summary>
    /// <param name="getKeyDown">入力するキー</param>
    /// <param name="direction">移動方向</param>
    private void Move(bool getKeyDown, Vector3 direction)
    {
        if (getKeyDown)
        {
            _transform.Translate(direction * MOVE_DISTANCE_PER_KEY, Space.World);
        }
    }

    /// <summary>
    /// 回転
    /// </summary> 
    private void Rotation()
    {
        // 回転不可スキルが有効化されていたら回転しない。
        if (isLockRotation_)
        {
            return;
        }

        // 上キーで90度回転。
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _transform.Rotate(ROTATION_ANGLE);
        }
    }

    /// <summary>
    /// 他のオブジェクトと当たったとき
    /// </summary>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // タワーステートに変更
        _mino.TowerState();
    }
}
