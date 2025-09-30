/// <summary>
/// テトリミノを制御するスクリプト。
/// </summary>
using UnityEngine;

public class TetriminoControllerComponent : MonoBehaviour
{
    private static readonly float NORMAL_FALL_SPEED = 50.0f;                            // 通常時の落下速度（units/sec）。
    private static readonly float FAST_FALL_SPEED = 100.0f;                             // 加速時の落下速度。
    private static readonly float MOVE_DISTANCE_PER_KEY = 10.0f;                        // 左右移動の幅。
    private static readonly Vector3 ROTATION_ANGLE = new Vector3(0.0f, 0.0f, 90.0f);    // 回転角度。
    private static readonly float GRAVITY = 30.0f;                                      // 重力。
    private static readonly float DESTROY_Y_THRESHOLD = -40.0f;                         // このY座標を下回ったら消滅。
    private static readonly float INVINCIBLE_DURATION = 30.0f;                          // 無敵スキルの持続時間。

    [Header("自身のRigidBody2D"), SerializeField]
    private Rigidbody2D rigidBody_;

    [Header("付与するタグ"), SerializeField]
    private string tagToAssign_ = "Tower";

    private TowerHeightCalculatorComponent towerHeightCalculator_;          // TowerHeightCalculatorComponent型の変数。
    private TetriminoSpawnerComponent spawner_;                             // SpawnTetrimino型の変数。
    private bool hasCollided_ = false;                                      // 当たり判定が一度検出されたら立てるフラグ。
    private bool isActiveBindSkill_ = false;                                // バインドスキルが有効かどうか。
    private bool isInvincibleSkill_ = false;                                // 無敵スキルが有効かどうか。
    private float invincibleTimer_ = 0.0f;                                  // 無敵スキルの持続時間。
    private bool isLockRotation_ = false;                                   // 回転不可スキルが有効かどうか。

    /// <summary>
    /// バインドスキルを有効にする。
    /// </summary>
    public void SetIsActiveBindSkill()
    {
        isActiveBindSkill_ = true;
    }


    void Start()
    {
        FindAndAssignTowerHeightCalculator();
    }
    void Update()
    {
        HandleMovementAndRotation();
        DestroyIfBelowThreshold();
        DeactivateInvincibleSkill();
    }

    /// <summary>
    /// テトリミノの移動と回転処理。
    /// </summary>
    private void HandleMovementAndRotation()
    {
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

        // 回転操作。
        HandleRotation();
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
    /// 回転処理用メソッド。
    /// </summary> 
    private void HandleRotation()
    {
        // 回転不可スキルが有効化されていたら回転しない。
        if (isLockRotation_)
        {
            return;
        }

        // 上キーで90度回転。
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isLockRotation_)
        {
            transform.Rotate(ROTATION_ANGLE);
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

        // バインドスキルが有効化されていたら発動。
        if (isActiveBindSkill_)
        {
            BindWithTower(collision.gameObject);
        }

        // 重力を設定する。
        rigidBody_.gravityScale = GRAVITY;

        // 次のテトリミノをスポーンさせる。
        spawner_.SpawnTetrimino();

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
        // テトリミノのy座標が一定以下になったら、削除する。
        if (transform.position.y > DESTROY_Y_THRESHOLD)
        {
            return;
        }

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
            spawner_.SpawnTetrimino();
        }
    }


    /// <summary>
    /// 他のテトリミノと接触したときに結合させる処理。
    /// </summary>
    /// <param name="towerTetrimino"> 接触したテトリミノ。</param>
    private void BindWithTower(GameObject towerTetrimino)
    {
        if (!towerTetrimino.CompareTag("Tower"))
        {
            return;
        }

        // 自分にFixedJoint2Dを追加
        var joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = towerTetrimino.GetComponent<Rigidbody2D>();

        // 壊れないようにする
        joint.breakForce = Mathf.Infinity;
        joint.breakTorque = Mathf.Infinity;

        // ぶつかった後も押し合えるようにする
        joint.enableCollision = true;

        // バインドスキルを無効化。
        isActiveBindSkill_ = false;
    }

    /// <summary>
    /// 自身が無敵になる処理。
    /// </summary>
    public void ActivateInvincibleSkill()
    {
        isInvincibleSkill_ = true;
    }

    public void DeactivateInvincibleSkill()
    {
        if (isInvincibleSkill_)
        {
            invincibleTimer_ += Time.deltaTime;
            if (invincibleTimer_ >= INVINCIBLE_DURATION)
            {
                invincibleTimer_ = 0.0f;
                isInvincibleSkill_ = false;
            }
        }
    }

    /// <summary>
    /// 自分の操作しているテトリミノを巨大化する処理。
    /// </summary>
    public void ScaleUpSkill()
    {
        // 無敵スキルが有効化されていたら発動しない。
        if (isInvincibleSkill_)
        {
            return;
        }

        // 巨大化スキルを有効化。
        transform.localScale *= 2f;

        // スポーン位置に戻す。
        transform.position = spawner_.transform.position;
    }

    /// <summary>
    /// 自分の操作しているテトリミノを回転不可にする処理。
    /// </summary>
    public void LockRotationSkill()
    {
        // 無敵スキルが有効化されていたら発動しない。
        if (isInvincibleSkill_)
        {
            return;
        }

        // 回転不可スキルを有効化。
        isLockRotation_ = true;

        // スポーン位置に戻す。
        transform.position = spawner_.transform.position;
    }
}
