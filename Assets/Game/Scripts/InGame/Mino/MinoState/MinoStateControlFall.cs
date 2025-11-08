using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ミノステート　：　落下操作
/// </summary>
public class MinoStateControlFall : IMinoState
{
    /// <summary>
    /// ミノ
    /// </summary>
    private Mino _mino;

    /// <summary>
    /// ミノのトランスフォーム
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// ステート
    /// </summary>
    public MinoState CurrentState => MinoState.ControlFall;

    /// <summary>
    /// 回転負荷スキルが有効か
    /// </summary>
    private bool isLockRotation_ = false;

    /// <summary>
    /// 落下速度アップしているか
    /// (ゲームパッド十字キー下方向が押されたか)
    /// </summary>
    private bool _isFallSpeedUp => _mino.PlayerInput.actions["Fall"].ReadValue<Vector2>().y < 0;

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
        // ミノObjectのタグをMinoに変更
        _mino.gameObject.tag = "Mino";

        // 入力イベント登録
        _mino.PlayerInput.actions["Move"].performed += OnMove;
        _mino.PlayerInput.actions["Rotation"].performed += OnRotation;
    }

    /// <summary>
    /// フレームごとに実行
    /// </summary>
    public void Update()
    {
        // 落下処理
        Fall();
    }
    
    /// <summary>
    /// ステート終了時に実行
    /// </summary>
    public void Exit()
    {
    }

    /// <summary>
    /// 落下処理
    /// </summary>
    private void Fall()
    {
        // 落下速度決定
        var fallSpeed = _isFallSpeedUp ? MinoConst.NORMAL_FALL_SPEED * MinoConst.SPEED_UP_MAGNIFICATION : MinoConst.NORMAL_FALL_SPEED;

        // 時間によって位置を下げる
        _transform.position += Vector3.down * fallSpeed * Time.deltaTime;

    }

    /// <summary>
    /// 移動入力
    /// </summary>
    private void OnMove(InputAction.CallbackContext context)
    {
        // performedでなければ処理しない
        // NOTE: startedやcanceledでも呼ばれるため制御が必要
        if(!context.performed)
        {
            return;
        }

        // Y方向に入力があったら移動しない
        Vector2 inputValue = context.ReadValue<Vector2>();
        if(Mathf.Abs(inputValue.y) > Mathf.Epsilon)
        {
            return;
        }

        // trueなら右移動、falseなら左移動
        var direction = inputValue.x < 0 ? Vector3.left : Vector3.right;
        Move(direction);
    }

    /// <summary>
    /// 回転入力
    /// </summary>
    private void OnRotation(InputAction.CallbackContext context)
    {
        // performedでなければ処理しない
        // NOTE: startedやcanceledでも呼ばれるため制御が必要
        if (!context.performed)
        {
            return;
        }

        // X方向に入力があったら回転しない
        Vector2 inputValue = context.ReadValue<Vector2>();
        if(Mathf.Abs(inputValue.x) > Mathf.Epsilon)
        {
            return;
        }

        // Y方向が下方向なら回転しない
        if(inputValue.y <= 0)
        {
            return;
        }

        // 回転処理
        Rotation();
    }

    /// <summary>
    /// 左右移動
    /// </summary>
    /// <param name="direction">移動方向</param>
    private void Move(Vector3 direction)
    {
        _transform.Translate(direction * MinoConst.MOVE_DISTANCE_PER_KEY, Space.World);
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

        // 回転SE再生
        SoundManager.PlaySE("SeMinoRotation");

        _transform.Rotate(MinoConst.ROTATION_ANGLE);
    }

    /// <summary>
    /// 他のオブジェクトと当たったとき
    /// </summary>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // イベント登録解除
        _mino.PlayerInput.actions["Move"].performed -= OnMove;
        _mino.PlayerInput.actions["Rotation"].performed -= OnRotation;

        // 接着SE再生
        SoundManager.PlaySE("SeMinoInstallation");

        // タワーステートに変更
        _mino.TowerState();
    }
}
