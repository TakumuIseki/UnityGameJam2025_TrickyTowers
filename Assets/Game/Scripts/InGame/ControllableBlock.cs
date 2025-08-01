///
/// テトリミノを制御するスクリプト。
///
using System.Runtime.CompilerServices;
using UnityEngine;

public class ControllableBlock : MonoBehaviour
{
    private Rigidbody2D rb;

    private static readonly float NORMAL_FALL_SPEED = 50f;                  // 通常時の落下速度（units/sec）
    private static readonly float FAST_FALL_SPEED = 100f;                   // 加速時の落下速度
    private static readonly float MOVE_DISTANCE_PER_KEY = 10f;              // 左右移動の幅
    private static readonly Vector3 ROTATION_ANGLE = new Vector3(0, 0, 90); // 回転角度
    private static readonly float GRAVITY = 30f;                            // 重力

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
    }

    void Update()
    {
        float fallSpeed = Input.GetKey(KeyCode.DownArrow) ? FAST_FALL_SPEED : NORMAL_FALL_SPEED;

        // 時間によって位置を下げる（transformを直接動かす）
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // 左に移動
        Move(Input.GetKeyDown(KeyCode.LeftArrow), Vector3.left);

        // 右に移動
        Move(Input.GetKeyDown(KeyCode.RightArrow), Vector3.right);

        // 回転
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(ROTATION_ANGLE);
        }
    }

    private void Move(bool getKeyDown, Vector3 direction)
    {
        if (getKeyDown)
        {
            transform.Translate(direction * MOVE_DISTANCE_PER_KEY, Space.World);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.gravityScale = GRAVITY;
        this.enabled = false;
    }
}
