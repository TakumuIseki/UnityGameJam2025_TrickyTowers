using UnityEngine;

public class ControllableBlock : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFixed = false;

    [Header("落下速度")]
    public static readonly float normalFallSpeed = 50f;  // 通常時の落下速度（units/sec）
    public static readonly float fastFallSpeed = 100f;    // 加速時の落下速度

    // ステージの大きさ
    public static readonly int width = 400;
    public static readonly int height = 800;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
    }

    void Update()
    {
        if (isFixed) return;

        float fallSpeed = Input.GetKey(KeyCode.DownArrow) ? fastFallSpeed : normalFallSpeed;

        // 時間によって位置を下げる（transformを直接動かす）
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // 左右移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * 10, Space.World);

            //// 今回追加
            //if (!ValidMovement())
            //{
            //    transform.position -= new Vector3(-10, 0, 0);
            //}
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * 10, Space.World);

            //if (!ValidMovement())
            //{
            //    transform.position -= new Vector3(10, 0, 0);
            //}
        }


        // 回転
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, 90);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFixed)
        {
            FixBlock();
        }
    }

    void FixBlock()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        isFixed = true;

        this.enabled = false;
    }

    bool ValidMovement()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            // minoがステージよりはみ出さないように制御
            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }
        }
        return true;
    }
}
