/// <summary>
/// テトリミノを制御するスクリプト。
/// </summary>
using UnityEngine;

public class TetriminoControllerComponent : MonoBehaviour
{
    private static readonly float NORMAL_FALL_SPEED = 50f;                  // 通常時の落下速度（units/sec）。
    private static readonly float FAST_FALL_SPEED = 100f;                   // 加速時の落下速度。
    private static readonly float MOVE_DISTANCE_PER_KEY = 10f;              // 左右移動の幅。
    private static readonly Vector3 ROTATION_ANGLE = new Vector3(0, 0, 90); // 回転角度。
    private static readonly float GRAVITY = 30f;                            // 重力。
    private static readonly float DESTROY_Y_THRESHOLD = -40f;               // このY座標を下回ったら消滅。

    [Header("自身のRigidBody2D"), SerializeField]
    private Rigidbody2D rigidBody_;

    [Header("付与するタグ"), SerializeField]
    private string tagToAssign_ = "Tower";

    private TowerHeightCalculatorComponent towerHeightCalculator_;          // TowerHeightCalculatorComponent型の変数。
    private TetriminoSpawnerComponent spawner_;                             // SpawnTetrimino型の変数。
    private bool hasCollided_ = false;                                      // 当たり判定が一度検出されたら立てるフラグ。

    void Start()
    {
        FindAndAssignTowerHeightCalculator();
    }
    void Update()
    {
        // 当たり判定を検出したら、操作不可にする。
        if (!hasCollided_)
        {
            HandleMovementAndRotation();
        }

        // テトリミノのy座標が一定以下になったら、削除する。
        if (transform.position.y < DESTROY_Y_THRESHOLD)
        {
            DestroyIfBelowThreshold();
        }
    }

    /// <summary>
    /// テトリミノの移動と回転処理。
    /// </summary>
    private void HandleMovementAndRotation()
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
    /// 左右移動処理用メソッド。
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
    public void SetSpawner(TetriminoSpawnerComponent spawner)
    {
        spawner_ = spawner;
    }

    /// <summary>
    /// 同じPlayerUnitに属するHeightを取得。
    /// </summary>
    private void FindAndAssignTowerHeightCalculator()
    {
        // 自分が属しているPlayerUnitを取得。
        // (テトリミノ→スポナー→プレイヤーユニット)
        var playerUnit = transform.parent.parent;

        // 自分が属しているPlayerUnitの中から、
        // TowerHeightCalculatorComponentを取得してセット。
        towerHeightCalculator_ = playerUnit.GetComponentInChildren<TowerHeightCalculatorComponent>();

        // towerHeightCalculator_が見つからない場合は警告。
        if (towerHeightCalculator_ == null)
        {
            Debug.LogError($"towerHeightCalculator_が見つかりません。" +
                $"PlayerUnitとの親子関係やTowerHeightCalculatorComponentが" +
                $"PlayerUnit内でアタッチされているかを確認してください。");
        }
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
        rigidBody_.gravityScale = GRAVITY;

        // 次のテトリミノをスポーンさせる。
        spawner_.Spawn();

        // タグを付与する。
        gameObject.tag = tagToAssign_;

        // Towerタグを持ったテトリミノのリストを更新。
        towerHeightCalculator_.FindTetriminosWithTagTower();
    }

    /// <summary>
    /// テトリミノが落下した時の処理。
    /// </summary>
    private void DestroyIfBelowThreshold()
    {
        // テトリミノ自身を削除する。
        Destroy(gameObject);

        // テトリミノにtowerタグが付いていたら。
        if (gameObject.tag == tagToAssign_)
        {
            // Heightが持つ、Towerタグを持ったテトリミノのリストを更新。
            towerHeightCalculator_.FindTetriminosWithTagTower();
        }
        else
        {
            // 次のテトリミノをスポーンさせる。
            spawner_.Spawn();
        }
    }
}
