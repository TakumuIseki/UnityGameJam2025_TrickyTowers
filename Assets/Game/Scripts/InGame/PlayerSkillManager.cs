using System;
using UnityEngine;

/// <summary>
/// プレイヤーのスキルマネージャー
/// </summary>
public class PlayerSkillManager : MonoBehaviour
{
    /// <summary>
    /// スキル発動に必要なポイント
    /// </summary>
    private static readonly float REQUIRED_SKILL_POINT = 60.0f;
    
    /// <summary>
    /// 無敵スキルの移動時間
    /// </summary>
    private static readonly float INVINCIBLE_DURATION = 30.0f;

    /// <summary>
    /// ミノの巨大化倍率
    /// </summary>
    private static readonly float SCALE_UP_RATIO = 2.0f;

    [Header("他プレイヤー"),SerializeField]
    private GameObject[] otherPlayers_;

    /// <summary>
    /// 現在のスキルポイント
    /// </summary>
    private float currentSkillPoint_ = 0.0f;

    /// <summary>
    /// 順位
    /// </summary>
    private int myRank_ = 1;

    /// <summary>
    /// 子のスポナー
    /// </summary>
    private SpawnMino tetriminoSpawner_;

    /// <summary>
    /// 無敵スキルが有効か
    /// </summary>
    private bool isInvincibleSkill_ = false;

    /// <summary>
    /// 無敵スキルの持続時間
    /// </summary>
    private float invincibleTimer_ = 0.0f;

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        // スキルポイントを蓄積。
        currentSkillPoint_ += myRank_ * Time.deltaTime;

        // スキルポイントが貯まったらスキルを有効化。
        if (currentSkillPoint_ >= REQUIRED_SKILL_POINT)
        {
            ActiveSkill();
        }

        // 無敵スキルの持続時間を管理。
        DeactivateInvincibleSkill();
    }

    /// <summary>
    /// スキルのUI表示と発動の処理。
    /// </summary>
    private void ActiveSkill()
    {
        // 特定のキー入力で特定のスキルを発動する。
        UseSkill(Input.GetKeyDown(KeyCode.Keypad2), CallBindSkill);
        UseSkill(Input.GetKeyDown(KeyCode.Keypad6), InvincibleSkill);
        UseSkill(Input.GetKeyDown(KeyCode.Keypad4), CallScaleUpSkillOnOthers);
        UseSkill(Input.GetKeyDown(KeyCode.Keypad8), CallLockRotationSkillOnOthers);
    }

    /// <summary>
    /// 特定のキーを入力したら特定のスキルを発動する処理。
    /// </summary>
    /// <param name="getKeyDown">入力するキーを設定。</param>
    /// <param name="skill">発動するスキルの関数を設定。</param>
    private void UseSkill(bool getKeyDown, Action skill)
    {
        if (getKeyDown)
        {
            // 引数の関数を処理する。
            skill?.Invoke();

            // ポイントを初期化。
            currentSkillPoint_ = 0.0f;
        }
    }

    /// <summary>
    /// 自身が今操作しているテトリミノが他のテトリミノと接触したとき結合させる処理を呼び出す。
    /// </summary>
    private void CallBindSkill()
    {
        GetControlledTetrimino();
        //skillTargetTetrimino_.SetIsActiveBindSkill();
        //skillTargetTetrimino_.transform.position = tetriminoSpawner_.transform.position;
    }

    /// <summary>
    /// 自身が無敵になる処理を呼び出す。
    /// </summary>
    private void InvincibleSkill()
    {
        isInvincibleSkill_ = true;
    }

    /// <summary>
    /// 他プレイヤーが操作しているテトリミノを巨大化する処理を呼び出す。
    /// </summary>
    private void CallScaleUpSkillOnOthers()
    {
        //ForEachOtherPlayerTetrimino(tetrimino => tetrimino.ScaleUpSkill());
    }

    /// <summary>
    /// 他プレイヤーが操作しているテトリミノを回転不可にする処理を呼び出す。
    /// </summary>
    private void CallLockRotationSkillOnOthers()
    {
        //ForEachOtherPlayerTetrimino(tetrimino => tetrimino.LockRotationSkill());
    }

    /// <summary>
    /// 操作中のテトリミノを取得する。
    /// </summary>
    private void GetControlledTetrimino()
    {
        // PlayerUnitの子オブジェクトからTetriminoSpawnerComponentを探す。
        tetriminoSpawner_ = GetComponentInChildren<SpawnMino>();
    }

    /// <summary>
    /// 他のプレイヤーが操作しているテトリミノに対して、指定したスキルを実行する。
    /// </summary>
    /// <param name="skill"> 実行するスキル。</param>
    private void ForEachOtherPlayerTetrimino()//Action<TetriminoControllerComponent> skill)
    {
        foreach (var unit in otherPlayers_)
        {
            // 他のプレイヤーユニットのSkillManagerComponentを取得。
            var otherSkillManager = unit.GetComponentInChildren<PlayerSkillManager>();
            if (otherSkillManager == null)
            {
                Debug.LogWarning("SkillManagerComponent が見つかりませんでした。");
                continue;
            }

            // 他のプレイヤーが無敵スキルを使っていたらスキルを発動しない。
            if (otherSkillManager.isInvincibleSkill_)
            {
                continue;
            }

            // 他のプレイヤーの操作中のテトリミノを取得。
            otherSkillManager.GetControlledTetrimino();
            //var tetrimino = otherSkillManager.skillTargetTetrimino_;
            //if (tetrimino == null)
            //{
            //    Debug.LogWarning("skillTargetTetrimino_ が null です。");
            //    continue;
            //}

            //skill?.Invoke(tetrimino);
        }
    }

    /// <summary>
    /// 一定時間が経ったら、無敵を解除する。
    /// </summary>
    private void DeactivateInvincibleSkill()
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
        //isActiveBindSkill_ = false;
    }

    /// <summary>
    /// 自分の操作しているテトリミノを巨大化する処理。
    /// </summary>
    public void ScaleUpSkill()
    {
        // 巨大化スキルを有効化。
        transform.localScale *= SCALE_UP_RATIO;

        // スポーン位置に戻す。
        transform.position = tetriminoSpawner_.transform.position;
    }

    /// <summary>
    /// 自分の操作しているテトリミノを回転不可にする処理。
    /// </summary>
    public void LockRotationSkill()
    {
        // 回転不可スキルを有効化。
        //isLockRotation_ = true;

        // スポーン位置に戻す。
        transform.position = tetriminoSpawner_.transform.position;
    }

    /// <summary>
    /// バインドスキルを有効にする。
    /// </summary>
    public void SetIsActiveBindSkill()
    {
        //isActiveBindSkill_ = true;
    }
}