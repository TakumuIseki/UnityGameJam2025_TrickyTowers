using UnityEngine;

public class TetrominoController : MonoBehaviour
{
    public PlayerUnit owner;
    private Rigidbody2D rb;
    private bool hasLanded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasLanded && collision.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;
            rb.bodyType = RigidbodyType2D.Static;

            // 落下完了を通知
            owner.OnTetrominoLanded();
        }
    }
}
