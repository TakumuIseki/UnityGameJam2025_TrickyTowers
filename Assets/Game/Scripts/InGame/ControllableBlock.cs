/// <summary>
/// テトリミノを制御するスクリプト。
/// </summary>
using UnityEngine;

public class ControllableBlock : MonoBehaviour
{
    private static readonly float NORMAL_FALL_SPEED = 50f;                  // 通常時の落下速度（units/sec）。
    private static readonly float FAST_FALL_SPEED = 100f;                   // 加速時の落下速度。
    private static readonly float MOVE_DISTANCE_PER_KEY = 10f;              // 左右移動の幅。
    private static readonly Vector3 ROTATION_ANGLE = new Vector3(0, 0, 90); // 回転角度。
    private static readonly float GRAVITY = 30f;                            // 重力。
    private static readonly float DESTROY_Y_THRESHOLD = -40f;               // このY座標を下回ったら消滅。

    private Rigidbody2D rb_;                                                 // Rigitbody2Dを格納する変数。
    private SpawnTetrimino spawner_;                                         // SpawnTetrimino型の変数。
    private bool hasCollided_ = false;                                       // 当たり判定が一度検出されたら立てるフラグ。

    [Header("付与するタグ"), SerializeField]
    private string tagToAssign_ = "Tower";

    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        rb_.bodyType = RigidbodyType2D.Dynamic;
        rb_.gravityScale = 0f;
    }

    void Update()
    {
        DestroyIfBelowThreshold();

        // 当たり判定を検出したら、操作不可にする。
        if (hasCollided_)
        {
            return;
        }

        // 落下速度を決める処理。
        float fallSpeed = Input.GetKey(KeyCode.DownArrow) ? FAST_FALL_SPEED : NORMAL_FALL_SPEED;

        // 時間によって位置を下げる（transformを直接動かす）。
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // 左に移動。
        Move(Input.GetKeyDown(KeyCode.LeftArrow), Vector3.left);

        // 右に移動。
        Move(Input.GetKeyDown(KeyCode.RightArrow), Vector3.right);

        // 回転処理。
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(ROTATION_ANGLE);
        }
    }

    /// <summary>
    /// 移動処理。
    /// </summary>
    /// <param name="getKeyDown">入力するキー。</param>
    /// <param name="direction">移動方向。</param>
    private void Move(bool getKeyDown, Vector3 direction)
    {
        if (getKeyDown)
        {
            transform.Translate(direction * MOVE_DISTANCE_PER_KEY, Space.World);
        }
    }

    /// <summary>
    /// テトリミノを生成するスポナーを設定する。
    /// </summary>
    public void SetSpawner(SpawnTetrimino spawner)
    {
        this.spawner_ = spawner;
    }

    /// <summary>
    /// 他のオブジェクトと当たったときの処理。
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 2回目以降は無視。
        if (hasCollided_)
        {
            return;
        }
        hasCollided_ = true;

        // 重力を設定する。
        rb_.gravityScale = GRAVITY;

        // 次のテトリミノをスポーンさせる。
        spawner_.Spawn();

        // タグを付与する。
        gameObject.tag = tagToAssign_;
    }

    /// <summary>
    /// テトリミノが落下した時の処理。
    /// </summary>
    private void DestroyIfBelowThreshold()
    {
        // テトリミノのy座標が一定以下になったら。
        if (transform.position.y < DESTROY_Y_THRESHOLD)
        {
            // テトリミノ自身を削除する。
            Destroy(gameObject);

            // テトリミノにtowerタグが付いていなかったら。
            if (gameObject.tag != tagToAssign_)
            {
                // 次のテトリミノをスポーンさせる。
                spawner_.Spawn();
            }
        }
    }
}
