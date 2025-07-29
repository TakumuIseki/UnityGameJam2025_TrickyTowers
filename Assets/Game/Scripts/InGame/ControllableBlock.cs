using UnityEngine;

public class ControllableBlock : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFixed = false;

    [Header("落下速度")]
    public float normalFallSpeed = 1f;  // 通常時の落下速度（units/sec）
    public float fastFallSpeed = 5f;    // 加速時の落下速度

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

        // 左右移動（必要なら）
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left, Space.World);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right, Space.World);
        }


        // 回転（必要なら）
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
}
