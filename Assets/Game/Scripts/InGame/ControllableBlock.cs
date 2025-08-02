/// <summary>
/// テトリミノを制御するスクリプト。
/// </summary>
using System.Runtime.CompilerServices;
using UnityEngine;

public class ControllableBlock : MonoBehaviour
{
    private Rigidbody2D rb;                                                 // Rigitbody2Dを格納する変数。

    private static readonly float NORMAL_FALL_SPEED = 50f;                  // 通常時の落下速度（units/sec）。
    private static readonly float FAST_FALL_SPEED = 100f;                   // 加速時の落下速度。
    private static readonly float MOVE_DISTANCE_PER_KEY = 10f;              // 左右移動の幅。
    private static readonly Vector3 ROTATION_ANGLE = new Vector3(0, 0, 90); // 回転角度。
    private static readonly float GRAVITY = 30f;                            // 重力。

    private SpawnTetrimino spawner;                                         // SpawnTetrimino型の変数。
    private bool hasCollided = false;                                       // 当たり判定が一度検出されたら立てるフラグ。

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
    }

    void Update()
    {
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
        this.spawner = spawner;
    }

    /// <summary>
    /// 他のオブジェクトと当たったときの処理。
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 2回目以降は無視。
        if (hasCollided)
        {
            return;
        }
        hasCollided = true;

        // 重力を設定する。
        rb.gravityScale = GRAVITY;

        // 操作不可にさせる。
        this.enabled = false;

        // 次のテトリミノをスポーンさせる。
        spawner.Spawn();
    }
}
