using UnityEngine;

/// <summary>
/// ミノ
/// </summary>
public class Mino : MonoBehaviour
{
    [Header("自身のRigidBody2D"), SerializeField]
    private Rigidbody2D rigidBody_;

    [Header("ミノがタワーの一部になった時のタグ"), SerializeField]
    private string towerTag_ = "Tower";

    /// <summary>
    /// スポナー
    /// </summary>
    private SpawnMino spawnMino_;

    /// <summary>s
    /// ステートパターンのContext
    /// </summary>
    private MinoStateContext _context;

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // Stateを初期化
        _context = new MinoStateContext();
        _context.Init(MinoState.Wait, this, transform, rigidBody_, towerTag_);
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update() => _context.Update();

    /// <summary>
    /// 他のオブジェクトと当たったとき
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_context == null)
        {
            return;
        }

        // ステートに衝突を通知
        _context.OnCollisionEnter2D(collision);
    }

    /// <summary>
    /// スポナーを設定
    /// </summary>
    public void SetSpawner(SpawnMino spawnMino)
    {
        spawnMino_ = spawnMino;
    }

    /// <summary>
    /// 待機ステート
    /// </summary>
    public void WaitState() => _context.ChangeState(MinoState.Wait);

    /// <summary>
    /// 落下操作ステート
    /// </summary>
    public void ControlFallState() => _context.ChangeState(MinoState.ControlFall);

    /// <summary>
    /// タワーステート
    /// </summary>
    public void TowerState() => _context.ChangeState(MinoState.Tower);

    /// <summary>
    /// 新しいミノをスポーン
    /// </summary>
    public void SpawnNewMino()
    {
        spawnMino_.Spawn();
    }

    /// <summary>
    /// 次のミノを表示
    /// </summary>
    public void ViewNextMino()
    {
        spawnMino_.ViewNextMino();
    }
}