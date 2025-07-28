using UnityEngine;

public class ControllableBlock : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFixed = false;

    [Header("�������x")]
    public float normalFallSpeed = 1f;  // �ʏ펞�̗������x�iunits/sec�j
    public float fastFallSpeed = 5f;    // �������̗������x

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

        // ���Ԃɂ���Ĉʒu��������itransform�𒼐ړ������j
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // ���E�ړ��i�K�v�Ȃ�j
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left, Space.World);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right, Space.World);
        }


        // ��]�i�K�v�Ȃ�j
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
